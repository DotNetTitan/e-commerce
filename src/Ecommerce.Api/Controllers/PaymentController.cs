using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
