using System;
using System.Threading.Tasks;
using OpenVisStreamer.VideoLibrary.Model.Entities;
using OpenVisStreamer.VideoLibrary.Repository.EFC;

namespace OpenVisStreamer.VideoLibrary.Services
{
    public class VideoService(DatabaseContext context)
    {
        public async Task Create(Video video)
        {
            context.Videos.Add(video);
            await context.SaveChangesAsync();
        }

        public async Task<Video> Read(Guid id)
        {
            return await context.Videos.FindAsync(id);
        }

        public async Task Update(Video video)
        {
            context.Videos.Update(video);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var video = await context.Videos.FindAsync(id);
            if (video != null)
            {
                context.Videos.Remove(video);
                await context.SaveChangesAsync();
            }
        }
        
        public async Task AddLike(Guid id)
        {
            var video = await context.Videos.FindAsync(id);
            if (video != null)
            {
                video.Likes++;
                await context.SaveChangesAsync();
            }
        }
        
        public async Task AddDislike(Guid id)
        {
            var video = await context.Videos.FindAsync(id);
            if (video != null)
            {
                video.Dislikes++;
                await context.SaveChangesAsync();
            }
        }
        
        
        
        
        
    }
}