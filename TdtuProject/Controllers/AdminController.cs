using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TdtuProject.Controllers
{
    
    public class AdminController : Controller
    {
        public IActionResult Display()
        {
            return View();
        }
    }
}
