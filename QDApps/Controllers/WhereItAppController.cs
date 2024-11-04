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
        [HttpGet]
        public IActionResult EditItem(int itemId)
        {
            int userId = GetCurrentUserId();
            if (!_modelHelper.IsItemOwnedByUser(userId, itemId)){
                return RedirectToAction("Index");
            };

            ViewItem item = _modelHelper.GetItem(itemId, userId);
            
            return View(item);
        }
        [HttpPost]
        public IActionResult EditItem(ViewItem item)
        {

            return RedirectToAction("Index");
        }

        public IActionResult AddTag(int tagId, int itemId)
        {
            int userId = GetCurrentUserId();
            if (!_modelHelper.IsItemOwnedByUser(userId, itemId) || !_modelHelper.IsTagOwnedByUser(userId, tagId))
            {
                return RedirectToAction("Index");
            }

            Status status = _modelHelper.AddItemTag(tagId, itemId);

            if (status.IsSuccess)
            {
                return RedirectToAction("EditItem", new { itemId = itemId });
            }
            else
            {
                return RedirectToAction("Error", new { message = status });
            }
        }
        public IActionResult RemoveTag(int tagId, int itemId)
        {
            int userId = GetCurrentUserId();
            if (!_modelHelper.IsItemOwnedByUser(userId, itemId) || !_modelHelper.IsTagOwnedByUser(userId, tagId))
            {
                return RedirectToAction("Index");
            }
            Status status = _modelHelper.RemoveItemTag(tagId, itemId);

            if (status.IsSuccess)
            {
                return RedirectToAction("EditItem", new { itemId = itemId });
            }
            else
            {
                return RedirectToAction("Error", new { message = status });
            }

        }
        [HttpPost]
        public IActionResult DeleteItem(int itemId)
        {
            return RedirectToAction("Index");

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
