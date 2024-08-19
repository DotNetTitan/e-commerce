using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
