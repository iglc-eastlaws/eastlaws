using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Infrastructure;
using Eastlaws.Services;

namespace Eastlaws.Controllers
{
    public class GeneralSearchController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
        public ViewResult Index1()
        {
            var m = GeneralSearchQuery.Search("القتل العمد");
            return View(m);
        }

        public ViewResult SearchResult(string q = "")
        {
            //   q = "القتل العمد";
            if (q.Trim() == string.Empty){ return View();}

            var m = GeneralSearchQuery.Search(q);
            ViewBag.mCount = m.Count();
            return View(m);
        }


        public ViewResult test()
        {
            string inputtext = "القتل    العمد مع   سبق الاصرار و الترصد";
            string outputtextand = TextHelpers.Build_and(
                TextHelpers.RemoveNoiseWords(inputtext),Build_Option.and
                ).ToString();

            string outputtextor = TextHelpers.Build_and(inputtext, Build_Option.or).ToString();

            //int x = 0;
            return View();
        }

        public IActionResult Test2()
        {
            string Input = "القتل العمد مع سبق الإصرار ";

            FTSPredicate p = new FTSPredicate(Input );
            //string dbLiteral = p.BuildPredicate(MatchType.AllWords);
            return View();
        }
    }
}
