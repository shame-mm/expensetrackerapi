using ExpenseTrackerApplicationApi.Database;

namespace ExpenseTrackerApplicationApi.Services
{
    public interface IExpensesService
    {
        /// <summary>
        /// Add expense to the database
        /// </summary>
        /// <param name="expenseModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> AddExpense(ExpenseDbModel expenseModel, CancellationToken cancellationToken);

        /// <summary>
        /// Get a specific expense. If the expense does not exist or if the userId does not own the expense an EntryNotFoundException is thrown.
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The Expense database model.</returns>
        Task<ExpenseDbModel> GetExpense(int expenseId, int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Get expenses for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of expenses for the user</returns>
        Task<List<ExpenseDbModel>> GetExpenses(int userId, CancellationToken cancellationToken);

        /// <summary>
        /// Edit an expense. If the expense does not exist or if the userId does not own the expense an EntryNotFoundException is thrown.
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <param name="category"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task EditExpense(int expenseId, int userId, double? amount, string? category, CancellationToken cancellationToken);

        /// <summary>
        /// Delete expense. If the expense does not exist or if the userId does not own the expense an EntryNotFoundException is thrown
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteExpense(int expenseId, int userId, CancellationToken cancellationToken);
    }
}
