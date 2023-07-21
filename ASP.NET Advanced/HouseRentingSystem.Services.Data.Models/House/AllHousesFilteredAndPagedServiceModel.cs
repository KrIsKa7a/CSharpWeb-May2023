namespace HouseRentingSystem.Services.Data.Models.House
{
    using Web.ViewModels.House;

    public class AllHousesFilteredAndPagedServiceModel
    {
        public AllHousesFilteredAndPagedServiceModel()
        {
            Houses = new HashSet<HouseAllViewModel>();
        }

        public int TotalHousesCount { get; set; }

        public IEnumerable<HouseAllViewModel> Houses { get; set; }
    }
}
