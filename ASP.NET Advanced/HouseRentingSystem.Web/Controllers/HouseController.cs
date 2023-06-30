namespace HouseRentingSystem.Web.Controllers
{
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Interfaces;
    using ViewModels.House;

    using static Common.NotificationMessagesConstants;

    [Authorize]
    public class HouseController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IAgentService agentService;

        public HouseController(ICategoryService categoryService, IAgentService agentService)
        {
            this.categoryService = categoryService;
            this.agentService = agentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isAgent =
                await this.agentService.AgentExistsByUserIdAsync(this.User.GetId()!);
            if (!isAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to add new houses!";

                return this.RedirectToAction("Become", "Agent");
            }

            HouseFormModel formModel = new HouseFormModel()
            {
                Categories = await this.categoryService.AllCategoriesAsync()
            };

            return View(formModel);
        }
    }
}
