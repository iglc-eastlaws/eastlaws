using Microsoft.AspNet.Mvc;
using Eastlaws.ViewModels.Ahkam;
using Eastlaws.Infrastructure;
using Eastlaws.Services;
using System.Data.SqlClient;

namespace Eastlaws.Controllers
{
    public class AhkamController : Controller
    {

        [HttpPost]
        public IActionResult GeneralSearch(GeneralSearchVM Model)
        {

            return null;
        }

        [HttpPost]
        public IActionResult CustomSearch(CustomSearchVM Model)
        {
            return null;
        }

        [HttpPost]
        public IActionResult AdvancedSearch(AdvancedTextSearchVM Model)
        {
            return null;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "أحكام المحاكم العربية العليا ";
            return View();
        }

        public IActionResult Test()
        {
  

            string Input = "القتل العمد مع سبق الإصرار والترصد";
            FTSPredicate p = new FTSPredicate(Input, FTSSqlModes.AND);
            string FullQuery = AhkamQueryBuilder.GeneralSearch(p.BuildPredicate());

            QueryCacher Cacher = new QueryCacher(1, FullQuery, "General", true);
            int MyID = Cacher.ID;




            return null;
        }
    }
}
