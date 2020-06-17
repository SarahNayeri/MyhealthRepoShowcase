using Microsoft.EntityFrameworkCore;

namespace ImageGallery.API.Entities
{
    public class GalleryContext : DbContext
    {
        public GalleryContext(DbContextOptions<GalleryContext> options)
           : base(new DbContextOptionsBuilder<GalleryContext>()
                    .UseInMemoryDatabase("TestDatabase").Options)
        {
        }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
