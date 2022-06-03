using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class CatalogController : Controller
    {
        private AddProduct catalog;
        public CatalogController(AddProduct catalog)
        {
            this.catalog = catalog;
        }

        public IActionResult Categories(GoodsCategory category)
        {
            return View(catalog);
        }
    }
}
