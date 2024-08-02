using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieShop.Security;

#nullable disable

namespace MovieShop.Database
{
    public partial class MWSContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public MWSContext() { }
        public MWSContext(DbContextOptions<MWSContext> options) : base(options) { }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<MovieActor> MovieActors { get; set; }
        public virtual DbSet<MovieCategory> MovieCategories { get; set; }
        public virtual DbSet<FavouriteMovie> FavouriteMovies { get; set; }
        public virtual DbSet<Director> Directors { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
