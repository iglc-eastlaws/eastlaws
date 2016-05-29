﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Eastlaws.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Eastlaws.Controllers
{
    public class FehresController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var m = FaharesService.GetCountriesByFehressID(30);
            return View(m);
        }
    }
}
