using Microsoft.AspNetCore.Mvc;
using QDApps.Context;
using QDApps.Models;
using QDApps.Models.WhereItAppModels;
using QDApps.Models.WhereItAppModels.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace QDApps.Controllers
{
    public class WhereItAppController : Controller
    {
        private readonly ILogger<WhereItAppController> _logger;
        public WhereItAppModelHelper _modelHelper;

        public WhereItAppController(ILogger<WhereItAppController> logger, WhereItAppModelHelper modelHelper)
        {
            _logger = logger;
            _modelHelper = modelHelper;
        }

        public IActionResult Index()
        {
            // New User Setup Check
            int userId = GetCurrentUserId();
            bool newUser = _modelHelper.IsUserNew(userId);
            if (newUser) { return RedirectToAction("Welcome"); }; 

            ViewInventory inventory = _modelHelper.GetInventory(userId);

            return View(inventory);
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            // New User Setup Check
            int userId = GetCurrentUserId();
            bool newUser = _modelHelper.IsUserNew(userId);
            if (!newUser) { return RedirectToAction("Index"); };

            return View();
        }

        [HttpPost]
        public IActionResult Welcome(EditWelcomeStash editWelcomeStash)
        {
            editWelcomeStash.UserId = GetCurrentUserId();
            Status status = _modelHelper.CreateWelcomeStashSet(editWelcomeStash);

            if (status.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { message = status});
            }

         
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
        }

    }
}
