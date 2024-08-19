using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
