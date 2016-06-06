﻿using Microsoft.AspNet.Mvc;
using Eastlaws.ViewModels.Ahkam;
using Eastlaws.Infrastructure;
using Eastlaws.Services;
using System.Data.SqlClient;
using System.Collections;
using Eastlaws.Entities;
using Dapper;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace Eastlaws.Controllers
{

    public class AhkamController : Controller
    {
        public IActionResult Index() => View();

        #region Search Result, Input Search Box, Filter Section
            public IActionResult SearchResult(int searchtype, int Countsearchchange, int PageNo, int Match,int Sort, string q)
            {
                AhkamSearchOptions Options = new AhkamSearchOptions();
                Options.PageNo = PageNo;
                Options.SortBy = (AhkamSortColumns)Sort;

                if (searchtype == 1)
                {
                    AhkamPresentation Model = AhkamService.Search(Options, new FTSPredicate(q, (FTSSqlModes)Match));
                    if (Model.IsValid)
                    {
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

            [HttpPost]
            public IActionResult SearchResultAssembly(SearchParms SearchParms)
            {
                AssemblySearch AssembleSearchInputs = SearchParms.AssemblySearch;
                SearchTools SearchTools = SearchParms.SearchTools;

                int PageNo = SearchTools.PageNo;
                int Sort = SearchTools.Sort;
                int pageSize = SearchTools.pageSize;
                bool Latest = SearchTools.Latest;
                int Days = SearchTools.Days;
                int typeView = SearchTools.typeView;
                int SortDir = SearchTools.SortDir;

                ViewBag.typeView = typeView;
                AhkamPresentation Model = new AhkamPresentation();
                AhkamSearchOptions Options = new AhkamSearchOptions { SortBy = (AhkamSortColumns)Sort, PageNo = PageNo, PageSize = pageSize, SortDirection = (SearchSortType)SortDir };

                if (Latest == true)
                {
                    Model = AhkamService.GetLatestByDate(Options, SearchParms.ahkamTasfeya);
                }
                else
                {
                    AhkamAdvancedSearch Obj = GetSearchObject(AssembleSearchInputs);
                    if (SearchParms.ahkamTasfeya != null)
                    {
                        Model = AhkamService.Search(Options, Obj, SearchParms.ahkamTasfeya);
                    }
                    else
                    {
                        Model = AhkamService.Search(Options, Obj);
                    }

                }
                if (Model.IsValid)
                {
                    return View("SearchResult", Model);
                }
                else
                {
                    return View("SearchResult");
                }

            }

            [HttpPost]
            public IActionResult SearchResultTadbe2atMada(SearchParms SearchParms)
            {
                Tadbe2atMadaSearch tadbe2atSearchInputs = SearchParms.Tadbe2atMadaSearchSearch;
                 SearchTools SearchTools = SearchParms.SearchTools;

                int PageNo = SearchTools.PageNo;
                int Sort = SearchTools.Sort;
                int pageSize = SearchTools.pageSize;
                int Days = SearchTools.Days;
                int typeView = SearchTools.typeView;
                int SortDir = SearchTools.SortDir;

                ViewBag.typeView = typeView;
                AhkamPresentation Model = new AhkamPresentation();
                AhkamSearchOptions Options = new AhkamSearchOptions { SortBy = (AhkamSortColumns)Sort, PageNo = PageNo, PageSize = pageSize, SortDirection = (SearchSortType)SortDir };

                AhkamMadaSearch Obj = GetTadbe2atMadaSearchObject(tadbe2atSearchInputs);
                if (SearchParms.ahkamTasfeya != null)
                {
                    Model = AhkamService.Search(Options, Obj, SearchParms.ahkamTasfeya);
                }
                else
                {
                    Model = AhkamService.Search(Options, Obj);
                }
                if (Model.IsValid)
                {
               
                    return View("SearchResult", Model);
                }
                else
                {
                    return View("SearchResult");
                }

            }

            [HttpPost]
            public JsonResult TasfeyaListJson(SarchTasfyaParms SarchTasfyaParms)
            {
                List<AhkamTasfeyaSelection> PreviousSelectedItems = new List<AhkamTasfeyaSelection>();
                PreviousSelectedItems = SarchTasfyaParms.ahkamTasfeyaList;
                AhkamAdvancedSearch Obj = GetSearchObject(SarchTasfyaParms.AssemblySearch);
                AhkamSearchTypes SearchType = AhkamSearchTypes.Advanced;
                List<AhkamTasfeyaCategory> UsedCategories;

              var Data = AhkamTasfeya.List(Obj, SearchType, out UsedCategories, SarchTasfyaParms.TasfeyaSearchText, PreviousSelectedItems,(AhkamTasfeyaCategoryIds?)SarchTasfyaParms.SelectedCatgID);
                var datajson = new[] {
                    new object[] { "Data" , Data},
                    new object[] { "Catg" , UsedCategories }
                };
                return new JsonResult(datajson);
            }

            [HttpPost]
            public JsonResult TasfeyaListJsonMada(SarchTasfyaParms SarchTasfyaParms)
            {
                List<AhkamTasfeyaSelection> PreviousSelectedItems = new List<AhkamTasfeyaSelection>();
                PreviousSelectedItems = SarchTasfyaParms.ahkamTasfeyaList;
                AhkamMadaSearch Obj = GetTadbe2atMadaSearchObject(SarchTasfyaParms.Tadbe2atMadaSearchSearch);

                AhkamSearchTypes SearchType = AhkamSearchTypes.Mada;

                List<AhkamTasfeyaCategory> UsedCategories;
                var Data = AhkamTasfeya.List(Obj, SearchType, out UsedCategories, SarchTasfyaParms.TasfeyaSearchText, PreviousSelectedItems, (AhkamTasfeyaCategoryIds?)SarchTasfyaParms.SelectedCatgID);

                var datajson = new[] {new object[] { "Data" , Data}, new object[] { "Catg" , UsedCategories } };
                return new JsonResult(datajson);
            }
        #endregion

        #region droplist
            public JsonResult GetCountries()
            {
                var Countries = AhkamService.GetCountries();
                return new JsonResult(Countries);
            }

            public JsonResult GetGehaType()
            {
                var Countries = AhkamService.GetGehaType();
                return new JsonResult(Countries);
            }

            public JsonResult GetMahakem(int countryID)
            {
                var Mahakem = AhkamService.GetMahakem(countryID);
                return new JsonResult(Mahakem);
            }

            public JsonResult GetMahkamaReplies()
            {
                var MahkamaReplies = AhkamService.GetMahkamaReplies();
                return new JsonResult(MahkamaReplies);
            }
        #endregion

        #region  mapping object
            private AhkamAdvancedSearch GetSearchObject(AssemblySearch AssembleSearchInputs)
            {
                AhkamAdvancedSearch Obj = new AhkamAdvancedSearch();
                Obj.PredicateAny = new FTSPredicate(string.IsNullOrEmpty(AssembleSearchInputs.Alltext) ? "" : AssembleSearchInputs.Alltext, (FTSSqlModes)AssembleSearchInputs.alltextSearchType);
                Obj.PredicateHay2a = new FTSPredicate(string.IsNullOrEmpty(AssembleSearchInputs.hay2a) ? "" : AssembleSearchInputs.hay2a, (FTSSqlModes)AssembleSearchInputs.hay2aSearchType);
                Obj.PredicateHaytheyat = new FTSPredicate(string.IsNullOrEmpty(AssembleSearchInputs.hyseyt) ? "" : AssembleSearchInputs.hyseyt, (FTSSqlModes)AssembleSearchInputs.hyseytSearchType);
                Obj.PredicateMabade2 = new FTSPredicate(string.IsNullOrEmpty(AssembleSearchInputs.Mabdaa) ? "" : AssembleSearchInputs.Mabdaa, (FTSSqlModes)AssembleSearchInputs.MabdaaSearchType);
                Obj.PredicateWakae3 = new FTSPredicate(string.IsNullOrEmpty(AssembleSearchInputs.waka23) ? "" : AssembleSearchInputs.waka23, (FTSSqlModes)AssembleSearchInputs.waka23SearchType);
                Obj.CaseDatefrom = AssembleSearchInputs.dateGalsaFrom;
                Obj.CaseDateTo = AssembleSearchInputs.dateGalsaTo;
                Obj.CaseYear = AssembleSearchInputs.caseYear;
                Obj.CaseNo = AssembleSearchInputs.caseNo;
                Obj.CountryIDs = AssembleSearchInputs.country == "0" ? "" : AssembleSearchInputs.country;
                Obj.IFAgree = AssembleSearchInputs.MahkamaReplay == "0" ? "" : AssembleSearchInputs.MahkamaReplay;
                Obj.Ma7akemIds = AssembleSearchInputs.mahakem;
                Obj.OfficeSuffix = AssembleSearchInputs.omarGroup;
                Obj.OfficeYear = AssembleSearchInputs.officeYear;
                Obj.PageNo = AssembleSearchInputs.pageNo;
                Obj.PartNo = AssembleSearchInputs.partNo;
                return Obj;
            }

            private AhkamMadaSearch GetTadbe2atMadaSearchObject(Tadbe2atMadaSearch Tadbe2atMadaSearch)
            {
                AhkamMadaSearch Obj = new AhkamMadaSearch();
                Obj.SearchPredicate = new FTSPredicate(string.IsNullOrEmpty(Tadbe2atMadaSearch.MadaText) ? "" : Tadbe2atMadaSearch.MadaText, (FTSSqlModes)Tadbe2atMadaSearch.MadaTextType);
                Obj.SearchInMadaText = true;
                Obj.SearchInTashText = true;
                Obj.CountryIds = Tadbe2atMadaSearch.country;
                Obj.MadaNo = Tadbe2atMadaSearch.madaNo;
                Obj.TashNo = Tadbe2atMadaSearch.tashNo;
                Obj.TashYear = Tadbe2atMadaSearch.tashYear;


                return Obj;
            }
        #endregion

        #region displayHokm
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

            [HttpPost]
            public IActionResult FullHokmView(int ID, int[] fakarat)
            {
                var Model = AhkamService.GetHokm(ID, null);
                if (Model.IsValid)
                {
                    if (fakarat.Length > 0)
                    {
                        ViewBag.PrintedFakarat = fakarat;
                    }
                    return View(Model);
                }
                else
                {
                    return View();
                }
            }
        #endregion






        //public IActionResult TasfeyaList(int QueryID)
        //{
        //    if (QueryID == 0)
        //        QueryID = 56;

        //    List<AhkamTasfeyaCategory> UsedCategories;
        //    var Data = AhkamTasfeya.List(QueryID, AhkamSearchTypes.Advanced, out UsedCategories, "", null, null);
        //    return View(Data);
        //}


        //public JsonResult TasfyaListJson(int QueryID)
        //{

        //    List<AhkamTasfeyaSelection> PreviousSelectedItems = new List<AhkamTasfeyaSelection>();
        //    // Uncomment the Following line to Safy the Tasfeya list 
        //    //PreviousSelectedItems.Add(new AhkamTasfeyaSelection { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "1,2,3,4" });


        //    List<AhkamTasfeyaCategory> UsedCategories;
        //    var Data = AhkamTasfeya.List(QueryID, AhkamSearchTypes.Advanced, out UsedCategories, "", PreviousSelectedItems, null);
        //    var datajson = new[] {
        //        new object[] { "Data" , Data},
        //        new object[] { "Catg" , UsedCategories }
        //    };
        //    return new JsonResult(datajson);
        //    //  return new JsonResult(new { Data="Data", UsedCategories = "Catg" });
        //}

        //public IActionResult TasfyaSearch()
        //{
        //    return View();
        //}

        // public JsonResult TasfeyaListJson(AssemblySearch AssemblySearchInputs ,  List<AhkamTasfeyaSelection> ahkamTasfeya,string TasfeyaSearch = "" )
        //public JsonResult TasfeyaListJson(AssemblySearch AssemblySearchInputs, string TasfeyaSearch = "")


        // Mada Search 
        //[HttpPost]
        //public IActionResult SearchResultAssemblyMada(AhkamMadaSearch MadaObj , AhkamSearchOptions Options , List<AhkamTasfeyaSelection> TasfeyaSelection)
        //{
        //    AhkamPresentation Model =  AhkamService.Search(Options, MadaObj, TasfeyaSelection);
        //    if (Model.IsValid)
        //    {
        //        ViewBag.typeView = 2;
        //        return View("SearchResult", Model);
        //    }
        //    else
        //    {
        //        return View("SearchResult");
        //    } 
        //}



        //public IActionResult Latest(int Days = 10, int PageNo = 1, int Sort = 5, int pageSize = 10, int typeView = 1)
        //{
        //    AhkamSearchOptions Options = new AhkamSearchOptions { PageNo = PageNo, SortBy = (AhkamSortColumns)Sort, SortDirection = SearchSortType.DESC, PageSize = pageSize };
        //    AhkamPresentation Model = AhkamService.GetLatest(Options, Days);
        //    return View("SearchResult", Model);
        //}


        //[HttpPost]
        //public ActionResult test2([FromBody] List<AhkamTasfeyaSelection> ahkamTasfeya)
        //{
        //    ViewBag.ahkamTasfeya = ahkamTasfeya;
        //    return View();
        //}

        //public IActionResult test1(string IDS)
        //{
        //    List<AhkamTasfeyaSelection> SelectedItems = new List<AhkamTasfeyaSelection>();
        //    //--- test1

        //    //if (countrIDs != "")
        //    //{
        //    //    string[] ids = ["1", "2", "3"];
        //    //    SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "1" });
        //    //    SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "2" });
        //    //    SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "3" });
        //    //}

        //    // test 2
        //    /*

        //    IDS =  {
        //            { catgID:1,IDs:1,2,3},
        //            { catgID:5,IDs:1,2,3}
        //        }
        //    eventfire = catg:1

        //    // pares IDs to list of SelectedItems
        //    */

        //    //test 3
        //    /*
        //     List<AhkamTasfeyaSelection> IDS 

        //          IDS =  {
        //                { CategoryID:1,Parameter:1},
        //                { CategoryID:1,Parameter:2},
        //                { CategoryID:1,Parameter:3},
        //                { CategoryID:5,Parameter:1},
        //                { CategoryID:5,Parameter:6},
        //                { CategoryID:6,Parameter:10},
        //        }
        //    eventfire = catg:1

        //    */

        //    //-----
        //    return View();
        //}




    }


}
