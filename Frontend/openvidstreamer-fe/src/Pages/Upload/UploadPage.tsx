import React, {useState} from 'react';
import axios from 'axios';
import './VideoUploadForm.css';
import {VideoCategory, VideoCategoryNames, VideoUploadDTO} from "../../Model/VideoUploadDTO.ts";
import {Label} from "flowbite-react"; // Assuming your CSS is in this file

const UploadPage = () => {
    const [videoMetadata, setVideoMetadata] = useState<VideoUploadDTO>({
        title: '',
        description: '',
        category: VideoCategory.Other, // Default category
       
    });
    const [videoFile, setVideoFile] = useState(null);
    const [thumbnailFile, setThumbnailFile] = useState(null);
    const [uploadPercentage, setUploadPercentage] = useState(0);

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
            await axios.post('https://localhost:5008/upload', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                onUploadProgress: progressEvent => {
                    const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
                    setUploadPercentage(percentCompleted);
                }
            });
            alert('Video uploaded successfully!');
        } catch (error) {
            alert(`Upload failed: ${error.response.data}`);
        }

        retu
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
                <input style={{color: "white"}}
                       type="file"
                       name="videoFile"
                       onChange={handleVideoFileChange}
                />
                <Label>Thumbnail File</Label>
                <input style={{color: "white"}}
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
        </div>
    );
};

export default UploadPage;
