using Microsoft.EntityFrameworkCore;
using OpenVisStreamer.VideoLibrary.Model.Entities;

namespace OpenVisStreamer.VideoLibrary.Repository.EFC;

public class DatabaseContext (DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Video> Videos { get; set; }
}