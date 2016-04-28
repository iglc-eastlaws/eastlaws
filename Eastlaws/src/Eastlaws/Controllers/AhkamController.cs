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

namespace Eastlaws.Controllers
{

    public class AhkamController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }

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
        public IActionResult SearchResultAssembly(AssemblySearch AssembleSearchInputs,int PageNo,int Sort,int pageSize,
                                                   bool Latest = false,int Days = 10, 
                                                   int typeView = 1)
        {

            ViewBag.typeView = typeView;
            AhkamPresentation Model = new AhkamPresentation();
            AhkamSearchOptions Options = new AhkamSearchOptions { SortBy = (AhkamSortColumns)Sort, PageNo = PageNo, PageSize = pageSize };

            if (Latest == true)
            {
                 Model = AhkamService.GetLatest(Options, Days);
            }
            else {
                //AssemblySearch AssembleSearchInputs = new AssemblySearch();
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
                Model = AhkamService.Search(Options, Obj);
            }
          //  AhkamPresentation Model1 = AhkamService.Search(Options, Obj);
                if (Model.IsValid)
                {
                    return View("SearchResult", Model);
            }
                else
                {
                    return View("SearchResult");
                }
           
        }

        //public IActionResult Latest(int Days = 10, int PageNo = 1, int Sort = 5, int pageSize = 10, int typeView = 1)
        //{
        //    AhkamSearchOptions Options = new AhkamSearchOptions { PageNo = PageNo, SortBy = (AhkamSortColumns)Sort, SortDirection = SearchSortType.DESC, PageSize = pageSize };
        //    AhkamPresentation Model = AhkamService.GetLatest(Options, Days);
        //    return View("SearchResult", Model);
        //}

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



        public IActionResult FullHokmView(int ID)
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
        public IActionResult TasfeyaList  (int QueryID)
        {
            if (QueryID == 0)
                QueryID = 56;

            List<AhkamTasfeyaCategory> UsedCategories;
            var Data = AhkamTasfeya.List(QueryID, AhkamSearchTypes.Advanced,out UsedCategories, "", "", null);
            return View(Data);
        }

        public IActionResult TasfyaSearch()
        {
            return View();
        }

         public JsonResult TasfyaListJson()
        {
            List<AhkamTasfeyaCategory> UsedCategories;
            var Data = AhkamTasfeya.List(56, AhkamSearchTypes.Advanced, out UsedCategories, "", "", null);
            var datajson = new[] {
                new object[] { "Data" , Data},
                new object[] { "Catg" , UsedCategories }
            };
            return new JsonResult(datajson);
          //  return new JsonResult(new { Data="Data", UsedCategories = "Catg" });
        }

        public JsonResult GetCountries()
        {
            var Countries = AhkamService.GetCountries();
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
    }

  
}
