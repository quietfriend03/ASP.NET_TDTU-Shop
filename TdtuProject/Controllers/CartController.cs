using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TdtuProject.EF;
using TdtuProject.Models;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(IProductService productService, IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            _databaseContext = databaseContext;
        }

        public IActionResult Index()
        {
            var cartJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new Cart { CartId = Guid.NewGuid(), Orders = new List<Order>() } : JsonConvert.DeserializeObject<Cart>(cartJson);

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);

            var cart = _httpContextAccessor.HttpContext.Session.GetString("Cart");
            var cartObject = string.IsNullOrEmpty(cart) ? new Cart { CartId = Guid.NewGuid(), DateCreated = DateTime.Now, Orders = new List<Order>() } : JsonConvert.DeserializeObject<Cart>(cart);

            var existedOrder = cartObject.Orders.FirstOrDefault(order => order.Product.ProductId == productId);

            if (existedOrder != null)
            {
                existedOrder.Quantity += 1;
            }
            else
            {
                cartObject.Orders.Add(new Order { ProductId = productId, Product = product, Quantity = 1 });
            }

            _httpContextAccessor.HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartObject));
            var totalCartPrice = cartObject.Orders.Sum(order => order.TotalPrice());
            ViewBag.TotalCartPrice = totalCartPrice;

            return RedirectToAction("Index", "Cart");

        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(string Name, string Address)
        {
            var existedCart = _httpContextAccessor.HttpContext.Session.GetString("Cart");
            if(!string.IsNullOrEmpty(existedCart))
            {
                var cartObject = JsonConvert.DeserializeObject<Cart>(existedCart);
                CheckoutViewModel model = new CheckoutViewModel()
                {
                    Id = cartObject.CartId,
                    Name = Name, 
                    Address = Address,
                    DateCreated = cartObject.DateCreated,
                    TotalPrice = cartObject.TotalPrice,
                };

                _databaseContext.CheckoutViewModel.Add(model);
                await _databaseContext.SaveChangesAsync();

                HttpContext.Session.Clear();
                bool isSessionCleared = HttpContext.Session.Keys.Count() == 0;
            }
            return RedirectToAction("Display", "Dashboard");
        }
    }
}
