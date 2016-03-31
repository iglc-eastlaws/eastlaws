using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Eastlaws.Entities;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Eastlaws.Controllers
{
    public class TashController : Controller
    {
        Tash t = new Tash();
        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = t.GetAll();
            return View(model);
        }

        public IActionResult Details(int ID)
        {
            var model = t.Find(ID);
            return View(model);
        }
    }
}
