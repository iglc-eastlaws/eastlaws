using Microsoft.AspNet.Mvc;
using Eastlaws.ViewModels;
using Eastlaws.Services;
using Eastlaws.Entities;

namespace Eastlaws.Controllers
{
    public class HomeController : Controller
    {

        private IRestaurantData _restaurantData;
        private string _greeter;

        public HomeController(IRestaurantData restaurantData)
        {
            _restaurantData = restaurantData;
            _greeter = "Hello Home Controller";
        }

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
