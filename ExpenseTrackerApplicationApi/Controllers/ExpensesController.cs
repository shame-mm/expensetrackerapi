using ExpenseTrackerApplicationApi.Controllers.Exceptions;
using ExpenseTrackerApplicationApi.Controllers.Models;
using ExpenseTrackerApplicationApi.Database;
using ExpenseTrackerApplicationApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerApplicationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("DefaultPolicy")]
    public class ExpensesController : ControllerBase
    {
        private IExpensesService ExpensesService { get; set; }

        public ExpensesController(IExpensesService expensesService)
        {
            ExpensesService = expensesService;
        }

        [HttpPost]
        public async Task<int> Create(CreateExpenseApiModel expenseApiModel, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            var newExpense = new ExpenseDbModel(id: 0, userId, expenseApiModel.Amount, expenseApiModel.Category, expenseApiModel.DateTime, expenseApiModel.Description);

            return await ExpensesService.AddExpense(newExpense, cancellationToken);
        }

        [HttpGet]
        public async Task<List<ResponseExpenseApiModel>> GetExpenses(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            var expenses = await ExpensesService.GetExpenses(userId, cancellationToken);

            return expenses.Select(e => new ResponseExpenseApiModel(e)).ToList();
        }

        [HttpGet(template: "{expenseId}")]
        public async Task<ResponseExpenseApiModel> GetExpense(int expenseId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            var expense = await ExpensesService.GetExpense(expenseId, userId, cancellationToken);

            return new ResponseExpenseApiModel(expense);
        }

        [HttpPatch(template: "{expenseId}")]
        public async Task EditExpense(int expenseId, UpdateExpenseApiModel expense, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            await ExpensesService.EditExpense(expenseId, userId, expense.Amount, expense.Category, cancellationToken);
        }

        [HttpDelete(template: "{expenseId}")]
        public async Task Delete(int expenseId, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            await ExpensesService.DeleteExpense(expenseId, userId, cancellationToken);
        }

        private int GetUserId()
        {
            if(!(User.Identity is ClaimsIdentity))
            {
                throw new InvalidUserException();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var userIdString = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

            if(!int.TryParse(userIdString, out var userId))
            {
                throw new InvalidUserException();
            }

            return userId;
        }
    }
}
