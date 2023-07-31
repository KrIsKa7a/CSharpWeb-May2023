namespace HouseRentingSystem.Web.Areas.Admin.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using ViewModels.House;

    public class HouseController : BaseAdminController
    {
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;

        public HouseController(IAgentService agentService, IHouseService houseService)
        {
            this.agentService = agentService;
            this.houseService = houseService;
        }

        public async Task<IActionResult> Mine()
        {
            string? agentId =
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);
            MyHousesViewModel viewModel = new MyHousesViewModel()
            {
                AddedHouses = await this.houseService.AllByAgentIdAsync(agentId!),
                RentedHouses = await this.houseService.AllByUserIdAsync(this.User.GetId()!),
            };

            return this.View(viewModel);
        }
    }
}
