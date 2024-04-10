using ExpenseTrackerApplicationApi.Database;
using ExpenseTrackerApplicationApi.Database.Exceptions;
using ExpenseTrackerApplicationApi.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApplicationTests
{
    internal class ExpensesServiceUnitTests
    {
        private double DefaultAmount { get; set; }
        private string DefaultCategory { get; set; }
        private DateTime DefaultDateTime { get; set; }
        private string DefaultDescription { get; set; }

        public ExpensesServiceUnitTests()
        {
            DefaultAmount = 123;
            DefaultCategory = "Category";
            DefaultDateTime = new DateTime(2024, 01, 01);
            DefaultDescription = "Description";
        }

        [Test]
        public async Task GetExpenses_HappyFlow()
        {
            var numberOfExpensesPerUser = 2;
            var userId = 1;

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 1, numberOfExpensesPerUser: numberOfExpensesPerUser);

            var expensesService = new ExpensesService(context);

            // ACT
            var expenses = await expensesService.GetExpenses(userId: userId, new CancellationToken());

            // VERIFY

            // Number of expenses returned for specific user
            Assert.That(actual: expenses.Count, Is.EqualTo(expected: numberOfExpensesPerUser));

            var firstExpense = expenses.First();

            Assert.That(actual: firstExpense.Amount, Is.EqualTo(expected: DefaultAmount));
            Assert.That(actual: firstExpense.Category, Is.EqualTo(expected: DefaultCategory));
            Assert.That(actual: firstExpense.DateTime, Is.EqualTo(expected: DefaultDateTime));
            Assert.That(actual: firstExpense.Description, Is.EqualTo(expected: DefaultDescription));
        }

        [Test]
        public async Task GetExpense_HappyFlow()
        {
            var numberOfExpensesPerUser = 2;
            var userId = 1;

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 1, numberOfExpensesPerUser: numberOfExpensesPerUser);

            var expensesService = new ExpensesService(context);

            var expenses = await expensesService.GetExpenses(userId: userId, new CancellationToken());
            var firstExpense = expenses.First().Id;

            // ACT
            var expense = await expensesService.GetExpense(expenseId: firstExpense, userId: userId, new CancellationToken());

            // VERIFY

            // Number of expenses returned for specific user
            Assert.That(actual: expense.Amount, Is.EqualTo(expected: DefaultAmount));
            Assert.That(actual: expense.Category, Is.EqualTo(expected: DefaultCategory));
            Assert.That(actual: expense.DateTime, Is.EqualTo(expected: DefaultDateTime));
            Assert.That(actual: expense.Description, Is.EqualTo(expected: DefaultDescription));
        }

        [Test]
        public async Task GetExpense_WithInvalidId_ThrowsException()
        {
            var numberOfExpensesPerUser = 2;
            var userId = 1;
            var nonExistingExpenseId = int.MaxValue;

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 1, numberOfExpensesPerUser: numberOfExpensesPerUser);

            var expensesService = new ExpensesService(context);

            var expenses = await expensesService.GetExpenses(userId: userId, new CancellationToken());
            var firstExpense = expenses.First().Id;

            // ACT / VERIFY
            Assert.ThrowsAsync<EntryNotFoundException>(() => expensesService.GetExpense(expenseId: nonExistingExpenseId, userId: userId, new CancellationToken()));
        }

        [Test]
        public async Task GetExpenses_ReturnsOnlyExpensesForRequestingUser()
        {
            var numberOfExpensesPerUser = 2;
            var userId = 1;

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 10, numberOfExpensesPerUser: numberOfExpensesPerUser);

            var expensesService = new ExpensesService(context);

            // ACT
            var expenses = await expensesService.GetExpenses(userId: userId, new CancellationToken());

            // VERIFY

            // Number of expenses returned for specific user
            Assert.That(actual: expenses.Count, Is.EqualTo(expected: numberOfExpensesPerUser));

            // User Id on the returned expense is the same as the requested user id
            foreach(var expense in expenses)
            {
                Assert.That(actual: expense.UserId, Is.EqualTo(userId));
            }
        }

        [Test]
        public async Task AddExpense_HappyFlow()
        {
            var userId = 2;

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 0, numberOfExpensesPerUser: 0);

            var expensesService = new ExpensesService(context);

            // Verify Total entries in database before adding
            Assert.That(actual: context.Expenses.Count(), Is.EqualTo(expected: 0));

            // ACT
            await expensesService.AddExpense(new ExpenseDbModel(id: 0, userId: userId, amount: 444, category: "Category", dateTime: new DateTime(2024, 01, 01), description: "Description"), new CancellationToken());

            // VERIFY

            // Total entries in database
            Assert.That(actual: context.Expenses.Count(), Is.EqualTo(expected: 1));
        }

        [Test]
        public async Task DeleteExpense_HappyFlow()
        {
            var userId = 1;

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 1, numberOfExpensesPerUser: 1);

            var expensesService = new ExpensesService(context);

            // Verify Total entries in database before adding
            Assert.That(actual: context.Expenses.Count(), Is.EqualTo(expected: 1));

            // ACT
            var existingExpense = await expensesService.GetExpenses(userId, new CancellationToken());
            await expensesService.DeleteExpense(existingExpense.Single().Id, userId, new CancellationToken());

            // VERIFY

            // Total entries in database
            Assert.That(actual: context.Expenses.Count(), Is.EqualTo(expected: 0));
        }

        [Test]
        public async Task EditExpense_Amount_HappyFlow()
        {
            var userId = 1;
            var newAmount = 456;

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 1, numberOfExpensesPerUser: 1);

            var expensesService = new ExpensesService(context);

            // ACT
            var existingExpense = await expensesService.GetExpenses(userId, new CancellationToken());
            await expensesService.EditExpense(existingExpense.Single().Id, userId, amount: newAmount, category: null, new CancellationToken());

            // VERIFY
            var updatedExpenses = await expensesService.GetExpenses(userId, new CancellationToken());

            // New Amount exists on the expense
            Assert.That(actual: updatedExpenses.Single().Amount, Is.EqualTo(expected: newAmount));

            // Category is unchanged
            Assert.That(actual: updatedExpenses.Single().Category, Is.EqualTo(expected: DefaultCategory));
        }

        [Test]
        public async Task EditExpense_Category_HappyFLow()
        {
            var userId = 1;
            var newCategory = "New Category";

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 1, numberOfExpensesPerUser: 1);

            var expensesService = new ExpensesService(context);

            // ACT
            var existingExpense = await expensesService.GetExpenses(userId, new CancellationToken());
            await expensesService.EditExpense(existingExpense.Single().Id, userId, amount: null, category: newCategory, new CancellationToken());

            // VERIFY
            var updatedExpenses = await expensesService.GetExpenses(userId, new CancellationToken());

            // Amount is unchanged
            Assert.That(actual: updatedExpenses.Single().Amount, Is.EqualTo(expected: DefaultAmount));

            // New Category exists on the expense
            Assert.That(actual: updatedExpenses.Single().Category, Is.EqualTo(expected: newCategory));
        }

        [Test]
        public async Task EditExpense_OtherUsersExpense_ThrowsException()
        {
            var userId = 1;
            var otherUserId = 2;
            var newCategory = "New Category";

            // SETUP
            var context = await SetupInMemoryExpensesDatabase(numberOfUsers: 2, numberOfExpensesPerUser: 1);

            var expensesService = new ExpensesService(context);

            var existingOtherUsersExpense = await expensesService.GetExpenses(otherUserId, new CancellationToken());

            // ACT / VERIFY
            Assert.ThrowsAsync<EntryNotFoundException>(() => expensesService.EditExpense(existingOtherUsersExpense.Single().Id, userId, amount: null, category: newCategory, new CancellationToken()));
        }

        private async Task<ExpensesDbContext> SetupInMemoryExpensesDatabase(int numberOfUsers, int numberOfExpensesPerUser)
        {
            var dbOptions = new DbContextOptionsBuilder<ExpensesDbContext>()
                           .UseInMemoryDatabase(databaseName: "testexpensesdatabase")
                           .Options;

            var dbContext = new ExpensesDbContext(dbOptions);

            // Clear previous data
            dbContext.Expenses.RemoveRange(dbContext.Expenses);

            for(int userId = 1; userId <= numberOfUsers; userId++)
            {
                for(int e = 0; e < numberOfExpensesPerUser; e++)
                {
                    dbContext.Expenses.Add(new ExpenseDbModel(id: 0, userId: userId, amount: DefaultAmount, category: DefaultCategory, dateTime: DefaultDateTime, description: DefaultDescription));
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }
    }
}
