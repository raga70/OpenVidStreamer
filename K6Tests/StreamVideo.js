import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    stages: [
        { duration: '2m', target: 10000 }, // Ramp up to 10 000 users over 2 minute
        { duration: '7m', target: 10000 }, // Stay at 10000 CPS for 7min
        { duration: '1m', target: 0 },  // Ramp down to 0 users over 1 minute
    ],
    thresholds: {
        'http_req_duration': ['p(90)<10000'] // 89% of requests must complete below 10sec each HSL video chunc is 10sec long, and if it`s not loaded in 10sec it will result in loading for the user
    },
    ext: {
        loadimpact: {
          // Project: OpenVidStreamer
          projectID: 3693023,
          // Test runs with the same name groups test runs together.
          name: 'StreamVideo',
        }
      }
};

export default function () {
    const params = {
        headers: {
            'Authorization': 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1cG4iOiIwOGRjNDIwNy1mZWMwLTRlOTctOGY4MC0zYWU1MDUyYTMwNWEiLCJzdWIiOiIwOGRjNDIwNy1mZWMwLTRlOTctOGY4MC0zYWU1MDUyYTMwNWEiLCJqdGkiOiIwNWEyM2JiMS02NThhLTRjODMtOTgyNS1lMzM4MWZjNGViOTQiLCJleHAiOjE3MTQ5ODY2MTAsImlzcyI6Ik9wZW5WaWRTdHJlYW1lckFjY291bnRTZXJ2aWNlIiwiYXVkIjoiT3BlblZpZFN0cmVhbWVyRkUifQ.j5DQGFXBZovCMj_vQVmOyd9fv0DwtyiMhbZ0pe6wwok'
        }
       
    };

    let response = http.get('http://145.220.74.148:8000/streamer/videos/20240409/43a3308d-e879-4954-857c-f45171e1f27f/playlist3.ts', params);

    // Optionally, you can check the response and log errors
    if (response.status !== 200) {
        console.log('Unexpected status ' + response.status);
    }

   // sleep(1); // Pause for 1 second between iterations
}
