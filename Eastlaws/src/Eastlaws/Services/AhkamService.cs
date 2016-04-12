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
        public static AhkamPresentation Search(AhkamSearchOptions Options , FTSPredicate SearchPredicate)
        {
            AhkamPresentation P = new AhkamPresentation();
            string InnerQuery = AhkamQueryBuilder.GeneralSearch(SearchPredicate);
            if(string.IsNullOrEmpty(InnerQuery))
            {
                P.IsValid = false;
                return P;
            }

            QueryCacher Cacher = new QueryCacher((int)LegalServices.Ahkam , InnerQuery , AhkamSearchTypes.General.ToString() , NewSearch:  true);
            string CachedQuery = Cacher.GetCachedQuery();
            P.ResultsCount = Cacher.ResultsCount;


            string OuterQuery = AhkamQueryBuilder.GetOuterQuery(CachedQuery, Options.PageNo, Options.PageSize, Options.SortBy, Options.SortDirections , null, true);

            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Grid = con.QueryMultiple(OuterQuery);
            P.AhkamList = Grid.Read<VW_Ahkam>();
            if(Options.DisplayMode == AhkamDisplayMode.Divs)
            {
                P.FakaratList = Grid.Read<VW_AhkamFakarat>();
            }


            return P;
        }

    }
}
