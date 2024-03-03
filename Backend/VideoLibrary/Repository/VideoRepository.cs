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
        
     
        
        public async Task UpdateVideoToPublic(Guid videoId)
        {
            var video = await context.Videos.FindAsync(videoId);
            if (video != null)
            {
                video.IsPublic = true;
                context.Videos.Update(video);
                await context.SaveChangesAsync();
            }
        }
        
    }
}