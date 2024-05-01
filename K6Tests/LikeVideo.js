import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    stages: [
        { duration: '1m', target: 2000 }, // Ramp up to 100 users over 1 minute
        { duration: '2m', target: 20 }, // Stay at 20 users for 2 minutes
        { duration: '1m', target: 0 },  // Ramp down to 0 users over 1 minute
    ],
    thresholds: {
        'http_req_duration': ['p(95)<500'] // 95% of requests must complete below 500ms
    },
    ext: {
        loadimpact: {
          // Project: OpenVidStreamer
          projectID: 3693023,
          // Test runs with the same name groups test runs together.
          name: 'LikeVideo',
        }
      }
};

export default function () {
    const params = {
        headers: {
            'Authorization': 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1cG4iOiIwOGRjNDIwNy1mZWMwLTRlOTctOGY4MC0zYWU1MDUyYTMwNWEiLCJzdWIiOiIwOGRjNDIwNy1mZWMwLTRlOTctOGY4MC0zYWU1MDUyYTMwNWEiLCJqdGkiOiIxYTM5ZDI0Ny02NjkwLTRkYzItODFhNi1hMTljZmI0MWI4MjIiLCJleHAiOjE3MTM3ODgyMzgsImlzcyI6Ik9wZW5WaWRTdHJlYW1lckFjY291bnRTZXJ2aWNlIiwiYXVkIjoiT3BlblZpZFN0cmVhbWVyRkUifQ.Z8u7QhROGm9jjr0Ih9s4weNHlialLU08dGnCRDo36ug'
        }
       
    };

    let response = http.get('http://145.220.74.148:8000/recommendationAlgo/likeVideo?videoId=1bf7a7ce-a9bf-47b2-b9f9-23877f82ec66', params);

    // Optionally, you can check the response and log errors
    if (response.status !== 200) {
        console.log('Unexpected status ' + response.status);
    }

    sleep(1); // Pause for 1 second between iterations
}
