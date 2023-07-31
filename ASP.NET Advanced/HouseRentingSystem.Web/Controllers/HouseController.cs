namespace HouseRentingSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Data.Interfaces;
    using Services.Data.Models.House;
    using ViewModels.House;

    using static Common.GeneralApplicationConstants;
    using static Common.NotificationMessagesConstants;

    [Authorize]
    public class HouseController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;
        private readonly IUserService userService;

        private readonly IMemoryCache memoryCache;

        public HouseController(ICategoryService categoryService, IAgentService agentService,
            IHouseService houseService, IUserService userService, IMemoryCache memoryCache)
        {
            this.categoryService = categoryService;
            this.agentService = agentService;
            this.houseService = houseService;
            this.userService = userService;

            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllHousesQueryModel queryModel)
        {
            AllHousesFilteredAndPagedServiceModel serviceModel =
                await houseService.AllAsync(queryModel);

            queryModel.Houses = serviceModel.Houses;
            queryModel.TotalHouses = serviceModel.TotalHousesCount;
            queryModel.Categories = await categoryService.AllCategoryNamesAsync();

            return View(queryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isAgent =
                await agentService.AgentExistsByUserIdAsync(User.GetId()!);
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must become an agent in order to add new houses!";

                return RedirectToAction("Become", "Agent");
            }

            try
            {
                HouseFormModel formModel = new HouseFormModel()
                {
                    Categories = await categoryService.AllCategoriesAsync()
                };

                return View(formModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            bool isAgent =
                await agentService.AgentExistsByUserIdAsync(User.GetId()!);
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must become an agent in order to add new houses!";

                return RedirectToAction("Become", "Agent");
            }

            bool categoryExists =
                await categoryService.ExistsByIdAsync(model.CategoryId);
            if (!categoryExists)
            {
                // Adding model error to ModelState automatically makes ModelState Invalid
                ModelState.AddModelError(nameof(model.CategoryId), "Selected category does not exist!");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();

                return View(model);
            }

            try
            {
                string? agentId = 
                    await agentService.GetAgentIdByUserIdAsync(User.GetId()!);

                string houseId = 
                    await houseService.CreateAndReturnIdAsync(model, agentId!);

                TempData[SuccessMessage] = "House was added successfully!";
                return RedirectToAction("Details", "House", new { id = houseId });
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occurred while trying to add your new house! Please try again later or contact administrator!");
                model.Categories = await categoryService.AllCategoriesAsync();

                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            bool houseExists = await houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                TempData[ErrorMessage] = "House with the provided id does not exist!";

                return RedirectToAction("All", "House");
            }

            try
            {
                HouseDetailsViewModel viewModel = await houseService
                    .GetDetailsByIdAsync(id);
                viewModel.Agent.FullName =
                    await userService.GetFullNameByEmailAsync(User.Identity?.Name!);

                return View(viewModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool houseExists = await houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                TempData[ErrorMessage] = "House with the provided id does not exist!";

                return RedirectToAction("All", "House");
                
                //return this.NotFound(); -> If you want to return 404 page
            }

            bool isUserAgent = await agentService
                .AgentExistsByUserIdAsync(User.GetId()!);
            if (!isUserAgent && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return RedirectToAction("Become", "Agent");
            }

            string? agentId = 
                await agentService.GetAgentIdByUserIdAsync(User.GetId()!);
            bool isAgentOwner = await houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return RedirectToAction("Mine", "House");
            }

            try
            {
                HouseFormModel formModel = await houseService
                    .GetHouseForEditByIdAsync(id);
                formModel.Categories = await categoryService.AllCategoriesAsync();

                return View(formModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, HouseFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();

                return View(model);
            }

            bool houseExists = await houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                TempData[ErrorMessage] = "House with the provided id does not exist!";

                return RedirectToAction("All", "House");
            }

            bool isUserAgent = await agentService
                .AgentExistsByUserIdAsync(User.GetId()!);
            if (!isUserAgent && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return RedirectToAction("Become", "Agent");
            }

            string? agentId =
                await agentService.GetAgentIdByUserIdAsync(User.GetId()!);
            bool isAgentOwner = await houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return RedirectToAction("Mine", "House");
            }

            try
            {
                await houseService.EditHouseByIdAndFormModelAsync(id, model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty,
                    "Unexpected error occurred while trying to update the house. Please try again later or contact administrator!");
                model.Categories = await categoryService.AllCategoriesAsync();

                return View(model);
            }

            TempData[SuccessMessage] = "House was edited successfully!";
            return RedirectToAction("Details", "House", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool houseExists = await houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                TempData[ErrorMessage] = "House with the provided id does not exist!";

                return RedirectToAction("All", "House");
            }

            bool isUserAgent = await agentService
                .AgentExistsByUserIdAsync(User.GetId()!);
            if (!isUserAgent && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return RedirectToAction("Become", "Agent");
            }

            string? agentId =
                await agentService.GetAgentIdByUserIdAsync(User.GetId()!);
            bool isAgentOwner = await houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return RedirectToAction("Mine", "House");
            }

            try
            {
                HousePreDeleteDetailsViewModel viewModel =
                    await houseService.GetHouseForDeleteByIdAsync(id);

                return View(viewModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, HousePreDeleteDetailsViewModel model)
        {
            bool houseExists = await houseService
                .ExistsByIdAsync(id);
            if (!houseExists)
            {
                TempData[ErrorMessage] = "House with the provided id does not exist!";

                return RedirectToAction("All", "House");
            }

            bool isUserAgent = await agentService
                .AgentExistsByUserIdAsync(User.GetId()!);
            if (!isUserAgent && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return RedirectToAction("Become", "Agent");
            }

            string? agentId =
                await agentService.GetAgentIdByUserIdAsync(User.GetId()!);
            bool isAgentOwner = await houseService
                .IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);
            if (!isAgentOwner && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return RedirectToAction("Mine", "House");
            }

            try
            {
                await houseService.DeleteHouseByIdAsync(id);

                TempData[WarningMessage] = "The house was successfully deleted!";
                return RedirectToAction("Mine", "House");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            if (this.User.IsInRole(AdminRoleName))
            {
                return this.RedirectToAction("Mine", "House", new { Area = AdminAreaName });
            }

            List<HouseAllViewModel> myHouses =
                new List<HouseAllViewModel>();

            string userId = User.GetId()!;
            bool isUserAgent = await agentService
                .AgentExistsByUserIdAsync(userId);

            try
            {
                if (User.IsAdmin())
                {
                    string? agentId =
                        await agentService.GetAgentIdByUserIdAsync(userId);

                    // Added houses as an Agent
                    myHouses.AddRange(await houseService.AllByAgentIdAsync(agentId!));

                    // Rented houses as user
                    myHouses.AddRange(await houseService.AllByUserIdAsync(userId));

                    myHouses = myHouses
                        .DistinctBy(h => h.Id)
                        .ToList();
                }
                else if (isUserAgent)
                {
                    string? agentId =
                        await agentService.GetAgentIdByUserIdAsync(userId);

                    myHouses.AddRange(await houseService.AllByAgentIdAsync(agentId!));
                }
                else
                {
                    myHouses.AddRange(await houseService.AllByUserIdAsync(userId));
                }

                return View(myHouses);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Rent(string id)
        {
            bool houseExists = await houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                TempData[ErrorMessage] = "House with provided id does not exist! Please try again!";

                return RedirectToAction("All", "House");
            }

            bool isHouseRented = await houseService.IsRentedAsync(id);
            if (isHouseRented)
            {
                TempData[ErrorMessage] =
                    "Selected house is already rented by another user! Please select another house.";

                return RedirectToAction("All", "House");
            }

            bool isUserAgent =
                await agentService.AgentExistsByUserIdAsync(User.GetId()!);
            if (isUserAgent && !User.IsAdmin())
            {
                TempData[ErrorMessage] = "Agents can't rent houses. Please register as a user!";

                return RedirectToAction("Index", "Home");
            }

            try
            {
                await houseService.RentHouseAsync(id, User.GetId()!);
            }
            catch (Exception)
            {
                return GeneralError();
            }

            this.memoryCache.Remove(RentsCacheKey);

            return RedirectToAction("Mine", "House");
        }

        [HttpPost]
        public async Task<IActionResult> Leave(string id)
        {
            bool houseExists = await houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
                TempData[ErrorMessage] = "House with provided id does not exist! Please try again!";

                return RedirectToAction("All", "House");
            }

            bool isHouseRented = await houseService.IsRentedAsync(id);
            if (!isHouseRented)
            {
                TempData[ErrorMessage] =
                    "Selected house is not rented! Please select one of your houses if you wish to leave them.";

                return RedirectToAction("Mine", "House");
            }

            bool isCurrentUserRenterOfTheHouse =
                await houseService.IsRentedByUserWithIdAsync(id, User.GetId()!);
            if (!isCurrentUserRenterOfTheHouse)
            {
                TempData[ErrorMessage] =
                    "You must be the renter of the house in order to leave it! Please try again with one of your rented houses if you wish to leave them.";

                return RedirectToAction("Mine", "House");
            }

            try
            {
                await houseService.LeaveHouseAsync(id);
            }
            catch (Exception)
            {
                return GeneralError();
            }

            this.memoryCache.Remove(RentsCacheKey);

            return RedirectToAction("Mine", "House");
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] =
                "Unexpected error occurred! Please try again later or contact administrator";

            return RedirectToAction("Index", "Home");
        }
    }
}
