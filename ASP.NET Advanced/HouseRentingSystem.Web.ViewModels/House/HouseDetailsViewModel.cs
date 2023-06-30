namespace HouseRentingSystem.Web.ViewModels.House
{
    using Agent;

    public class HouseDetailsViewModel : HouseAllViewModel
    {
        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public AgentInfoOnHouseViewModel Agent { get; set; } = null!;
    }
}
