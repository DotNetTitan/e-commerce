using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
