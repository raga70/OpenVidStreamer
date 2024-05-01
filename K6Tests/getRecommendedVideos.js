import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    stages: [
        { duration: '1m', target: 100 }, // Ramp up to 100 users over 1 minute
        { duration: '1m', target: 100 }, // Stay at 100 users (rps) for 10 minutes
        { duration: '1m', target: 0 },  // Ramp down to 0 users over 1 minute
    ],
    thresholds: {
       'http_req_duration': ['p(90)<700'] // 90% of requests must complete below 700ms
    }
};

export default function () {
    const params = {
        headers: {
            'Authorization': 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1cG4iOiIwOGRjNDIwNy1mZWMwLTRlOTctOGY4MC0zYWU1MDUyYTMwNWEiLCJzdWIiOiIwOGRjNDIwNy1mZWMwLTRlOTctOGY4MC0zYWU1MDUyYTMwNWEiLCJqdGkiOiIyNWYwNjgxZC04NzMwLTQ4ZDgtODIyMC0xMzYyMzAwNGVhMmIiLCJleHAiOjE3MTQ1NzE3MTAsImlzcyI6Ik9wZW5WaWRTdHJlYW1lckFjY291bnRTZXJ2aWNlIiwiYXVkIjoiT3BlblZpZFN0cmVhbWVyRkUifQ.LV4iObB6OXV66kdx9caprcWawHbgAHG0HmyYEvuuxmk'
        }
       
    };

    let response = http.get('http://145.220.74.148:8000/videolib/recommendedVideos?category=Other&topN=20', params);

    // Optionally, you can check the response and log errors
    if (response.status !== 200) {
        console.log('Unexpected status ' + response.status);
    }

   // sleep(1); // Pause for 1 second between iterations
}
