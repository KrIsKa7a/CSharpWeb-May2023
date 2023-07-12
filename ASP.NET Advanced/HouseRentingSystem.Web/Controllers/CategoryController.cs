namespace HouseRentingSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Services.Data.Interfaces;
    using ViewModels.Category;

    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<AllCategoriesViewModel> viewModel =
                await this.categoryService.AllCategoriesForListAsync();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, string information)
        {
            bool categoryExists = await this.categoryService.ExistsByIdAsync(id);
            if (!categoryExists)
            {
                return this.NotFound();
            }

            CategoryDetailsViewModel viewModel =
                await this.categoryService.GetDetailsByIdAsync(id);
            if (viewModel.GetUrlInformation() != information)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}
