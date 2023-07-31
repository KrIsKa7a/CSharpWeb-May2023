namespace HouseRentingSystem.Services.Data.Interfaces
{
    using Web.ViewModels.Rent;

    public interface IRentService
    {
        Task<IEnumerable<RentViewModel>> AllAsync();
    }
}
