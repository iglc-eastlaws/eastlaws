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

        public IActionResult SearchResultAssembly(AssemblySearch Search,int PageNo,int Sort)
        {
            AhkamSearchOptions Options = new AhkamSearchOptions { SortBy = (AhkamSortColumns)Sort, PageNo = PageNo };
            AhkamAdvancedSearch Obj = new AhkamAdvancedSearch();
            Obj.PredicateAny = new FTSPredicate(Search.Alltext, (FTSSqlModes)Search.alltextSearchType);
            Obj.PredicateHay2a= new FTSPredicate(Search.hay2a, (FTSSqlModes)Search.hay2aSearchType);
            Obj.PredicateHaytheyat=new FTSPredicate(Search.hyseyt, (FTSSqlModes)Search.hyseytSearchType);
            Obj.PredicateMabade2 = new FTSPredicate(Search.Mabdaa, (FTSSqlModes)Search.MabdaaSearchType);
            Obj.PredicateWakae3 = new FTSPredicate(Search.waka23, (FTSSqlModes)Search.waka23SearchType);
            Obj.CaseDatefrom = Search.dateGalsaFrom;
            Obj.CaseDateTo = Search.dateGalsaTo;
            Obj.CaseYear = Search.caseYear;
            Obj.CaseNo = Search.caseNo;
            Obj.CountryIDs = Search.country;
            Obj.IFAgree = Search.MahkamaReplay;
            Obj.Ma7akemIds = Search.mahakem;
            Obj.OfficeSuffix = Search.omarGroup;
            Obj.OfficeYear = Search.officeYear;
            Obj.PageNo = Search.pageNo;
            Obj.PartNo = Search.partNo;


            AhkamPresentation Model = AhkamService.Search(Options, Obj);
                if (Model.IsValid)
                {
                    return View(Model);
                }
                else
                {
                    return View();
                }
           
        }

        public IActionResult Latest(int Days = 10)
        {
            AhkamSearchOptions Options = new AhkamSearchOptions { PageNo = 1 , SortBy = AhkamSortColumns.DateAdded , SortDirection = SearchSortType.DESC };
            AhkamPresentation Model = AhkamService.GetLatest(Options, Days);
            return View("SearchResult", Model);
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
