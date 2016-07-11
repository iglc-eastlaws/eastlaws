using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eastlaws.Entities;
using Eastlaws.Infrastructure;
using Eastlaws;
using Dapper;
using System.Data.SqlClient;
using System.Text;

namespace Eastlaws.Services
{
    public enum LegalServices
    {
        Ahkam = 1  , Tashree3at = 2 , FatwaMaglesDawla = 3 , Etefa2eyat = 4
    }
    public enum AhkamSortColumns
    {
        Default = 0 ,CaseNo = 1 , CaseYear = 2 , CaseDate = 3 , TashCount = 4 , DateAdded = 5 
    }
    public enum AhkamDisplayMode
    {
        Divs = 0 , Table = 1
    }
    public enum SearchSortType
    {
        ASC = 0 , DESC = 1
    }
    public enum AhkamSearchTypes

    {
        General , Custom , Advanced , Mada,
        Fehres
    }


    public class AhkamSearchOptions
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public AhkamSortColumns SortBy { get; set; } = AhkamSortColumns.Default;
        public SearchSortType SortDirection { get; set; } = SearchSortType.DESC;
        public AhkamDisplayMode DisplayMode { get; set; } = AhkamDisplayMode.Divs;
        public List<AhkamTasfeyaSelection> TasfeyaSelection { get; set; }     
    }

    public class AhkamService
    {
        // General Search 
        public static AhkamPresentation Search(AhkamSearchOptions Options , FTSPredicate SearchPredicate)
        {
            string InnerQuery = AhkamQueryBuilder.GeneralSearch(SearchPredicate);
            return Search(InnerQuery, Options, null, AhkamSearchTypes.General);
        }

        /// <summary>
        /// Adcanced + Custom Search Combined Together
        /// </summary>
        /// <param name="Options">Search Options </param>
        /// <param name="SrchObj">Search Parameters (Filters ) </param>
        /// <returns></returns>
        public static AhkamPresentation Search(AhkamSearchOptions Options  , AhkamAdvancedSearch SrchObj, List<AhkamTasfeyaSelection> TasfeyaSelection = null)
        {
            string FakaratQueryCustom;
            List<FTSPredicate> SearchPredicates; 
            string InnerQuery = AhkamQueryBuilder.AdvancedCustomSearch(SrchObj , out FakaratQueryCustom , out SearchPredicates);
            return Search(InnerQuery, Options, FakaratQueryCustom, AhkamSearchTypes.Advanced , null , SearchPredicates, TasfeyaSelection);
        }
       

        // Mada Search
        public static AhkamPresentation Search(AhkamSearchOptions Options, AhkamMadaSearch SrchObj, List<AhkamTasfeyaSelection> TasfeyaSelection = null)
        {
            string FakaratQueryCustom;
            List<FTSPredicate> SearchPredicates;
            string InnerQuery =  AhkamQueryBuilder.MadaSearch(SrchObj, out FakaratQueryCustom, out SearchPredicates);
            return Search(InnerQuery, Options, FakaratQueryCustom, AhkamSearchTypes.Mada, null, SearchPredicates, TasfeyaSelection);            
        }

        //Fehres Search 
        public static AhkamPresentation Search(AhkamSearchOptions Options , int FehresItemID , List<AhkamTasfeyaSelection> TasfeyaSelection = null)
        {
            string FakaratQueryCustom;
            List<FTSPredicate> SearchPredicates;
            string InnerQuery = AhkamQueryBuilder.FehresSearch(FehresItemID, out FakaratQueryCustom, out SearchPredicates);
            return Search(InnerQuery, Options, FakaratQueryCustom, AhkamSearchTypes.Fehres, null, SearchPredicates, TasfeyaSelection);
        }


        public static AhkamPresentation GetLatest(AhkamSearchOptions Options , int DaysCount = 10)
        {
            string InnerQuery = AhkamQueryBuilder.LatestAhkam(DaysCount);
            return Search(InnerQuery, Options, null, AhkamSearchTypes.Custom, "أُضـــيـــف حديــــثــــاً");
        }


        public static AhkamPresentation GetLatestByDate(AhkamSearchOptions Options, List<AhkamTasfeyaSelection> TasfeyaSelection = null)
        {
            string InnerQuery = AhkamQueryBuilder.LatestAhkamByDate();
            //Options.SortBy = AhkamSortColumns.CaseDate;
            //Options.SortDirection = SearchSortType.DESC;
          
            return Search(InnerQuery, Options, null, AhkamSearchTypes.Custom, "أحدث الأحكام",null,TasfeyaSelection);  
        }

        public static AhkamPresentation GetHokm(int ID , FTSPredicate PredicateHighlight )
        {
            AhkamPresentation P = new AhkamPresentation();
            string Query = AhkamQueryBuilder.GetSingleHokm(ID);
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Grid = con.QueryMultiple(Query);
            P.AhkamList = Grid.Read<VW_Ahkam>();
            P.FakaratList = Grid.Read<VW_AhkamFakarat>();
            P.FaharesList = Grid.Read<FehresItem>();
            P.FaharesLinks = Grid.Read<FehresLink>();

            if (P.AhkamList.Count() == 0)
            {
                P.IsValid = false;
            }
            return P;
        }

        // Internal Search Assembler !
        private static AhkamPresentation Search(string InnerQuery, AhkamSearchOptions Options , string CustomFakaratQuery , AhkamSearchTypes SearchType , 
            string CustomPresentationTitle  = null , List<FTSPredicate> Predicates = null,  List<AhkamTasfeyaSelection> TasfeyaSelection = null)
        {
            AhkamPresentation P = new AhkamPresentation();
            if (string.IsNullOrEmpty(InnerQuery))
            {
                P.IsValid = false;
                return P;
            }

            QueryCacher Cacher = new QueryCacher((int)LegalServices.Ahkam, InnerQuery, SearchType.ToString(), NewSearch: true , SecondaryQuery : CustomFakaratQuery);
            string CachedQuery = Cacher.GetCachedQuery();
            P.ResultsCount = Cacher.ResultsCount;

            string OuterQuery = AhkamQueryBuilder.GetOuterQuery(Cacher, Options, CustomFakaratQuery, TasfeyaSelection);
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Grid = con.QueryMultiple(OuterQuery);
            P.AhkamList = Grid.Read<VW_Ahkam>();
            if (Options.DisplayMode == AhkamDisplayMode.Divs)
            {
                P.FakaratList = Grid.Read<VW_AhkamFakarat>();
            }
            P.QueryInfo = Cacher.Info;
            P.PresentationTitle = CustomPresentationTitle;
            P.TextPredicates = Predicates;

            if(TasfeyaSelection != null && TasfeyaSelection.Count > 0)
            {
                string CountQuery = AhkamQueryBuilder.ResolveTasfeyaQuery(TasfeyaSelection ,
                    @"SELECT Count(*) FROM EastlawsUsers..QueryCacheRecords QCR With (Nolock)
                    Join Ahkam AMS  With (Nolock) on AMS.ID = QCR.ItemID
                    WHERE QCR.MasterID = " + Cacher.ID);
                int NewCount = 0;
                using(var conCount = DataHelpers.GetConnection(DbConnections.Data))
                {
                    NewCount = conCount.QuerySingle<int>(CountQuery);
                    P.ResultsCount = NewCount;
                }
            }
            return P;
        }

        public static IEnumerable<Country> GetCountries()
        {
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Data = con.Query<Country>("GetServiceCountries", new { ServiceID = 1 }, null, true, null, System.Data.CommandType.StoredProcedure);
            return Data;
        }
    
        public static IEnumerable<AhkamGehaType> GetGehaType()
        {
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Data = con.Query<AhkamGehaType>("GetAhkamGehaType", null, null, true, null, System.Data.CommandType.StoredProcedure);
            return Data;
        }

        public static IEnumerable<AhkamMahakem> GetMahakem(int CountryID)
        {
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Data = con.Query<AhkamMahakem>("GetMahakem", new { CountryID = CountryID }, null, true, null, System.Data.CommandType.StoredProcedure);
            return Data;
        }

        public static IEnumerable<FehresItem> GetMahkamaReplies()
        {
            List<FehresItem> Items = new List<FehresItem>();
            Items.Add(new FehresItem { ID = 732403, Name = "غير محدد" });
            Items.Add(new FehresItem { ID = 732398, Name = "رفض الطعن" });
            Items.Add(new FehresItem { ID = 732399, Name = "قبول الطعن" });
            Items.Add(new FehresItem { ID = 732401, Name = "عدم قبول الطعن" });
            Items.Add(new FehresItem { ID = 732405, Name = "عدم جواز الطعن" });
            Items.Add(new FehresItem { ID = 732400, Name = "عدم إختصاص" });
            Items.Add(new FehresItem { ID = 732404, Name = "إنتهاء الخصومة" });
            Items.Add(new FehresItem { ID = 732402, Name = "إثبات ترك الخصومة" });
            Items.Add(new FehresItem { ID = 732418, Name = "إنقطاع سير الخصومة" });
            Items.Add(new FehresItem { ID = 801052, Name = "إنقضاء الدعوى الجنائية" });
            Items.Add(new FehresItem { ID = 801560, Name = "بطلان الطعن" });
            Items.Add(new FehresItem { ID = 801703, Name = "انقضاء الخصومة في الطعن" });
            Items.Add(new FehresItem { ID = 802378, Name = "سقوط الطعن" });
            Items.Add(new FehresItem { ID = 805255, Name = "عدم قبول الطلب" });
            Items.Add(new FehresItem { ID = 805256, Name = "قبول الطلب" });
            Items.Add(new FehresItem { ID = 805260, Name = "مُرجح" });
            Items.Add(new FehresItem { ID = 806259, Name = "إقرار الطلب" });
            Items.Add(new FehresItem { ID = 819456, Name = "عدم القبول شكلاً" });
            Items.Add(new FehresItem { ID = 819457, Name = "عدم القبول موضوعاً" });
            Items.Add(new FehresItem { ID = 819749, Name = "النقض والإحالة للدعوى الجنائية" });
            Items.Add(new FehresItem { ID = 819750, Name = "النقض والتصحيح  للدعوى الجنائية" });
            Items.Add(new FehresItem { ID = 819752, Name = "اعتبار الطعن المقدم من النيابة طلباً" });
            Items.Add(new FehresItem { ID = 819753, Name = "النقض والإحالة للدعوى المدنية" });
            Items.Add(new FehresItem { ID = 825308, Name = "النقض والتصحيح للدعوى المدنية" });
            Items.Add(new FehresItem { ID = 819755, Name = "إنقضاء الدعوى المدنية" });
            Items.Add(new FehresItem { ID = 819756, Name = "إثبات ترك الدعوى المدنية" });
            Items.Add(new FehresItem { ID = 825305, Name = "تصحيح الحكم" });
            Items.Add(new FehresItem { ID = 825311, Name = "النقض للدعويين والتصحيح" });
            Items.Add(new FehresItem { ID = 825312, Name = "النقض للدعويين والإحالة" });
            Items.Add(new FehresItem { ID = 831289, Name = "النقض وتحديد جلسة موضوعية" });
            Items.Add(new FehresItem { ID = 831290, Name = "نقض الحكم وعدم إختصاص المحاكم المصرية" });
            Items.Add(new FehresItem { ID = 831291, Name = "إعادة المحاكمة" });
            Items.Add(new FehresItem { ID = 831292, Name = "نقض الأمر والإحالة" });
            Items.Add(new FehresItem { ID = 831293, Name = "إقرار الحكم بالإعدام" });
            Items.Add(new FehresItem { ID = 831294, Name = "نقض الحكم وتعيين المحكمة المختصة" });
            Items.Add(new FehresItem { ID = 831295, Name = "نقض الحكم والإحالة" });
            Items.Add(new FehresItem { ID = 831296, Name = "نقض الحكم والتصدي للموضوع" });
            Items.Add(new FehresItem { ID = 831297, Name = "نقض الحكم دون الإحالة أو التصدي" });
            Items.Add(new FehresItem { ID = 831298, Name = "سقوط الحق في الطعن" });
            Items.Add(new FehresItem { ID = 831299, Name = "سقوط الخصومة" });
            Items.Add(new FehresItem { ID = 831457, Name = "إلغاء القرار المطعون فيه" });
            Items.Add(new FehresItem { ID = 831458, Name = "تعديل القرار المطعون فيه" });
            Items.Add(new FehresItem { ID = 833722, Name = "إلغاء الحكم المطعون فيه" });
            Items.Add(new FehresItem { ID = 833723, Name = "تعديل الحكم المطعون فيه" });
            Items.Add(new FehresItem { ID = 835016, Name = "بطلان الحكم المطعون فيه" });
            Items.Add(new FehresItem { ID = 835017, Name = "وقف تنفيذ القرار المطعون فيه" });
            Items.Add(new FehresItem { ID = 835018, Name = "وقف الفصل في الطعن" });
            Items.Add(new FehresItem { ID = 844649, Name = "رفض الطلب" });
            Items.Add(new FehresItem { ID = 874454, Name = "عدم جواز الطلب" });
            Items.Add(new FehresItem { ID = 898247, Name = "إعادة الدعوى للمرافعة" });

            return Items;
        }
    }
}
