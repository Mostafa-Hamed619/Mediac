using MediacApi.Data.Entities;
using MediacApi.Services.IRepositories;
using MediacBack.HelperClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;
using System.Text;

namespace MediacApi.Data
{
    public class MediacDbContext : IdentityDbContext<User>
    {
        private readonly ihttpAccessor accessor;

        public MediacDbContext(DbContextOptions<MediacDbContext> options,ihttpAccessor accessor ):base(options)
        {
            this.accessor = accessor;
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Subscribe> subscribes { get; set; }
        public DbSet<Followers> followers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                        .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Blog>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Subscribe>()
                .HasKey(e => new { e.FollowerId, e.BlogId });

            modelBuilder.Entity<Followers>()
                .HasKey(e => new { e.FollowerUserId, e.FolloweeUserId });
           
            modelBuilder.Entity<Post>()
                .HasOne(b => b.Blog)
                .WithMany(p => p.posts)
                .HasForeignKey(p => p.BlogNumber);

            modelBuilder.Entity<Post>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(i => i.AuthorId);

            modelBuilder.Entity<Subscribe>()
                .HasOne(s => s.blog)
                .WithMany(p => p.subscribes)
                .HasForeignKey(s => s.BlogId);

            modelBuilder.Entity<Subscribe>()
                .HasOne(s => s.user)
                .WithMany(u => u.subscribes)
                .HasForeignKey(s => s.FollowerId);

            modelBuilder.Entity<Followers>()
                .HasOne(f => f.FollowerUser)
                .WithMany(u => u.followees)
                .HasForeignKey(f => f.FollowerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Followers>()
                .HasOne(f => f.FolloweeUser)
                .WithMany(u => u.followers)
                .HasForeignKey(k => k.FolloweeUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUser =accessor.GetContext().HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var modificationEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Deleted
            || x.State == EntityState.Modified).ToList();

            foreach (var entity in modificationEntities)
            {
                var auditLog = new AuditLog()
                {
                    Action = entity.State.ToString(),
                    EntityType = entity.Entity.GetType().ToString(),
                    TimeStamp = DateTime.UtcNow,
                    Changes =GetUpdates(entity),
                    User = currentUser
                };
                AuditLogs.Add(auditLog);

            }
            return  base.SaveChangesAsync(cancellationToken);
        }

        private static string GetUpdates(EntityEntry entry)
        {
            var sb = new StringBuilder();

            foreach (var prop in entry.OriginalValues.Properties)
            {
                var originalValues = entry.OriginalValues[prop];
                var currentValues = entry.CurrentValues[prop];

                if (originalValues != currentValues)
                {
                    sb.AppendLine($"{prop.Name} from {originalValues} to {currentValues}");
                }
            }

            return sb.ToString();
        }
    }

     
    
}
