using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Rinya_App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["pr"] = "a ver";
            ViewData["Message"] = "Prueba";
            return View();
        }

        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";
            ViewBag.Message = "Prueba!";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
