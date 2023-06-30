namespace HouseRentingSystem.Services.Data.Interfaces
{
    using Web.ViewModels.Agent;

    public interface IAgentService
    {
        Task<bool> AgentExistsByUserIdAsync(string userId);

        Task<bool> AgentExistsByPhoneNumberAsync(string phoneNumber);

        Task<bool> HasRentsByUserIdAsync(string userId);

        Task Create(string userId, BecomeAgentFormModel model);

        Task<string?> GetAgentIdByUserIdAsync(string userId);
    }
}
