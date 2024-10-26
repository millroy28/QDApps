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
                int userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return RedirectToAction("EditUser");
                } 
                else
                {
                    return RedirectToAction("Welcome");
                };

            }
            return View();
        }

        public IActionResult Welcome()
        {
            int userId = GetCurrentUserId();

            var userName = _modelHelper.GetUserName(userId);

            return View("Welcome", userName);
        }
     
        [HttpGet] public IActionResult EditUser()
        {
            int userId = GetCurrentUserId();
            User user = new User();

            if (userId != 0) { user = _modelHelper.GetUser(userId); };

            EditUser editUser = new()
            {
                TimeZones = _modelHelper.GetTimeZones(),
                UserName = userId == 0 ? "UserName" : user.UserName,
                TimeZoneId = userId == 0 ? 6 : user.TimeZoneId,
                UserId = userId == 0 ? userId : -1,
            };

            return View(editUser);
        }
        [HttpPost] public IActionResult EditUser(EditUser editUser)
        {
            editUser.UserId = GetCurrentUserId();
            editUser.AspNetUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool status = _modelHelper.EditUser(editUser);

            if (status)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { message = "Borked on editing your user details - whoop!" });
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

        public int GetCurrentUserId()
        {
            int userId = 0;
            var aspNetUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (aspNetUserId != null)
            {
                userId = _modelHelper.GetUserId(aspNetUserId);
            }
            return userId;
        }
    }
}
