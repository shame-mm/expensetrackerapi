using ExpenseTrackerApplicationApi.Database;
using ExpenseTrackerApplicationApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ExpenseTrackerApplicationApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            AddDatabase(builder);

            AddServices(builder);

            builder.Services.AddControllers();

            AddSwagger(builder);

            AddAuthorization(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.MapControllers();

            app.Run();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IExpensesService, ExpensesService>();
        }

        private static void AddAuthorization(WebApplicationBuilder builder)
        {
            builder.Services
                            .AddAuthentication()
                            .AddBearerToken();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                    policy.RequireClaim("CanAccessExpenseApi"));

            });
        }

        private static void AddSwagger(WebApplicationBuilder builder)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. <br />
                      Enter 'Bearer' [space] and then your token in the text input below. <br />
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });
        }

        private static void AddDatabase(WebApplicationBuilder builder)
        {
            // Expenses Database
            builder.Services.AddDbContext<ExpensesDbContext>(db =>
            {
                bool.TryParse(builder.Configuration["UseSqlServer"], out var useSqlServer);

                if (useSqlServer)
                {
                    db.UseSqlServer(builder.Configuration.GetConnectionString("ExpensesConnectionString"));
                }
                else
                {
                    db.UseInMemoryDatabase(databaseName: "expenses");
                }
            }, ServiceLifetime.Singleton);

            // User Database
            builder.Services.AddDbContext<UsersDbContext>(db =>
            {
                bool.TryParse(builder.Configuration["UseSqlServer"], out var useSqlServer);

                if (useSqlServer)
                {
                    db.UseSqlServer(builder.Configuration.GetConnectionString("AuthServiceConnectionString"));
                }
                else
                {
                    db.UseInMemoryDatabase(databaseName: "authentication");
                }

            }, ServiceLifetime.Singleton);
        }
    }
}
