﻿using Microsoft.AspNet.Mvc;
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
        

        //[HttpPost]
        //public IActionResult GeneralSearch(GeneralSearchVM Model)
        //{

        //    return null;
        //}

        //[HttpPost]
        //public IActionResult CustomSearch(CustomSearchVM Model)
        //{
        //    return null;
        //}

        //[HttpPost]
        //public IActionResult AdvancedSearch(AdvancedTextSearchVM Model)
        //{
        //    return null;
        //}

        //public IActionResult Index_Old()
        //{
        //    ViewBag.Title = "أحكام المحاكم العربية العليا ";
        //    return View();
        //}




        //public IActionResult Test()
        //{




        //    return null;
        //}



        //public JsonResult SearchCount(string q = "")
        //{

        //    var Result = 3000; //count of rows
        //    return Json(new { data = Result });
        //}

    }
}
