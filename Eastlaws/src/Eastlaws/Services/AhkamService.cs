﻿using System;
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

        // Advanced Search 
        public static AhkamPresentation Search(AhkamSearchOptions Options , bool AllElementsCombined, FTSPredicate PredicateMabade2, FTSPredicate PredicateWakae3
            , FTSPredicate PredicateDestoreya, FTSPredicate PredicateHay2a, FTSPredicate PredicateMantoo2, FTSPredicate PredicateHaytheyat)
        {

            string InnerQuery = AhkamQueryBuilder.AdvancedSearch(AllElementsCombined, PredicateMabade2, PredicateWakae3
            , PredicateDestoreya, PredicateHay2a, PredicateMantoo2, PredicateHaytheyat);

            return Search(InnerQuery, Options, null, AhkamSearchTypes.Advanced);
        }

        //Custom Search 
        public static AhkamPresentation Search(AhkamSearchOptions Options , FTSPredicate PredicateFakarat, string FakaratCondition, string CountryIDs, string Ma7akemIds, string CaseNo, string CaseYear, string PartNo
            , string PageNo, string OfficeYear, string IFAgree, string OfficeSuffix, string CaseDatefrom, string CaseDateTo)
        {
            string InnerQuery = AhkamQueryBuilder.CustomSearch(PredicateFakarat, FakaratCondition, CountryIDs, Ma7akemIds, CaseNo, CaseYear, PartNo
           , PageNo, OfficeYear, IFAgree, OfficeSuffix, CaseDatefrom, CaseDateTo);

            return Search(InnerQuery, Options, null, AhkamSearchTypes.Custom);
        }

        // Mada Search
        public static AhkamPresentation Search(AhkamSearchOptions Options , int? CountryID, string TashNo, string TashYear, string MadaNo, FTSPredicate TashTextPredicate, bool SearchInTashTile = true, bool SearchInTashMawad = true)
        {
            string InnerQuery = AhkamQueryBuilder.MadaSearch(CountryID, TashNo, TashYear, MadaNo, TashTextPredicate, SearchInTashTile, SearchInTashMawad);
            return Search(InnerQuery, Options, null, AhkamSearchTypes.Mada);
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
            return P;
        }

    }
}
