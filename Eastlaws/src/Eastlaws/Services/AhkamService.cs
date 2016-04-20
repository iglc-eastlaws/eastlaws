using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eastlaws.Entities;
using Eastlaws.Infrastructure;
using Eastlaws;
using Dapper;
using System.Data.SqlClient;

namespace Eastlaws.Services
{
    public enum LegalServices
    {
        Ahkam = 1  , Tashree3at = 2 , FatwaMaglesDawla = 3 , Etefa2eyat = 4
    }
    public enum AhkamSortColumns
    {
        Default = 0 ,CaseNo = 1 , CaseYear = 2 , CaseDate = 3 , TashCount = 4 
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
        General , Custom , Advanced , Mada 
    }

    public class AhkamSearchOptions
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public AhkamSortColumns SortBy { get; set; } = AhkamSortColumns.Default;
        public SearchSortType SortDirections { get; set; } = SearchSortType.DESC;
        public AhkamDisplayMode DisplayMode { get; set; } = AhkamDisplayMode.Divs;
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
        public static AhkamPresentation Search(AhkamSearchOptions Options  , AhkamAdvancedSearch SrchObj)
        {
            string FakaratQueryCustom;
            string InnerQuery = AhkamQueryBuilder.AdvancedCustomSearch(SrchObj , out FakaratQueryCustom);
            return Search(InnerQuery, Options, FakaratQueryCustom, AhkamSearchTypes.Advanced);
        }


        public static AhkamPresentation Search(AhkamSearchOptions Options , int CachedQueryID)
        {
            throw new NotImplementedException();
        }


        // Mada Search
        public static AhkamPresentation Search(AhkamSearchOptions Options , int? CountryID, string TashNo, string TashYear, string MadaNo, FTSPredicate TashTextPredicate, bool SearchInTashTile = true, bool SearchInTashMawad = true)
        {
            string InnerQuery = AhkamQueryBuilder.MadaSearch(CountryID, TashNo, TashYear, MadaNo, TashTextPredicate, SearchInTashTile, SearchInTashMawad);
            return Search(InnerQuery, Options, null, AhkamSearchTypes.Mada);
        }


        public static AhkamPresentation GetLatest(AhkamSearchOptions Options , int DaysCount = 3)
        {
            string InnerQuery = AhkamQueryBuilder.LatestAhkam(DaysCount);
            return Search(InnerQuery, Options, null, AhkamSearchTypes.Custom);
        }
       
        public static AhkamPresentation GetHokm(int ID , FTSPredicate PredicateHighlight )
        {
            AhkamPresentation P = new AhkamPresentation();
            string Query = AhkamQueryBuilder.GetSingleHokm(ID);
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Grid = con.QueryMultiple(Query);
            P.AhkamList = Grid.Read<VW_Ahkam>();
            P.FakaratList = Grid.Read<VW_AhkamFakarat>();
            if(P.AhkamList.Count() == 0)
            {
                P.IsValid = false;
            }
            return P;
        }



        // Internal Search Assembler !
        private static AhkamPresentation Search(string InnerQuery, AhkamSearchOptions Options , string CustomFakaratQuery , AhkamSearchTypes SearchType )
        {
            AhkamPresentation P = new AhkamPresentation();
            if (string.IsNullOrEmpty(InnerQuery))
            {
                P.IsValid = false;
                return P;
            }

            QueryCacher Cacher = new QueryCacher((int)LegalServices.Ahkam, InnerQuery, SearchType.ToString(), NewSearch: true);
            string CachedQuery = Cacher.GetCachedQuery();
            P.ResultsCount = Cacher.ResultsCount;

            string OuterQuery = AhkamQueryBuilder.GetOuterQuery(CachedQuery, Options, CustomFakaratQuery);
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Grid = con.QueryMultiple(OuterQuery);
            P.AhkamList = Grid.Read<VW_Ahkam>();
            if (Options.DisplayMode == AhkamDisplayMode.Divs)
            {
                P.FakaratList = Grid.Read<VW_AhkamFakarat>();
            }
            P.QueryInfo = Cacher.Info;
            return P;
        }


        

    }
}
