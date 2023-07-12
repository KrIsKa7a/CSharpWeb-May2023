namespace HouseRentingSystem.Web.ViewModels.Category
{
    using Interfaces;

    public class CategoryDetailsViewModel : ICategoryDetailsModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
