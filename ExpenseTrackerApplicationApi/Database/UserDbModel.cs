using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerApplicationApi.Database
{
    public class UserDbModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public UserDbModel(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
