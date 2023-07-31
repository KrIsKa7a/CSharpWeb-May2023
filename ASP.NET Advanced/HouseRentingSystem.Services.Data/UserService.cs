namespace HouseRentingSystem.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using HouseRentingSystem.Data;
    using HouseRentingSystem.Data.Models;
    using Interfaces;
    using Web.ViewModels.User;

    public class UserService : IUserService
    {
        private readonly HouseRentingDbContext dbContext;

        public UserService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> GetFullNameByEmailAsync(string email)
        {
            ApplicationUser? user = await this.dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return string.Empty;
            }

            return $"{user.FirstName} {user.LastName}";
        }

        public async Task<string> GetFullNameByIdAsync(string userId)
        {
            ApplicationUser? user = await this.dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return string.Empty;
            }

            return $"{user.FirstName} {user.LastName}";
        }

        public async Task<IEnumerable<UserViewModel>> AllAsync()
        {
            List<UserViewModel> allUsers = await this.dbContext
                .Users
                .Select(u => new UserViewModel()
                {
                    Id = u.Id.ToString(),
                    Email = u.Email,
                    FullName = u.FirstName + " " + u.LastName
                })
                .ToListAsync();
            foreach (UserViewModel user in allUsers)
            {
                Agent? agent = this.dbContext
                    .Agents
                    .FirstOrDefault(a => a.UserId.ToString() == user.Id);
                if (agent != null)
                {
                    user.PhoneNumber = agent.PhoneNumber;
                }
                else
                {
                    user.PhoneNumber = string.Empty;
                }
            }

            return allUsers;
        }
    }
}
