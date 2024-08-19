using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
