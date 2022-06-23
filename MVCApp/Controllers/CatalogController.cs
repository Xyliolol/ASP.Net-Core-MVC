using Microsoft.AspNetCore.Mvc;
using MVCApp.Interface;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class CatalogController : Controller
    {
        private static Catalog _catalog = new();
        private object _lock = new object();
        private readonly IEmailSender _emailSender;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IEmailSender emailSender, ILogger<CatalogController> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Products()
        {
            return View(_catalog);
        }

        [HttpPost]
        public IActionResult Products(Product product)
        { 
            lock (_lock)
            {
                _emailSender.SendEmailAsync(product);
            }
            _catalog.ProductAdd(product);
                    
            return RedirectToAction("Products");
        }

        [HttpPost, ActionName("ProductDelete")]
        public IActionResult ProductDeleteConfirmed(int id,Product product)
        {
            lock (_lock)
            {
                _emailSender.SendEmailAsync(product);
            }
            _catalog.ProductDelete(id);
           
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult ProductsAdd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductDelete(int id)
        {
            var product = _catalog.ProductsGetAll()
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
