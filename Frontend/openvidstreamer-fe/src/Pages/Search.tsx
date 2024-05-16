import React, {useEffect, useState} from 'react';
import {useLocation} from "react-router-dom";
import {Video} from "../Model/Video.ts";
import {GetVideoCategoryEnumIndex, VideoCategory} from "../Model/VideoUploadDTO.ts";
import axios from "axios";
import {ApiServerBaseUrl} from "../../configProvider.ts";
import toast from "react-hot-toast";
import {useStoreState} from "../persistenceProvider.ts";
import {stylesVideos, VideoItem} from "./VideoRecomendations/VideoRecomendations.tsx";

const Search = () => {
    const location = useLocation();
    const searchQuerry: string = location.state.searchQuery;

    console.log(searchQuerry);
    
    const authToken = useStoreState('authToken');

    const [videos, setVideos] = useState<Video[]>([])
    
    const fetchVideos = async (): Promise<Video[]> => {
        if (!searchQuerry) {
            toast('No search query provided',{ position: "top-right"})
            return [];} // Avoids unnecessary API calls
        let toastSearchingId;
        try {
             toastSearchingId = toast.loading('Searching...',{ position: "top-right"})
            const response = await axios.get(ApiServerBaseUrl() + '/videolib/searchVideos', {
                params: {
                    searchQuery: searchQuerry,
                    
                },
                headers: {
                    Authorization: `Bearer ${authToken}`
                }
            });
            console.log(response.data);
            setVideos(response.data);
            toast.dismiss(toastSearchingId);
            return response.data; 
        } catch (error) {
            toast.dismiss(toastSearchingId);
            toast.error('Failed to fetch Search Result Videos');
            return [];
        }
    };




    useEffect(() => {
        fetchVideos();
    }, [searchQuerry]);
    
    
    return (
        <div  style={{height:"calc(100vh - 70px)", overflowY:"scroll"}}>
            <div>
            <h2 style={{fontWeight: 700, margin: 20, marginBottom: 0}}>Search Results 🔎</h2> 
                
            <div style={stylesVideos.videoList}>
                {videos.map(video => (
                    <VideoItem key={video.videoId} video={video}/>
                ))}
            </div>
            <hr/>
            <br/>
            </div>
        </div>
    );
};

export default Search;
