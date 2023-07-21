namespace HouseRentingSystem.Web.ViewModels.House
{
    using System.ComponentModel.DataAnnotations;

    using Enums;

    using static Common.GeneralApplicationConstants;

    public class AllHousesQueryModel
    {
        public AllHousesQueryModel()
        {
            CurrentPage = DefaultPage;
            HousesPerPage = EntitiesPerPage;

            Categories = new HashSet<string>();
            Houses = new HashSet<HouseAllViewModel>();
        }

        public string? Category { get; set; }

        [Display(Name = "Search by word")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort Houses By")]
        public HouseSorting HouseSorting { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Show Houses On Page")]
        public int HousesPerPage { get; set; }

        public int TotalHouses { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<HouseAllViewModel> Houses { get; set; }
    }
}
