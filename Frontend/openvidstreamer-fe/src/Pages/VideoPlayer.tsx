import React, {useEffect, useRef, useState} from 'react';
import Hls from 'hls.js';
import {ApiServerBaseUrl} from "../../configProvider.ts";
import {useStoreState} from "../persistenceProvider.ts";
import {useLocation} from "react-router-dom";
import axios from "axios";
import {getCategoryName, Video} from "../Model/Video.ts";

import {AiOutlineDislike, AiOutlineLike} from "react-icons/ai";
import {VideoCategory} from "../Model/VideoUploadDTO.ts";
const VideoPlayer = () => {
    const location = useLocation();
    const video: Video = location.state.video;

    const authToken = useStoreState('authToken');
    
    const [likes, setLikes] = useState(video.totalLikes);
    const [dislikes, setDislikes] = useState(video.totalDislikes);
    
    const videoRef = useRef(null);
    

    console.log(video);
    
    useEffect(() => {
        console.log(authToken)
        const videoElement = videoRef.current;
        let hls;

        if (Hls.isSupported() && videoElement) {
            hls = new Hls({
                xhrSetup: (xhr, url) => {
                    // Apply the authorization token to each request
                    xhr.setRequestHeader('Authorization', `Bearer ${authToken}`);
                },
            });

            hls.loadSource(ApiServerBaseUrl() + '/streamer/videos/' + video.videoUri);
            hls.attachMedia(videoElement);
            hls.on(Hls.Events.MANIFEST_PARSED, function () {
                videoElement.play();
            });
        } else if (videoElement && videoElement.canPlayType('application/vnd.apple.mpegurl')) {
            // Native support for HLS (like Safari)
            videoElement.src = ApiServerBaseUrl() + '/streamer/videos/' + video.videoUri;
            videoElement.addEventListener('loadedmetadata', () => {
                videoElement.play();
            });
        }

        // Cleanup
        return () => {
            if (hls) {
                hls.destroy();
            }
        };
    }, [video.videoUri, authToken]);



    const handleLike = async () => {
        try {
            const response = await axios.post(ApiServerBaseUrl()+ `/recommendationAlgo/likeVideo`, {}, {
                params: {
                   videoId: video.videoId
               },
               headers: {
                   Authorization: `Bearer ${authToken}`
               }});
           updateLikes();
        } catch (error) {
            console.error('Error liking the video', error);
            // Handle error (e.g., show a message)
        }
    };

    // Dislike function
    const handleDislike = async () => {
        try {
            const response = await axios.post(ApiServerBaseUrl()+ `/recommendationAlgo/dislikeVideo/`, {},{
                params: {
                    videoId: video.videoId
                },
                headers: {
                Authorization: `Bearer ${authToken}`
            }});
            updateLikes();
        } catch (error) {
            console.error('Error disliking the video', error);
            // Handle error
        }
    };
    
    const updateLikes = async () => {
        try {
            const response = await axios.get(ApiServerBaseUrl()+ `/videolib/`+ video.videoId , {
                params: {
                    videoId: video.videoId
                },
                headers: {
                    Authorization: `Bearer ${authToken}`
                }
            });
            
           
            
            setLikes(response.data.totalLikes);
            setDislikes(response.data.totalDislikes);
        } catch (error) {
            console.error('Error fetching video stats', error);
            // Handle error
        }
    }

 


    return (
        <body style={{overflowY:"scroll", maxHeight:"100vh"}}>
        <div style={{fontFamily: 'Arial, sans-serif', maxWidth: '70%', margin: 'auto', padding: '20px'}}>
            <div style={{boxShadow: '0 4px 8px rgba(0,0,0,0.1)', marginBottom: '20px'}}>
                <video ref={videoRef} controls style={{width: '100%', display: 'block'}}>
                    Your browser does not support HLS. Please update your browser or use a different one.
                </video>
            </div>
            <h2 style={{margin: '10px 0', fontSize: '24px'}}>{video.title}</h2>
            <p style={{fontSize: '16px'}}>{video.description}</p>
            <p style={{fontSize: '14px', color: '#666'}}> Category: {getCategoryName(video.category)}    • Uploaded
                on: {new Date(video.uploadDateTime).toLocaleDateString()}</p>
            <div>
                <button onClick={handleLike} style={{
                    marginRight: '10px',
                    padding: '10px 15px',
                  
                    color: 'white',
                    border: 'none',
                    borderRadius: '5px',
                    cursor: 'pointer'
                }}><AiOutlineLike /> Like ({likes})
                </button>
                <button onClick={handleDislike} style={{
                    padding: '10px 15px',
                    color: 'white',
                    border: 'none',
                    borderRadius: '5px',
                    cursor: 'pointer'
                }}><AiOutlineDislike /> Dislike ({dislikes})
                </button>
            </div>
        </div>
        </body>
    );
};

export default VideoPlayer;
