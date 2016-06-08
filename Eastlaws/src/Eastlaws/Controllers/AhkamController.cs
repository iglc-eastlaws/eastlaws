using Microsoft.AspNet.Mvc;
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
        public IActionResult SearchResult(int searchtype, int Countsearchchange, int PageNo, int Match, int Sort, string q)
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
        public IActionResult SearchResultFehres(int FehresItemID , AhkamSearchOptions Options , List<AhkamTasfeyaSelection> TasfeyaSelection = null )
        {
            AhkamPresentation Model = AhkamService.Search(Options, FehresItemID, TasfeyaSelection);
            if (Model.IsValid)
            {

                return View("SearchResult", Model);
            }
            else
            {
                return View("SearchResult");
            }

        }


        #endregion


        #region Tasfeya 
        [HttpPost]
        public JsonResult TasfeyaListJson(SarchTasfyaParms SarchTasfyaParms)
        {
            List<AhkamTasfeyaSelection> PreviousSelectedItems = new List<AhkamTasfeyaSelection>();
            PreviousSelectedItems = SarchTasfyaParms.ahkamTasfeyaList;
            AhkamAdvancedSearch Obj = GetSearchObject(SarchTasfyaParms.AssemblySearch);

            List<AhkamTasfeyaCategory> UsedCategories;

            var Data = AhkamTasfeya.List(Obj, out UsedCategories, SarchTasfyaParms.TasfeyaSearchText, PreviousSelectedItems, (AhkamTasfeyaCategoryIds?)SarchTasfyaParms.SelectedCatgID);
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

            List<AhkamTasfeyaCategory> UsedCategories;
            var Data = AhkamTasfeya.List(Obj, out UsedCategories, SarchTasfyaParms.TasfeyaSearchText, PreviousSelectedItems, (AhkamTasfeyaCategoryIds?)SarchTasfyaParms.SelectedCatgID);

            var datajson = new[] { new object[] { "Data", Data }, new object[] { "Catg", UsedCategories } };
            return new JsonResult(datajson);
        }

        [HttpPost]
        public JsonResult TasfeyaListJsonFehres(int FehresItemID , string TasfeyaFilter , List<AhkamTasfeyaSelection> TasfeyaSelection , AhkamTasfeyaCategoryIds? Sender  )
        {

            List<AhkamTasfeyaCategory> UsedCategories;
            var Data = AhkamTasfeya.List(FehresItemID, out UsedCategories, TasfeyaFilter, TasfeyaSelection, Sender);

            var datajson = new[] { new object[] { "Data", Data }, new object[] { "Catg", UsedCategories } };
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



    }


}
