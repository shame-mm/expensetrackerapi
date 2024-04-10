using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApplicationApi.Database
{
    public class ExpensesDbContext : DbContext
    {
        public DbSet<ExpenseDbModel> Expenses { get; set; }

        public ExpensesDbContext() { }

        public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExpenseDbModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ExpenseDbModel>().HasIndex(x => x.UserId);
        }
    }
}
