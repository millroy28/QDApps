using Microsoft.AspNetCore.Mvc;
using QDApps.Context;
using QDApps.Models;
using QDApps.Models.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace QDApps.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ModelHelper _modelHelper;

        public HomeController(ILogger<HomeController> logger, ModelHelper modelHelper)
        {
            _logger = logger;
            _modelHelper = modelHelper;
        }

        public IActionResult Index()
        {
            if (User.Identities.First().IsAuthenticated)
            {
                var aspNetUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (_modelHelper.IsUserNew(aspNetUserId))
                {
                    return RedirectToAction("SetupNewUser", new {aspNetUserId = aspNetUserId});
                };

            }


            return View();
        }
        [HttpGet]
        public IActionResult SetupNewUser(string aspNetUserId)
        {
            EditUser newUser = new()
            {
                AspNetUserId = aspNetUserId,
                TimeZoneId = 6,
                TimeZones = _modelHelper.GetTimeZones()
            };
            return View(newUser);
        }
        [HttpPost]
        public IActionResult SetupNewUser(EditUser newUser)
        {
            bool status = _modelHelper.CreateNewUser(newUser);

            if (status)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { message = "Borked on creating your user details - whoop!" });
            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
        }
    }
}
