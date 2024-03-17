import React, {useEffect, useState} from 'react';
import {BiSolidDislike, BiSolidLike} from "react-icons/bi";
import axios from "axios";
import {ApiServerBaseUrl} from "../../../configProvider.ts";
import {VideoCategory} from "../../Model/VideoUploadDTO.ts";
import {Video} from "../../Model/Video.ts";
import {useStoreState} from "../../../persistenceProvider.ts";
import toast from "react-hot-toast";
import * as http2 from "http2";

// Mock fetch function to simulate fetching data from the backend


// Styles
const styles = {
    videoList: {
        display: 'flex',
        flexWrap: 'wrap',
        gap: '20px',
        justifyContent: 'center',
        padding: '20px',
    },
    videoItem: {
        color: '#333',
        width: '300px',
        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
        borderRadius: '10px',
        overflow: 'hidden',
        backgroundColor: '#fff',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
    },
    videoThumbnail: {
        width: '100%',
        height: '180px',
        objectFit: 'cover',
    },
    videoContent: {
        padding: '15px',
    },
    videoTitle: {
        fontSize: '18px',
        fontWeight: 'bold',
        marginBottom: '10px',
    },
    videoDescription: {
        fontSize: '14px',
        marginBottom: '10px',
    },
    videoStats: {
        display: 'flex',
        alignItems: 'flex-end',
        flexDirection: 'column',
        fontSize: '14px',
        marginBottom: '5px',
    },
    videoUploadDate: {
        fontSize: '12px',
        color: '#666',
    },
};

const truncateDescription = (description: string) => {
    return description.length > 300 ? description.substring(0, 300) + '...' : description;
};

// VideoItem component for rendering individual video details
const VideoItem = ({video}) => (
    <div style={styles.videoItem}>
        <img src={video.thumbnailUri} alt={video.title} style={styles.videoThumbnail}/>
        <div style={styles.videoContent}>
            <h3 style={styles.videoTitle}>{video.title}</h3>
            <p style={styles.videoDescription}>{truncateDescription(video.description)}</p>
            <div style={{display: "flex", justifyContent: "space-between"}}>
                <div
                    style={styles.videoUploadDate}>Uploaded: {new Date(video.uploadDateTime).toLocaleDateString()}</div>
                <div style={styles.videoStats}>
                    <div style={{display: "flex", alignItems: "center", flexDirection: "column"}}>
                        <div style={{display: "flex"}}>
                            <BiSolidLike/><BiSolidDislike/>
                        </div>
                        <div>
                            {video.totalLikes}/{video.totalDislikes}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
);

// VideoList component for rendering the list of videos
const VideoListCarosel = ({categoryName, isHotVideos}) => {

    const [videos, setVideos] = useState<Video[]>([]);
    const authToken = useStoreState("authToken");
    const fetchRecommendedVideos = async (category: VideoCategory, topN = 20): Promise<Video[]> => {
        try {
            const response = await axios.get(ApiServerBaseUrl() + '/videolib/recommendedVideos', {
                params: {
                    category: category,
                    topN: topN,
                },
                headers: {
                    Authorization: `Bearer ${authToken}`
                }
            });
            console.log(response.data);
            return response.data; // The list of recommended videos
        } catch (error) {
            toast.error('Failed to fetch recommended videos');
            return [];
        }
    };

    
    const fetchHotVideos = async (topN = 20): Promise<Video[]> => {
        try {
            const response = await axios.get(ApiServerBaseUrl() + '/videolib/hotVideos', {
                params: {
                    topN: topN,
                },
                headers: {
                    Authorization: `Bearer ${authToken}`
                }
            });
            console.log(response.data);
            return response.data; // The list of hot videos
        } catch (error) {
            toast.error('Failed to fetch hot videos');
            return [];
        }
    }
    

    useEffect(() => {
        if (isHotVideos) {
            fetchHotVideos().then(videos => setVideos(videos));
        } else
        fetchRecommendedVideos(categoryName).then(videos => setVideos(videos));
    }, []);

    return (
        <div>
            {isHotVideos?<h2>HotVideos</h2> : <h2>{categoryName == VideoCategory.Other ? 'Recommended': categoryName}</h2>}
            <div style={styles.videoList}>
                {videos.map(video => (
                    <VideoItem key={video.videoId} video={video}/>
                ))}
            </div>
        </div>
    );
};

export default VideoListCarosel;
