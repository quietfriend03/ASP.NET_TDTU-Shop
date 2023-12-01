using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TdtuProject.EF;
using TdtuProject.Repository.Implementation;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        public async Task<IActionResult> Index()
        {
            var order = await _checkoutService.GetAllAsync();
            return View(order);
        }
    }
}
