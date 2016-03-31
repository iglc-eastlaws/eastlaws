using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Eastlaws.ViewModels.Ahkam;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Eastlaws.Controllers
{
    public class AhkamController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GeneralSearch(GeneralSearchVM Model , int? Test)
        {
            return View(Model);
        }

        [HttpPost]
        public IActionResult CustomSearch(CustomSearchVM Model)
        {

            return View(Model);
        }


    }
}
