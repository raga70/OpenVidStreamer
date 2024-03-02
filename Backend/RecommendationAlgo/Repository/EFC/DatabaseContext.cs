using Microsoft.EntityFrameworkCore;
using RecommendationAlgo.Repository.Entities;

namespace OpenVisStreamer.VideoLibrary.Repository.EFC;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<WatchHistory> WatchHistories { get; set; }
    public DbSet<VideoStats> VideoStats { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<WatchHistory>().HasKey(e => new { e.UserId, e.VideoId }); //ensure  defining composite key (annotations are not reliable for composite keys)
       
        modelBuilder.Entity<WatchHistory>()//One To Many relationship between WatchHistory and VideoStats , assuming that VideoStats gets created first (///VideoMetadataConsumer should fill it after video upload)
            .HasOne<VideoStats>() 
            .WithMany() // VideoStats does not explicitly reference back to WatchHistory
            .HasForeignKey(wh => wh.VideoId) 
            .IsRequired(); // Indicates that the foreign key cannot be null

    }

}
