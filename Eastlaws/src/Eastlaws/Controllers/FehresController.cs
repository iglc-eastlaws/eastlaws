using System;
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
            //var m = FaharesService.GetCountriesByFehressID(30);
            var data = FaharesService.GetProgramsByService(1);
            return View(data);
        }



        public JsonResult ServicePrograms(int servicesID)
        {
            var data = FaharesService.GetProgramsByService(servicesID);
            return new JsonResult(data);
        }

        public JsonResult ProgramCountries(int FehresProgramID)
        {
            var data = FaharesService.GetCountriesByFehressID(FehresProgramID);
            return new JsonResult(data);
        }

        public JsonResult Categories (int ProgCountryID)
        {
            var data = FaharesService.GetCategoriesByProgCountry(ProgCountryID);
            return new JsonResult(data);
            
        }

        public JsonResult Items(int CategoryID , int? ParentID = null, string SearchText = null)
        {
            var Data = FaharesService.GetItems(CategoryID, SearchText);
            return new JsonResult(Data);
        }




    }
}
