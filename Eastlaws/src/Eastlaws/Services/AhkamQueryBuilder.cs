using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eastlaws;
using Eastlaws.Infrastructure;



namespace Eastlaws.Services
{
    public class AhkamQueryBuilder
    {
        public static string GeneralSearch(string FTSPredicate)
        {
            return "Select F.HokmID as ID , Sum(CT.[Rank]) as DefaultRank From  "
                +"\n" + " ContainsTable(AhkamFakarat, *, " + FTSPredicate + ") CT "
                + "\n" + " Join AhkamFakarat F on F.ID = CT.[Key] "
                +"\n" + " Group By F.HokmID ";
        }
        public static string CustomSearch(string FTSPredicate)
        {
            return "";
        }

        private static string GetFakraQueryForAdvancedSearch(FTSPredicate Predicate, string FakraNumCondition)
        {
            if (Predicate.IsValid)
            {
                return "Select HokmID as ID  , 0 as DefaultRank  From AhkamFakarat  A  "
                    + "\n" + "Where " + FakraNumCondition + " And Contains(A.Text , " + Predicate.BuildPredicate() + ") "
                    + "\n" + "Group By A.HokmID";
            }
            return null;
        }

        public static string AdvancedSearch(bool AllElementsCombined , FTSPredicate PredicateMabade2 , FTSPredicate PredicateWakae3 , FTSPredicate PredicateDestoreya 
            , FTSPredicate PredicateHay2a , FTSPredicate PredicateMantoo2 , FTSPredicate PredicateHaytheyat
            )
        {
    
        

            string strMabade2Query = GetFakraQueryForAdvancedSearch(PredicateMabade2, "FakraNo  >=  1");
            string strWakae3Query = GetFakraQueryForAdvancedSearch(PredicateMabade2, "FakraNo  =  -1");
            string strDestoreyaQuery = GetFakraQueryForAdvancedSearch(PredicateMabade2, "FakraNo  = -50 ");
            string strHay2aQuery = GetFakraQueryForAdvancedSearch(PredicateMabade2, "FakraNo  = 0");
            string strMantoo2Query = GetFakraQueryForAdvancedSearch(PredicateMabade2, "FakraNo  = -2");
            string strHaytheyatQuery = GetFakraQueryForAdvancedSearch(PredicateMabade2, "FakraNo = -3");

            string[] Queries = { strMabade2Query, strWakae3Query, strDestoreyaQuery, strHay2aQuery, strMantoo2Query, strHaytheyatQuery };
            int IncludedCount = 0;
            StringBuilder builder = new StringBuilder();
            string Operator = AllElementsCombined ? " Intersect " : " Union ";
            for (int i = 0; i < Queries.Length; i++)
            {
                if(!string.IsNullOrEmpty(Queries[i]))
                {
                    IncludedCount++;
                    builder.Append(Queries[i]);
                    builder.Append("\n");
                    builder.Append(Operator);
                }
            }
            if(IncludedCount == 0)
            {
                return null;
            }
            else
            {
                builder.Remove(builder.Length - Operator.Length, Operator.Length);
                
            }

            return builder.ToString();
        }

        public static string MadaSearch(int?  CountryID, string TashNo, string TashYear, string MadaNo , string TashTextFilter , bool SearchInTashTile = true , bool SearchInTashMawad = true)
        {

            return "";
        }

        public static string GetOuterQuery(string InnerQuery , int PageSize  = 10, int PageNo = 1 )
        {
            StringBuilder builder = new StringBuilder();
            // Creating the temp table and adding the current Page Data

            builder.AppendFormat("Declare @PageNo int ={0} , @PageSize int = {1}"  , PageNo , PageSize);
            builder.Append("\n");
            builder.Append("Declare @ResultsPage Table(ItemID int Primary Key  ,  SortValue Sql_Variant )");
            builder.Append("\n");
            builder.Append("Insert Into @ResultsPage ");
            builder.Append("\n");
            builder.Append("Select  ItemID , DefaultRank as SortValue From (");
            builder.Append("\n");
            builder.Append("Select Row_Number() Over (order By DefaultRank desc) as Serial , *  From ");
            builder.Append("\n");
            builder.Append("(");
            builder.Append("\n");
            builder.Append(InnerQuery);
            builder.Append(") ResultsTableInner  ");
            builder.Append("\n");
            builder.Append(") ResultsTableOuter ");
            builder.Append("\n");
            builder.Append("Where Serial > ((@PageNo - 1) * @PageSize) And Serial <= (@PageNo * @PageSize)");

            builder.Append("\n");

            // Ahkam Data  (Master Data )
            builder.Append("Select A.* From @ResultsPage RP  "
                + " join VW_Ahkam A WITH(NOLOCK)  On A.ID = RP.ItemID  "
                + "Order By RP.SortValue Desc ");




            return builder.ToString();
        }
    }
}
