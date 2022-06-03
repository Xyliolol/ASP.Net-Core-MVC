using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class AddProductController : Controller
    {
        private AddProduct catalog;
        public AddProductController(AddProduct catalog)
        {
            this.catalog = catalog;
        }
        [HttpPost]
        public IActionResult AddProducts([FromForm] GoodsCategory category)
        {
            catalog.Categories.Add(category);
            return View(catalog);
        }
        [HttpGet]
        public IActionResult AddProducts()
        {
            return View();
        }
    }
}
