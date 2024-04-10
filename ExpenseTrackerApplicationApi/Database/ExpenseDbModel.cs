using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerApplicationApi.Database
{
    public class ExpenseDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }

        public ExpenseDbModel(int id, int userId, double amount, string category, DateTime dateTime, string description)
        {
            Id = id;
            UserId = userId;
            Amount = amount;
            Category = category;
            DateTime = dateTime;
            Description = description;
        }
    }
}
