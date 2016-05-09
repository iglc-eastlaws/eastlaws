using Microsoft.AspNet.Mvc;
using Eastlaws.ViewModels;
using Eastlaws.Services;
using Eastlaws.Entities;

namespace Eastlaws.Controllers
{
    public class HomeController : Controller
    {

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Details()
        {
            return View();
        }


    }
}
