# Expense Tracker Api

## Requirements
* .Net 8 SDK
* Sql Server (optional, In Memory db exists as alternative)

## Configuration: appsettings.json
* "UseSqlServer": 
	- false: Use In-Memory database. 
	- true: Use Sql Server. This requires the connection strings "ExpensesConnectionString" and "AuthServiceConnectionString" to be properly configured
* "ExpensesConnectionString": The connection string to the Expenses database
* "AuthServiceConnectionString": The connection string to the Authentication Service

## Run
Run the application using the following command, or open the solution file in your favorite IDE

	dotnet run --project ExpenseTrackerApplicationApi/ExpenseTrackerApplicationApi.csproj

Navigate to http://localhost:5087/swagger (The port might differ) to see the swagger documentation

## Authentication Api
Use the POST /Authentication endpoint to create a bearer token. 3 example users are added to the database, user1, user2, user3. The password field is required but is currently not validated to match. See section Future improvements below.
The generated token grants access for the user to access the Expenses api.

## Expenses Api
A user can create, edit, delete, and get expenses. Users can not access other users expenses.

All expenses endpoints requires a bearer token. See Authentication section above how to generate it


## Unit Tests
A separate project ExpenseTrackerApplicationTests contains the unit tests, currently tests only exists for ExpensesService





# Future improvements

The following is a list of things that should be improved upon for continuing the development of the application. These were left out because of lack of time.

## Separate Authentication Api to a separate Api 
	Authentication and Expense Tracker should not be in the same application. They have different purpose and should be separate because of isolation of concerns principles.
	
## Password management for the Authentication Service
	Passwords are currently not validated
	Passwords should not be stored in clear text
	Make necessary configurations to allow for tokens from separate authentication api to be used in Expense Tracker Api, such as "Audience"

## Authorization Policies for different actions, using different claims
	This improves configuration of endpoint security by having more fine-grained access
	
## Add logging
	Use ILogger interface
	Add logging to ExceptionHandlerMiddleware
	Exception handling and logging should be kept at as few places as possible. Try to keep it limited to the try catch in ExceptionHandlerMiddleware.

## Add more unit tests

## Expense Category could be something that is stored in a separate table in the database
	Columns "CategoryId", "CategoryName", expense can be connected to the category Id instead of the string name

