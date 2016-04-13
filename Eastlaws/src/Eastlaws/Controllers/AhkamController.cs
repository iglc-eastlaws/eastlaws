using Microsoft.AspNet.Mvc;
using Eastlaws.ViewModels.Ahkam;
using Eastlaws.Infrastructure;
using Eastlaws.Services;
using System.Data.SqlClient;
using System.Collections;
using Eastlaws.Entities;
using Dapper;

namespace Eastlaws.Controllers
{
    public class AhkamController : Controller
    {
        public IActionResult View(int ID)
        {
            var Model = AhkamService.GetHokm(ID, null);
            if (Model.IsValid)
            {
                return View(Model);
            }
            else
            {
                return null;
            }
     
        }

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




            return null;
        }



        public IActionResult Index1()
        {

            return View();
        }

        public ViewResult SearchResult(string q = "")
        {

          
            AhkamPresentation Model = AhkamService.Search(new AhkamSearchOptions(), new FTSPredicate(q));
            if (Model.IsValid)
            {
                var Ahkam = Model.AhkamList;
                ViewBag.mCount = Model.ResultsCount;
                return View(Ahkam);
            }
            else
            {
                ViewBag.mCount = 0;
                return View();
            }

            /*
                        if (q.Trim() == string.Empty) {
                ViewBag.mCount = 0;
                return View();
                  }
            FTSPredicate p = new FTSPredicate(q.Trim(), FTSSqlModes.AND);
            string FullQuery = AhkamQueryBuilder.GeneralSearch(p);
            QueryCacher Cacher = new QueryCacher(1, FullQuery, "General", true);
            string CachedQuery = Cacher.GetCachedQuery();
            int PageNo = 1, PageSize = 10;
            string PagedQuery = AhkamQueryBuilder.GetOuterQuery(CachedQuery, PageSize, PageNo);
            var Ahkam = DataHelpers.GetConnection(DbConnections.Data).Query<VW_Ahkam>(PagedQuery).AsList();
            ViewBag.mCount = Ahkam.Count;
            return View(Ahkam);
            */

        }


        public JsonResult SearchCount(string q = "")
        {

            var Result = 3000; //count of rows
            return Json(new { data = Result });
        }

       

    }
}
