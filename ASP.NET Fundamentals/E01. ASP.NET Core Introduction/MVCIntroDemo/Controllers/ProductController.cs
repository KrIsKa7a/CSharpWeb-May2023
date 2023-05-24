namespace MVCIntroDemo.Controllers
{
    using System.Text;
    using System.Text.Json;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    using ViewModels.Product;
    using static Seeding.ProductsData;

    public class ProductController : Controller
    {
        public IActionResult All(string keyword)
        {
            if (String.IsNullOrWhiteSpace(keyword))
            {
                return View(Products);
            }

            IEnumerable<ProductViewModel> productsAfterSearch = Products
                .Where(p => p.Name.ToLower().Contains(keyword.ToLower()))
                .ToArray();
            return View(productsAfterSearch);
        }

        [Route("/Product/Details/{id?}")]
        public IActionResult ById(string id)
        {
            ProductViewModel? product = Products
                .FirstOrDefault(p => p.Id.ToString().Equals(id));
            if (product == null)
            {
                return this.RedirectToAction("All");
            }

            return this.View(product);
        }

        public IActionResult AllAsJson()
        {
            return Json(Products, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
        }

        public IActionResult DownloadProductsInfo()
        {
            // This should be in separate class
            StringBuilder sb = new StringBuilder();
            foreach (var product in Products)
            {
                sb
                    .AppendLine($"Product with Id: {product.Id}")
                    .AppendLine($"## Product Name: {product.Name}")
                    .AppendLine($"## Price: {product.Price:f2}$")
                    .AppendLine($"-------------------------------");
            }

            Response.Headers
                .Add(HeaderNames.ContentDisposition, "attachment;filename=products.txt");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain");
        }
    }
}
