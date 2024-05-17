import  {useState} from 'react';
import axios from 'axios';
import './VideoUploadForm.css';
import {VideoCategory, VideoCategoryNames, VideoUploadDTO} from "../../Model/VideoUploadDTO.ts";
import {Label} from "flowbite-react";
import {ApiServerBaseUrl} from "../../../configProvider.ts";
import {useStoreState} from "../../persistenceProvider.ts"; 
import {
    Flex,
    Box,
    FormControl,
    FormLabel,
    Input,
    Textarea,
    Select,
    Button,
    Progress,
    VStack, Heading
} from '@chakra-ui/react';

const UploadPage = () => {
    const [videoMetadata, setVideoMetadata] = useState<VideoUploadDTO>({
        title: '',
        description: '',
        category: VideoCategory.Other, // Default category
       
    });
    const [videoFile, setVideoFile] = useState(null);
    const [thumbnailFile, setThumbnailFile] = useState(null);
    const [uploadPercentage, setUploadPercentage] = useState(0);

    const authToken = useStoreState('authToken');
    
    const handleInputChange = (e) => {
        setVideoMetadata({
            ...videoMetadata,
            [e.target.name]: e.target.value
        });
    };

    const handleVideoFileChange = (e) => {
        setVideoFile(e.target.files[0]);
    };

    const handleThumbnailFileChange = (e) => {
        setThumbnailFile(e.target.files[0]);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        
        const formData = new FormData();
        formData.append('videoMetadata', JSON.stringify(videoMetadata));
        formData.append('videoFile', videoFile);
        formData.append('thumbnailFile', thumbnailFile);

        try {
            await axios.post(ApiServerBaseUrl()+ '/upload/upload', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    "Authorization": "Bearer " + authToken
                },
                onUploadProgress: progressEvent => {
                    const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
                    setUploadPercentage(percentCompleted);
                    console.log("Upload progress: " + percentCompleted + "%")
                }
            });
            alert('Video uploaded successfully!');
        } catch (error) {
            alert(`Upload failed: ${error.response.data}`);
        }

        
        // Reset progress bar after a short delay
        setTimeout(() => setUploadPercentage(0), 1500);
    };

    return (
        <div style={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100vh"
        }}>
        <VStack marginTop={-20} spacing={4}>
            <Box backgroundColor={"#fff"} p={4} maxW="sm" borderWidth="1px" borderRadius="lg" boxShadow="lg" m="20px auto">
                <Heading as="h2" size="lg" textAlign="center">Upload Video</Heading>
            
        <form onSubmit={handleSubmit} style={{color:"black"}} className="upload-form">
            <div className="input-group">
                <Label>Title</Label>
                <input
                    type="text"
                    name="title"
                    placeholder="Title"
                    value={videoMetadata.title}
                    onChange={handleInputChange}
                />
            </div>
            <div className="input-group">
                <Label>Description</Label>
                <textarea
                    type="text"
                    name="description"
                    placeholder="Description"
                    value={videoMetadata.description}
                    onChange={handleInputChange}
                />
            </div>
            <div className="input-group">
                <Label>Category</Label>
                <select
                    name="category"
                    value={videoMetadata.category}
                    onChange={e => {
                        setVideoMetadata({
                            ...videoMetadata,
                            category: e.target.value
                        });
                    }}
                >
                    {Object.entries(VideoCategoryNames).map(([key, value]) => (
                        <option value={key} key={key}>
                            {value}
                        </option>
                    ))}
                </select>
            </div>

            <div className="file-input-group">
                <Label>Video File</Label>
                <input 
                       type="file"
                       name="videoFile"
                       onChange={handleVideoFileChange}
                />
                <Label>Thumbnail File</Label>
                <input 
                    type="file"
                    name="thumbnailFile"
                    onChange={handleThumbnailFileChange}
                />
            </div>
            <button type="submit" className="submit-btn">Upload Video</button>
            {uploadPercentage > 0 && (
                <div className="progress-bar">
                    <div className="progress-bar-fill" style={{ width: `${uploadPercentage}%` }}>{uploadPercentage}%</div>
                </div>
            )}
        </form>
            </Box>
        </VStack>
        </div>
    );
};

export default UploadPage;
