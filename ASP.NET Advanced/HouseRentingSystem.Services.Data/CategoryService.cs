namespace HouseRentingSystem.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using HouseRentingSystem.Data;
    using Interfaces;
    using Web.ViewModels.Category;

    public class CategoryService : ICategoryService
    {
        private readonly HouseRentingDbContext dbContext;

        public CategoryService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<HouseSelectCategoryFormModel>> AllCategoriesAsync()
        {
            IEnumerable<HouseSelectCategoryFormModel> allCategories = await this.dbContext
                .Categories
                .AsNoTracking()
                .Select(c => new HouseSelectCategoryFormModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToArrayAsync();

            return allCategories;
        }
    }
}
