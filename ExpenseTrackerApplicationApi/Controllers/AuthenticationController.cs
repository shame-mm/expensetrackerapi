using ExpenseTrackerApplicationApi.Controllers.Models;
using ExpenseTrackerApplicationApi.Database;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerApplicationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private UsersDbContext UsersDbContext;

        public AuthenticationController(UsersDbContext usersDbContext)
        {
            UsersDbContext = usersDbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<IResult> Authenticate(AuthenticateModel authenticateModel, CancellationToken cancellationToken)
        {
            UsersDbContext.Database.EnsureCreated();

            var user = UsersDbContext.Users.Where(user => user.Username == authenticateModel.Username).FirstOrDefault();

            if (user == null)
            {
                return Task.FromResult(Results.Unauthorized());
            }

            var claims = new List<Claim>();

            // Always present: username, userId
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim("UserId", user.Id.ToString()));

            // Claims based on user configuration
            claims.Add(new Claim("CanAccessExpenseApi", ""));

            var claimsPrincipal = new ClaimsPrincipal(
                   new ClaimsIdentity(
                     claims,
                     BearerTokenDefaults.AuthenticationScheme
                   )
                 );

            return Task.FromResult(Results.SignIn(claimsPrincipal));
        }
    }
}
