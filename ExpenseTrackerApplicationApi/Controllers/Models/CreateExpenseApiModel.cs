namespace ExpenseTrackerApplicationApi.Controllers.Models
{
    public class CreateExpenseApiModel
    {
        public double Amount { get; set; }
        public string Category { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }

        public CreateExpenseApiModel(double amount, string category, DateTime dateTime, string description)
        {
            Amount = amount;
            Category = category;
            DateTime = dateTime;
            Description = description;
        }

    }
}
