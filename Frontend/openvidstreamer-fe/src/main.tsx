import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import {loadConfig} from "../configProvider.ts";
import toast from "react-hot-toast";
import axios from "axios";
import {dispatch} from "./persistenceProvider.ts";


(async () => {
    await loadConfig()

    axios.interceptors.response.use(
        function (response) {
            // Any status code that lie within the range of 2xx cause this function to trigger
            return response;
        },
        function (error) {
            // Any status codes that falls outside the range of 2xx cause this function to trigger

            // Handle 401 Unauthorized
            if (error.response && error.response.status === 401) {
                toast("Your session has expired. you will need to login again,Please wait...")
                dispatch({type: 'setAuthToken', authToken: null});
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            } else if (error.response && error.response.status === 522) {
                toast("Document is no longer available, Please wait...")
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            } else if (error.response && (error.response.status === 483 || error.response.status === 518 || error.response.status === 519 || error.response.status === 521 || error.response.status === 517)) {
                //ingore errors that are handled inside the component
            } else {
                if (error.response.data)
                    toast.error("Network error: Status Code " + error?.response?.status + " \n"+ error?.response?.data)

            }


            return Promise.reject(error);
        }
    );
    


ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)

})()