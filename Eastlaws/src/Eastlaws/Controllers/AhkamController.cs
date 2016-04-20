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



        public IActionResult Index()
        {

            return View();
        }

        public IActionResult SearchResult(int searchtype, int Countsearchchange, int PageNo, int Match, string q)
        {
            AhkamSearchOptions Options = new AhkamSearchOptions();
            Options.PageNo = PageNo;
            //Options.SortBy

            if (searchtype == 1)
            {
                AhkamPresentation Model = AhkamService.Search(Options, new FTSPredicate(q, (FTSSqlModes)Match));
                if (Model.IsValid)
                {
                    //ViewBag.aa = Match;
                    return View(Model);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult View(int ID)
        {
            var Model = AhkamService.GetHokm(ID, null);
            if (Model.IsValid)
            {
                return View(Model);
            }
            else
            {
                return View();
            }
     
        }
        public IActionResult TasfyaSearch()
        {
            return View();
        }


        public IActionResult TestBesada()
        {
            AhkamAdvancedSearch Obj = new AhkamAdvancedSearch();
            Obj.CountryIDs = "1";
           // Obj.PredicateAny = new FTSPredicate("القتل العمد مع سبق الإصرار ", FTSSqlModes.AND);
            Obj.PredicateHay2a = new FTSPredicate("برئاسة", FTSSqlModes.None);
            Obj.PredicateDestoreya = new FTSPredicate("نقض الحكم");

            AhkamService.Search(new AhkamSearchOptions(), Obj);

            return null;
        }
        

    }
}
