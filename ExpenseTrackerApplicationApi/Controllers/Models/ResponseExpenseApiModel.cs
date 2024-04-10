using ExpenseTrackerApplicationApi.Database;

namespace ExpenseTrackerApplicationApi.Controllers.Models
{
    public class ResponseExpenseApiModel
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Category { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }

        public ResponseExpenseApiModel(ExpenseDbModel expenseDefinition)
        {
            Id = expenseDefinition.Id;
            Amount = expenseDefinition.Amount;
            Category = expenseDefinition.Category;
            DateTime = expenseDefinition.DateTime;
            Description = expenseDefinition.Description;
        }
    }
}
