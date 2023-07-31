namespace HouseRentingSystem.Web.Areas.Admin.ViewModels.House
{
    using Web.ViewModels.House;

    public class MyHousesViewModel
    {
        public IEnumerable<HouseAllViewModel> AddedHouses { get; set; } = null!;

        public IEnumerable<HouseAllViewModel> RentedHouses { get; set; } = null!;
    }
}
