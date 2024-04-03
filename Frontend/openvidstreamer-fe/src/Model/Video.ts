import {VideoCategory} from "./VideoUploadDTO.ts";

export type Video = {
    videoId: string;
    title: string;
    description: string;
    category: VideoCategory;
    videoLength: number;
    videoUri: string;
    thumbnailUri: string;
    totalLikes: number;
    totalDislikes: number;
    uploadDateTime: string; 
}

export function getCategoryName(categoryId: number): string {
    const categories = Object.entries(VideoCategory).map(([, value]) => value);
    return categories[categoryId] ?? "Unknown Category";
}
