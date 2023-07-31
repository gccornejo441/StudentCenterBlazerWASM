using StudentCenter.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCenter.Client
{
    public class UserAccountDataService
    {
        private List<UserAccount> users;

        public UserAccountDataService()
        {
            // Mock data for demonstration purposes
            users = new List<UserAccount>
        {
            new UserAccount
            {
                UserId = 1,
                Username = "john.doe",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe",
                Phone = "123-456-7890",
                Email = "john.doe@example.com",
                Role = "User"
            },
            new UserAccount
            {
                UserId = 2,
                Username = "jane.smith",
                Password = "qwerty789",
                FirstName = "Jane",
                LastName = "Smith",
                Phone = "987-654-3210",
                Email = "jane.smith@example.com",
                Role = "Admin"
            },
            // Add more users as needed
        };
        }

        public Task<List<UserAccount>> GetUsersAsync()
        {
            return Task.FromResult(users);
        }

        public Task UpdateUserAsync(UserAccount updatedUser)
        {
            var existingUser = users.FirstOrDefault(u => u.UserId == updatedUser.UserId);
            if (existingUser != null)
            {
                existingUser.Username = updatedUser.Username;
                existingUser.Password = updatedUser.Password;
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Phone = updatedUser.Phone;
                existingUser.Email = updatedUser.Email;
                existingUser.Role = updatedUser.Role;
            }

            return Task.CompletedTask;
        }
    }

}
