import {useEffect, useState} from 'react';
import {VideoCategory} from "../../Model/VideoUploadDTO.ts";
import VideoListCarosel from "./VideoRecomendations.tsx";
import axios from "axios";
import {ApiServerBaseUrl} from "../../../configProvider.ts";
import {useStoreState} from "../../../persistenceProvider.ts";
import {getCategoryName} from "../../Model/Video.ts";

const HomePage = () => {

    const [favoriteCategories, setFavoriteCategories] = useState<VideoCategory[]>([]);
    const authToken = useStoreState("authToken");
    const getUserFavoriteCategories = async (): Promise<VideoCategory[]> => {
        const resp = await axios.get(ApiServerBaseUrl() + '/recommendationAlgo/preferedCategory', {
                headers: {
                    Authorization: `Bearer ${authToken}`

                },
                params: {
                    topN: 5,
                }
            }
        )
        console.log(resp.data);

        return resp.data;
        // Fetch user favorite categories
    }

    useEffect(() => {
        
        getUserFavoriteCategories().then(categories => setFavoriteCategories(categories));
    }, []);


    return (
        
        <div style={{overflowY:"scroll", height:"100vh"}}>
            <VideoListCarosel isHotVideos={false} categoryName={VideoCategory.Other}/>
            <VideoListCarosel isHotVideos={true}  categoryName={VideoCategory.Other}/>
            {favoriteCategories.length >0 ? favoriteCategories.map((category) => (
                <VideoListCarosel isHotVideos={false} key={category} categoryName={getCategoryName(category)} />
            
                ))
            :<p>loading</p>
            }
        </div>
    );
};

export default HomePage;
