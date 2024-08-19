using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
