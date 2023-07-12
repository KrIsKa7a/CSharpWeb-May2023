namespace HouseRentingSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Data.Interfaces;
    using Services.Data.Models.House;
    using ViewModels.House;

    using static Common.NotificationMessagesConstants;

    [Authorize]
    public class HouseController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;

        public HouseController(ICategoryService categoryService, IAgentService agentService,
            IHouseService houseService)
        {
            this.categoryService = categoryService;
            this.agentService = agentService;
            this.houseService = houseService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllHousesQueryModel queryModel)
        {
            AllHousesFilteredAndPagedServiceModel serviceModel =
                await this.houseService.AllAsync(queryModel);

            queryModel.Houses = serviceModel.Houses;
            queryModel.TotalHouses = serviceModel.TotalHousesCount;
            queryModel.Categories = await this.categoryService.AllCategoryNamesAsync();

            return this.View(queryModel);
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

            try
            {
                HouseFormModel formModel = new HouseFormModel()
                {
                    Categories = await this.categoryService.AllCategoriesAsync()
                };

                return View(formModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            bool isAgent =
                await this.agentService.AgentExistsByUserIdAsync(this.User.GetId()!);
            if (!isAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to add new houses!";

                return this.RedirectToAction("Become", "Agent");
            }

            bool categoryExists =
                await this.categoryService.ExistsByIdAsync(model.CategoryId);
            if (!categoryExists)
            {
                // Adding model error to ModelState automatically makes ModelState Invalid
                this.ModelState.AddModelError(nameof(model.CategoryId), "Selected category does not exist!");
            }

            if (!this.ModelState.IsValid)
            {
                model.Categories = await this.categoryService.AllCategoriesAsync();

                return this.View(model);
            }

            try
            {
                string? agentId = 
                    await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);

                string houseId = 
                    await this.houseService.CreateAndReturnIdAsync(model, agentId!);

                this.TempData[SuccessMessage] = "House was added successfully!";
                return this.RedirectToAction("Details", "House", new { id = houseId });
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred while trying to add your new house! Please try again later or contact administrator!");
                model.Categories = await this.categoryService.AllCategoriesAsync();

                return this.View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            bool houseExists = await this.houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";

                return this.RedirectToAction("All", "House");
            }

            try
            {
                HouseDetailsViewModel viewModel = await this.houseService
                    .GetDetailsByIdAsync(id);

                return View(viewModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool houseExists = await this.houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";

                return this.RedirectToAction("All", "House");
                
                //return this.NotFound(); -> If you want to return 404 page
            }

            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);
            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }

            string? agentId = 
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);
            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return this.RedirectToAction("Mine", "House");
            }

            try
            {
                HouseFormModel formModel = await this.houseService
                    .GetHouseForEditByIdAsync(id);
                formModel.Categories = await this.categoryService.AllCategoriesAsync();

                return this.View(formModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, HouseFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = await this.categoryService.AllCategoriesAsync();

                return this.View(model);
            }

            bool houseExists = await this.houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";

                return this.RedirectToAction("All", "House");
            }

            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);
            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }

            string? agentId =
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);
            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return this.RedirectToAction("Mine", "House");
            }

            try
            {
                await this.houseService.EditHouseByIdAndFormModelAsync(id, model);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty,
                    "Unexpected error occurred while trying to update the house. Please try again later or contact administrator!");
                model.Categories = await this.categoryService.AllCategoriesAsync();

                return this.View(model);
            }

            this.TempData[SuccessMessage] = "House was edited successfully!";
            return this.RedirectToAction("Details", "House", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool houseExists = await this.houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";

                return this.RedirectToAction("All", "House");
            }

            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);
            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }

            string? agentId =
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);
            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return this.RedirectToAction("Mine", "House");
            }

            try
            {
                HousePreDeleteDetailsViewModel viewModel =
                    await this.houseService.GetHouseForDeleteByIdAsync(id);

                return this.View(viewModel);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, HousePreDeleteDetailsViewModel model)
        {
            bool houseExists = await this.houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";

                return this.RedirectToAction("All", "House");
            }

            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(this.User.GetId()!);
            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return this.RedirectToAction("Become", "Agent");
            }

            string? agentId =
                await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId()!);
            bool isAgentOwner = await this.houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return this.RedirectToAction("Mine", "House");
            }

            try
            {
                await this.houseService.DeleteHouseByIdAsync(id);

                this.TempData[WarningMessage] = "The house was successfully deleted!";
                return this.RedirectToAction("Mine", "House");
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<HouseAllViewModel> myHouses =
                new List<HouseAllViewModel>();

            string userId = this.User.GetId()!;
            bool isUserAgent = await this.agentService
                .AgentExistsByUserIdAsync(userId);

            try
            {
                if (isUserAgent)
                {
                    string? agentId =
                        await this.agentService.GetAgentIdByUserIdAsync(userId);

                    myHouses.AddRange(await this.houseService.AllByAgentIdAsync(agentId!));
                }
                else
                {
                    myHouses.AddRange(await this.houseService.AllByUserIdAsync(userId));
                }

                return this.View(myHouses);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Rent(string id)
        {
            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with provided id does not exist! Please try again!";

                return this.RedirectToAction("All", "House");
            }

            bool isHouseRented = await this.houseService.IsRentedAsync(id);
            if (isHouseRented)
            {
                this.TempData[ErrorMessage] =
                    "Selected house is already rented by another user! Please select another house.";

                return this.RedirectToAction("All", "House");
            }

            bool isUserAgent =
                await this.agentService.AgentExistsByUserIdAsync(this.User.GetId()!);
            if (isUserAgent)
            {
                this.TempData[ErrorMessage] = "Agents can't rent houses. Please register as a user!";

                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                await this.houseService.RentHouseAsync(id, this.User.GetId()!);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }

            return this.RedirectToAction("Mine", "House");
        }

        [HttpPost]
        public async Task<IActionResult> Leave(string id)
        {
            bool houseExists = await this.houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                this.TempData[ErrorMessage] = "House with provided id does not exist! Please try again!";

                return this.RedirectToAction("All", "House");
            }

            bool isHouseRented = await this.houseService.IsRentedAsync(id);
            if (!isHouseRented)
            {
                this.TempData[ErrorMessage] =
                    "Selected house is not rented! Please select one of your houses if you wish to leave them.";

                return this.RedirectToAction("Mine", "House");
            }

            bool isCurrentUserRenterOfTheHouse =
                await this.houseService.IsRentedByUserWithIdAsync(id, this.User.GetId()!);
            if (!isCurrentUserRenterOfTheHouse)
            {
                this.TempData[ErrorMessage] =
                    "You must be the renter of the house in order to leave it! Please try again with one of your rented houses if you wish to leave them.";

                return this.RedirectToAction("Mine", "House");
            }

            try
            {
                await this.houseService.LeaveHouseAsync(id);
            }
            catch (Exception)
            {
                return this.GeneralError();
            }

            return this.RedirectToAction("Mine", "House");
        }

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
