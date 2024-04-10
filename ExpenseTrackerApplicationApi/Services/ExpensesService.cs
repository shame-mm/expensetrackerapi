using ExpenseTrackerApplicationApi.Database;
using ExpenseTrackerApplicationApi.Database.Exceptions;

namespace ExpenseTrackerApplicationApi.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly ExpensesDbContext ExpensesDbContext;

        public ExpensesService(ExpensesDbContext expensesDbContext) 
        {
            ExpensesDbContext = expensesDbContext;
        }

        public async Task<int> AddExpense(ExpenseDbModel expenseDbModel, CancellationToken cancellationToken)
        {
            ExpensesDbContext.Expenses.Add(expenseDbModel);

            await ExpensesDbContext.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

            return expenseDbModel.Id;
        }

        public async Task DeleteExpense(int expenseId, int userId, CancellationToken cancellationToken)
        {
            var expense = ExpensesDbContext.Expenses.Where(expense => expense.Id == expenseId && expense.UserId == userId).FirstOrDefault();

            if (expense == null)
            {
                throw new EntryNotFoundException();
            }

            ExpensesDbContext.Expenses.Remove(expense);

            await ExpensesDbContext.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
        }

        public async Task EditExpense(int expenseId, int userId, double? amount, string? category, CancellationToken cancellationToken)
        {
            var expense = ExpensesDbContext.Expenses.FirstOrDefault(expense => expense.Id == expenseId && expense.UserId == userId);

            if (expense == null)
            {
                throw new EntryNotFoundException();
            }

            expense.Amount = amount ?? expense.Amount;
            expense.Category = category ?? expense.Category;

            await ExpensesDbContext.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
        }

        public Task<ExpenseDbModel> GetExpense(int expenseId, int userId, CancellationToken cancellationToken)
        {
            var expense = ExpensesDbContext.Expenses.FirstOrDefault(expense => expense.Id == expenseId && expense.UserId == userId);

            if(expense == null)
            {
                throw new EntryNotFoundException();
            }

            return Task.FromResult(expense);
        }

        public Task<List<ExpenseDbModel>> GetExpenses(int userId, CancellationToken cancellationToken)
        {
            var expenses = ExpensesDbContext.Expenses.Where(x => x.UserId == userId).ToList();

            return Task.FromResult(expenses);
        }
    }
}
