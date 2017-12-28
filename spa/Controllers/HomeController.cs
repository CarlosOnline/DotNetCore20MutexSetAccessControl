using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Util;

namespace DotNetCore20MutexSetAccessControl.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var mutexTest = new MutexTest("MyTest");
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
