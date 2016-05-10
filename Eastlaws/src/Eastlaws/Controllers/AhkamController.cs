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

        [HttpPost]
        public IActionResult SearchResultAssembly(AssemblySearch AssembleSearchInputs,int PageNo,int Sort,int pageSize,
                                                   bool Latest = false,int Days = 10, 
                                                   int typeView = 1,int SortDir = 1)
        {

            ViewBag.typeView = typeView;
            AhkamPresentation Model = new AhkamPresentation();
            AhkamSearchOptions Options = new AhkamSearchOptions { SortBy = (AhkamSortColumns)Sort, PageNo = PageNo, PageSize = pageSize, SortDirection = (SearchSortType)SortDir };

            if (Latest == true)
            {
                //Model = AhkamService.GetLatest(Options, Days);
                Model = AhkamService.GetLatestByDate(Options);
            }
            else
            {
                //AssemblySearch AssembleSearchInputs = new AssemblySearch();
                AhkamAdvancedSearch Obj = GetSearchObject(AssembleSearchInputs);
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

        public JsonResult TasfyaListJson(int QueryID)
        {
            List<AhkamTasfeyaCategory> UsedCategories;
            var Data = AhkamTasfeya.List(QueryID, AhkamSearchTypes.Advanced, out UsedCategories, "", "", null);
            var datajson = new[] {
                new object[] { "Data" , Data},
                new object[] { "Catg" , UsedCategories }
            };
            return new JsonResult(datajson);
          //  return new JsonResult(new { Data="Data", UsedCategories = "Catg" });
        }

        public JsonResult TasfeyaListJson(AssemblySearch AssemblySearchInputs , string TasfeyaSearch = "" )
        {
            AhkamAdvancedSearch Obj = GetSearchObject(AssemblySearchInputs);

            // needs to be sent as a param 
            AhkamSearchTypes SearchType = AhkamSearchTypes.Advanced;

            List<AhkamTasfeyaCategory> UsedCategories;
            var Data = AhkamTasfeya.List(Obj, SearchType, out UsedCategories, TasfeyaSearch, "", null);

            var datajson = new[] {
                new object[] { "Data" , Data},
                new object[] { "Catg" , UsedCategories }
            };
            return new JsonResult(datajson);


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


        //public IActionResult test1(string countrIDs,string mahkmaIDs ,string mana3y)
        public IActionResult test1(string IDS)
        {
            List<AhkamTasfeyaSelection> SelectedItems = new List<AhkamTasfeyaSelection>();
            //--- test1

            //if (countrIDs != "")
            //{
            //    string[] ids = ["1", "2", "3"];
            //    SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "1" });
            //    SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "2" });
            //    SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "3" });
            //}

            // test 2
            /*

            IDS =  {
                    { catgID:1,IDs:1,2,3},
                    { catgID:5,IDs:1,2,3}
                }
            eventfire = catg:1

            // pares IDs to list of SelectedItems
            */

            //test 3
            /*
             List<AhkamTasfeyaSelection> IDS 

                  IDS =  {
                        { CategoryID:1,Parameter:1},
                        { CategoryID:1,Parameter:2},
                        { CategoryID:1,Parameter:3},
                        { CategoryID:5,Parameter:1},
                        { CategoryID:5,Parameter:6},
                        { CategoryID:6,Parameter:10},
                }
            eventfire = catg:1

            */

            //-----
            return View();
        }


        public IActionResult TestBesada55()
        {
            List<AhkamTasfeyaSelection> SelectedItems = new List<AhkamTasfeyaSelection>();
            SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country , Parameter= "1" });
            SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "2" });
            SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Country, Parameter = "3" });



            SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Defoo3, Parameter = "1" });
            SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Defoo3, Parameter = "2" });


            SelectedItems.Add(new AhkamTasfeyaSelection() { CategoryID = AhkamTasfeyaCategoryIds.Mana3y, Parameter = "3" });


            string XXX = AhkamQueryBuilder.ResolveTasfeyaQuery(SelectedItems);

            return null;
        }
    }

  
}
