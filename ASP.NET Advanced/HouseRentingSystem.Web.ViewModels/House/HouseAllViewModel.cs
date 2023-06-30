namespace HouseRentingSystem.Web.ViewModels.House
{
    using System.ComponentModel.DataAnnotations;

    public class HouseAllViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Address { get; set; } = null!;

        [Display(Name = "Image Link")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "Monthly Price")]
        public decimal PricePerMonth { get; set; }

        [Display(Name = "Is Rented")]
        public bool IsRented { get; set; }
    }
}
