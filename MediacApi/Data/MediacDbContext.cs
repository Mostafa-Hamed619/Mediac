using MediacApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediacApi.Data
{
    public class MediacDbContext : DbContext
    {
        public MediacDbContext(DbContextOptions<MediacDbContext> options):base(options)
        {
            
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Blog>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Post>()
                .HasOne(b => b.Blog)
                .WithMany(p => p.posts)
                .HasForeignKey(p => p.BlogNumber);


        }
    }
}
