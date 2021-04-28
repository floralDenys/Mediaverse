using Mediaverse.Domain.Authentication.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Mediaverse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        
        public HomeController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "ContentConsumption");
            }
            
            return View();
        }
    }
}