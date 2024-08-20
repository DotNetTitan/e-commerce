using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
