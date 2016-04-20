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
        public static string GeneralSearch(FTSPredicate Predicate)
        {
            if (Predicate.IsValid)
            {
                return @"Select F.HokmID as ID , Sum(CT.[Rank]) as DefaultRank From  
                   ContainsTable(AhkamFakarat, *, " + Predicate.BuildPredicate() + @") CT 
                   Join AhkamFakarat F on F.ID = CT.[Key] 
                   Group By F.HokmID ";
            }
            return null;
        }

        public static string AdvancedCustomSearch(AhkamAdvancedSearch srchObj)
        {
            string[] Conditions = {
                    new Range(srchObj.CountryIDs, "A.CountryID").GetCondition()
                    ,new Range(srchObj.Ma7akemIds, "A.MahkamaID ").GetCondition()
                    ,new Range(srchObj.OfficeYear, "A.OfficeYear").GetCondition()
                    ,new Range(srchObj.CaseNo, "A.CaseNo").GetCondition()
                    ,new Range(srchObj.CaseYear, "A.CaseYear").GetCondition()
                    ,new Range(srchObj.PartNo, "A.PartNo").GetCondition()
                    ,new Range(srchObj.PageNo, "A.PageNo").GetCondition()
                    ,new Range(srchObj.IFAgree, "A.IfAgree").GetCondition()
                    ,new Range(srchObj.OfficeSuffix, "A.OfficeSuffix").GetCondition()
            };

            StringBuilder Builder = new StringBuilder();
            Builder.Append(@"Select A.ID as ID  ,  0 as DefaultRank From Ahkam A Where (1 = 1)");

            int ConditionsCount = 0;
            for (int i = 0; i < Conditions.Length; i++)
            {
                string CurrentCondition = Conditions[i];
                if (!string.IsNullOrWhiteSpace(CurrentCondition))
                {
                    Builder.Append("\n" + " And " + CurrentCondition);
                    ConditionsCount++;
                }
            }
            DateTime dtCaseDateFrom, dtCaseDateTo;
            if (DateTime.TryParseExact(srchObj.CaseDatefrom, DataHelpers.ClientDateFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtCaseDateFrom))
            {
                Builder.Append("\n And A.CaseDate >= " + "'" + dtCaseDateFrom.ToString("yyyy-MM-dd") + "'");
                ConditionsCount++;
            }
            if (DateTime.TryParseExact(srchObj.CaseDateTo, DataHelpers.ClientDateFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtCaseDateTo))
            {
                Builder.Append("\n And A.dtCaseDateTo >= " + "'" + dtCaseDateFrom.ToString("yyyy-MM-dd") + "'");
                ConditionsCount++;
            }

     

            string[] Queries = {
                      GetFakraQueryForAdvancedSearch(srchObj.PredicateMabade2, "FakraNo  >=  1")
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateWakae3, "FakraNo  =  -1")
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateDestoreya, "FakraNo  = -50 ")
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateHay2a, "FakraNo  = 0")
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateMantoo2, "FakraNo  = -2")
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateHaytheyat, "FakraNo = -3")
            };



            /*
            // Adding Text Search From Fakarat Table 
            if (PredicateFakarat != null && PredicateFakarat.IsValid)
            {
                string FakaratAndClause = "";
                if (!string.IsNullOrWhiteSpace(FakaratCondition))
                {

                }
                Builder.AppendFormat(@"
                                Intersect
                                Select HokmID as ID , 0 as DefaultRank From AhkamFakarat
                                Where Contains(* , {0}) {1}
                                Group By HokmID "
                , PredicateFakarat.BuildPredicate(), FakaratAndClause);
                ConditionsCount++;
            }
            */
            return Builder.ToString();     
        }


        public static string LatestAhkam(int daysCount)
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

        public static string AdvancedSearch(bool AllElementsCombined , FTSPredicate PredicateMabade2 , FTSPredicate PredicateWakae3
            , FTSPredicate PredicateDestoreya , FTSPredicate PredicateHay2a , FTSPredicate PredicateMantoo2 , FTSPredicate PredicateHaytheyat)
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

        public static string MadaSearch(int?  CountryID, string TashNo, string TashYear, string MadaNo , FTSPredicate TashTextFilter , bool SearchInTashTile = true , bool SearchInTashMawad = true)
        {
            return "";
        }

        public static string GetOuterQuery(string InnerQuery , AhkamSearchOptions Options, string CustomFakaratQuery = null)
        {
            StringBuilder builder = new StringBuilder();
            // Creating the temp table and adding the current Page Data
            builder.AppendFormat(@"Set NoCount on ;
                                   Declare @PageNo int ={0} , @PageSize int = {1} 
                                   Declare @ResultsPage Table(ItemID int Primary Key  ,  SortValue Sql_Variant )
                                   Insert Into @ResultsPage 
                                   Select  ItemID , DefaultRank as SortValue From 
                                   (
                                       Select Row_Number() Over (order By DefaultRank desc) as Serial , *  From 
                                       (
                                            {2}
                                       ) ResultsTableInner  
                                   ) ResultsTableOuter 
                                   Where Serial > ((@PageNo - 1) * @PageSize) And Serial <= (@PageNo * @PageSize)"
        , Options.PageNo, Options.PageSize , InnerQuery); 


            builder.Append("\n");

            // Ahkam Data  (Master Data )
            builder.Append("Select A.* From @ResultsPage RP  "
                + " join VW_Ahkam A WITH(NOLOCK)  On A.ID = RP.ItemID  "
                + " Order By RP.SortValue Desc ");

            if(Options.DisplayMode == AhkamDisplayMode.Divs)
            {
                if (!string.IsNullOrEmpty(CustomFakaratQuery))
                {
                    builder.Append(CustomFakaratQuery);
                }
                else
                {
                    // Temp 
                    builder.Append(@"Select AF.* From VW_AhkamFakarat AF Join @ResultsPage RP  on AF.HokmID = RP.ItemID Where AF.FakraNo = 1 ");
                }
            }
            return builder.ToString();
        }


        public static string GetSingleHokm(int ID )
        {
            return string.Format(@"Select A.* From VW_Ahkam A Where A.ID = {0}
                                    Select AF.* From VW_AhkamFakarat AF Where AF.HokmID = {0} Order By AF.MyOrder ", ID);
        }

    }
}
