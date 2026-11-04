using Microsoft.AspNetCore.Mvc;

namespace front.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();
        public IActionResult Register() => View();
    }
}