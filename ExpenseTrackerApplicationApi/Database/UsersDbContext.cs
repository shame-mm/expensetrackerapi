using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApplicationApi.Database
{
    public class UsersDbContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDbModel>().HasKey(x => x.Id);
            modelBuilder.Entity<UserDbModel>().HasIndex(x => x.Username).IsUnique(unique: true);

            modelBuilder.Entity<UserDbModel>().HasData(
                new UserDbModel(1, "user1", "password1"),
                new UserDbModel(2, "user2", "password2"),
                new UserDbModel(3, "user3", "password3")
            );
        }
    }
}
