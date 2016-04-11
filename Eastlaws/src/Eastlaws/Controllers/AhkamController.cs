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


            var x1 = new Range("1,2,3,4,5,6,7,8,9,0", "A.HokmNo").GetCondition();
            var x2 = new Range("1,2,3,4,5,6,7,8,9,", "A.HokmNo").GetCondition();
            var x3 = new Range("1,,,,", "A.HokmNo").GetCondition();
            var x4 = new Range("1-5", "A.HokmNo").GetCondition();
            var x5 = new Range("11-100", "A.HokmNo").GetCondition();
            var x6 = new Range("11-", "A.HokmNo").GetCondition();





            string Input = "القتل العمد مع سبق الإصرار والترصد";
            Input = "سرقة بالإكراه ";
            FTSPredicate p = new FTSPredicate(Input, FTSSqlModes.AND);
            string FullQuery = AhkamQueryBuilder.GeneralSearch(p.BuildPredicate());

            QueryCacher Cacher = new QueryCacher(1, FullQuery, "General", true);
            string CachedQuery = Cacher.GetCachedQuery();


            int PageNo = 1, PageSize = 10;

            string PagedQuery = AhkamQueryBuilder.GetOuterQuery(CachedQuery, PageSize , PageNo);

            IEnumerable Ahkam = DataHelpers.GetConnection(DbConnections.Data).Query<VW_Ahkam>(PagedQuery);




            return null;
        }



        public IActionResult Index1()
        {

            return View();
        }

        public ViewResult SearchResult(string q = "")
        {
            if (q.Trim() == string.Empty) {
                ViewBag.mCount = 0;
                return View();
            }

            FTSPredicate p = new FTSPredicate(q.Trim(), FTSSqlModes.AND);
            string FullQuery = AhkamQueryBuilder.GeneralSearch(p.BuildPredicate());
            QueryCacher Cacher = new QueryCacher(1, FullQuery, "General", true);
            string CachedQuery = Cacher.GetCachedQuery();
            int PageNo = 1, PageSize = 10;
            string PagedQuery = AhkamQueryBuilder.GetOuterQuery(CachedQuery, PageSize, PageNo);
            var Ahkam = DataHelpers.GetConnection(DbConnections.Data).Query<VW_Ahkam>(PagedQuery).AsList();
            ViewBag.mCount = Ahkam.Count;

            return View(Ahkam);
        }


        public JsonResult SearchCount(string q = "")
        {
            var Result = 3000; //count of rows
            return Json(new { data = Result });
        }

    }
}
