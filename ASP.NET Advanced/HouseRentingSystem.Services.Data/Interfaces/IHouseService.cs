namespace HouseRentingSystem.Services.Data.Interfaces
{
    using Models.House;

    using Web.ViewModels.Home;
    using Web.ViewModels.House;

    public interface IHouseService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();

        Task CreateAsync(HouseFormModel formModel, string agentId);

        Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel queryModel);
    }
}
