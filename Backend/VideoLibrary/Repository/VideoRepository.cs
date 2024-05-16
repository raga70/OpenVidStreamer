using Microsoft.EntityFrameworkCore;
using OpenVisStreamer.VideoLibrary.Repository.EFC;
using OpenVisStreamer.VideoLibrary.Repository.Entities;

namespace OpenVisStreamer.VideoLibrary.Repository
{
    public class VideoRepository(DatabaseContext context)
    {
        public async Task Create(Video video)
        {
            context.Videos.Add(video);
            await context.SaveChangesAsync();
        }

        public async Task<Video> GetVideoById(Guid videoId)
        {
            return await context.Videos.FindAsync(videoId);
        }

        public async Task Update(Video video)
        {
            context.Videos.Update(video);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid videoId)
        {
            var video = await context.Videos.FindAsync(videoId);
            if (video != null)
            {
                context.Videos.Remove(video);
                await context.SaveChangesAsync();
            }
        }


        public async Task<List<Video>> GetVideosByVideoIds(List<Guid> videoIds)
        {
            return await context.Videos.Where(v => videoIds.Contains(v.VideoId)).ToListAsync();
        }


        public async Task<Video?> UpdateVideoToPublic(Guid videoId, decimal videoLength)
        {
            var video = await context.Videos.FindAsync(videoId);
            if (video != null)
            {
                video.IsPublic = true;
                video.videoLength = videoLength;
                context.Videos.Update(video);
                await context.SaveChangesAsync();
            }

            return video;
        }

        /// <summary>
        /// uses Raw SQL to search for videos by soundex of title and description and by text search of title and description
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<List<Video>> FindVideosByTittle(string searchQuery, int topN=50)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return new List<Video>();

            searchQuery = searchQuery.Trim();

          
            var query = @"
    SELECT *
    FROM Videos
    WHERE SOUNDEX(Title) = SOUNDEX({0})
    OR Title LIKE CONCAT('%', {0}, '%')
    OR SOUNDEX(Description) = SOUNDEX({0})
    OR Description LIKE CONCAT('%', {0}, '%')
    ORDER BY 
        (CASE 
            WHEN Title LIKE CONCAT('%', {0}, '%') THEN 1
            WHEN SOUNDEX(Title) = SOUNDEX({0}) THEN 2
            WHEN SOUNDEX(Description) = SOUNDEX({0}) THEN 3
            WHEN Description LIKE CONCAT('%', {0}, '%') THEN 4
            ELSE 5
         END)
    LIMIT {1}";

        
            var result = await context.Videos.FromSqlRaw(query, searchQuery,topN).ToListAsync();

            return result;
        }
    }
}