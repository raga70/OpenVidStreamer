using Microsoft.EntityFrameworkCore;
using RecommendationAlgo.Repository.Entities;

namespace OpenVisStreamer.VideoLibrary.Repository.EFC;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<WatchHistory> WatchHistories { get; set; }
    public DbSet<VideoLike> VideoLikes { get; set; }
    public DbSet<VideoDislike> VideoDislikes { get; set; }
}
