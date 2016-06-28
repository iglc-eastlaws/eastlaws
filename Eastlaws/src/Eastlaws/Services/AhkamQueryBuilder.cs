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

        public static string AdvancedCustomSearch(AhkamAdvancedSearch srchObj , out string OuterFakartQuery , out List<FTSPredicate> TextMatches )
        {
            TextMatches = srchObj.GetValidPredicates();

            // this query is executed later , Ref : {AhkamQueryBuilder.GetOuterQuery()}
            OuterFakartQuery = @"Select AF.* , dbo.GetAhkamFakraTitle(AF.FakraNo) as Title  ,dbo.GetAhkamFakraOrder(AF.FakraNo) as MyOrder  "
                    + "\n" + "From AhkamFakarat AF Join @ResultsPage RP  on AF.HokmID = RP.ItemID ";
            bool FakaratQueryWhereClauseAdded = false;

            string[] Conditions = {
                    new Range(srchObj.CountryIDs, "A.CountryID").GetCondition()
                    ,new Range(srchObj.Ma7akemIds, "A.MahkamaID ").GetCondition()
                    ,new Range(srchObj.OfficeYear, "A.OfficeYear").GetCondition()
                    ,new Range(srchObj.CaseNo, "A.CaseNo").GetCondition()
                    ,new Range(srchObj.CaseYear, "A.CaseYear").GetCondition()
                    ,new Range(srchObj.PartNo, "A.PartNo").GetCondition()
                    ,new Range(srchObj.PageNo, "A.PageNo").GetCondition()
                    ,new Range(srchObj.IFAgree, "A.IfAgree").GetCondition()
                  //  ,new Range(srchObj.OfficeSuffix, "A.OfficeSuffix").GetCondition()   // ommar group not in range condation
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


            //add ommar group condation
            if ((!string.IsNullOrWhiteSpace(srchObj.OfficeSuffix)) && (srchObj.OfficeSuffix.Trim() == "ع"))
            {
                Builder.Append("\n And A.OfficeSuffix = 'ع' ");
                ConditionsCount++;
            }
       

            DateTime dtCaseDateFrom, dtCaseDateTo;
            if (DateTime.TryParseExact(srchObj.CaseDatefrom, DataHelpers.ClientDateFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtCaseDateFrom))
            {
                Builder.Append("\n And A.CaseDate >= " + "'" + dtCaseDateFrom.ToString("yyyy-MM-dd") + "'");
                ConditionsCount++;
            }
            if (DateTime.TryParseExact(srchObj.CaseDateTo, DataHelpers.ClientDateFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtCaseDateTo))
            {
                Builder.Append("\n And A.CaseDate <= " + "'" + dtCaseDateTo.ToString("yyyy-MM-dd") + "'");
                ConditionsCount++;
            }




            string strOuterFakraMabade2  , strOuterFakraWakae3, strOuterFakraDestoreya, strOuterFakraHay2a, strOuterFakraMantoo2, strOuterFakraHaytheyat;

            string[] Queries = {
                      GetFakraQueryForAdvancedSearch(srchObj.PredicateMabade2, "A.FakraNo  >=  1", out strOuterFakraMabade2)
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateWakae3, "A.FakraNo  =  -1", out strOuterFakraWakae3)
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateDestoreya, "A.FakraNo  = -50 " , out strOuterFakraDestoreya)
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateHay2a, "A.FakraNo  = 0" , out strOuterFakraHay2a)
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateMantoo2, "A.FakraNo  = -3" , out strOuterFakraMantoo2)
                    , GetFakraQueryForAdvancedSearch(srchObj.PredicateHaytheyat, "A.FakraNo = -2" , out strOuterFakraHaytheyat)
            };

            string[] OuterFakraConditions = {
                strOuterFakraMabade2  , strOuterFakraWakae3, strOuterFakraDestoreya, strOuterFakraHay2a, strOuterFakraMantoo2, strOuterFakraHaytheyat
            };




            // Fakarat Text Search 
            bool AllElementsCombined = true;    // Hardcoded for the moment 
            StringBuilder BuilderText = new StringBuilder();
            string Operator = AllElementsCombined ? " Intersect " : " Union ";
            int TextQueriesCount = 0;
            for (int i = 0; i < Queries.Length; i++)
            {
                if (!string.IsNullOrEmpty(Queries[i]))
                {                   
                    TextQueriesCount++;
                    BuilderText.Append("\n");
                    BuilderText.Append(Queries[i]);
                    BuilderText.Append("\n");
                    BuilderText.Append(Operator);

                    if (FakaratQueryWhereClauseAdded)
                    {
                        OuterFakartQuery += " OR " + OuterFakraConditions[i];
                    }
                    else
                    {
                        OuterFakartQuery += " Where " + OuterFakraConditions[i];
                        FakaratQueryWhereClauseAdded = true;
                    }

                }
            }
            if(TextQueriesCount > 0)
            {
                BuilderText.Remove(BuilderText.Length - Operator.Length, Operator.Length);
            }


            string GeneralSearchQuery = GeneralSearch(srchObj.PredicateAny);
            string TextQuery = BuilderText.ToString();
            string AhkamMasterQuery = Builder.ToString();


            // Adjusting Outer Fakarat Query 
            if(!string.IsNullOrEmpty(GeneralSearchQuery))
            {
                if (FakaratQueryWhereClauseAdded)
                {
                    OuterFakartQuery += " OR  ( Contains(AF.Text , " + srchObj.PredicateAny.BuildPredicate() + ") ) ";
                }
                else
                {
                    OuterFakartQuery += " Where  " + " Contains(AF.Text , " +  srchObj.PredicateAny.BuildPredicate() + ") ";
                    FakaratQueryWhereClauseAdded = true;
                }
            }


            // if no where clause is specified use the default query that comes in the {AhkamQueryBuilder.GetOuterQuery()}
            if (!FakaratQueryWhereClauseAdded)
            {
                OuterFakartQuery = null;
            }
            else // else Add the Correct Order By clause 
            {
                OuterFakartQuery += "\n Order By AF.HokmID ,  MyOrder";
            }

            Dictionary<string, string> FinalQueries = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(GeneralSearchQuery))
                FinalQueries.Add("GSQ", GeneralSearchQuery);

            if (!string.IsNullOrEmpty(TextQuery))
                FinalQueries.Add("FTQ", TextQuery);

            if (!string.IsNullOrEmpty(AhkamMasterQuery))
                FinalQueries.Add("AMQ", AhkamMasterQuery);



            StringBuilder builderRetVal = new StringBuilder();
            
            builderRetVal.Append("Select * From ");
            string LastQueryKey = "";
           // int FinalQueriesIterator = 0;
            foreach (var item in FinalQueries)
            {

                if(LastQueryKey == "")
                {
                    // Select * From (xx)  => Select GSQ.ID , GSQ.DefaultRank          :( 
                    builderRetVal.Replace("*", item.Key + ".ID" + " , " + item.Key + ".DefaultRank");
                }

                if(LastQueryKey != "")
                {
                    builderRetVal.Append(" Join ");
                }

                builderRetVal.Append("\n ( " + item.Value + " ) " + item.Key);

                if(LastQueryKey != "")
                {
                    builderRetVal.AppendFormat(" On {0}.ID = {1}.ID " , LastQueryKey, item.Key );
                }
                LastQueryKey = item.Key;
                
            }
     
            return builderRetVal.ToString();            
        }


        public static string LatestAhkam(int daysCount = 5)
        {
            return "Select A.ID , 0 as DefaultRank From Ahkam A Where A.DateAdded >= DateAdd(Day , -" + daysCount + " ,  (Select Max(DateAdded) From Ahkam ))";
    
        }

        public static string LatestAhkamByDate()
        {
            return "Select top 500 A.ID , 0 as DefaultRank From Ahkam A Where CountryID != 9 order by A.CaseDate DESC";

        }

        private static string GetFakraQueryForAdvancedSearch(FTSPredicate Predicate, string FakraNumCondition , out string FakraWhereClause)
        {
            FakraWhereClause = null;
            if (Predicate.IsValid)
            {
                FakraWhereClause = "(" +  FakraNumCondition.Replace("A." , "AF.") + " And Contains(AF.Text , "  + Predicate.BuildPredicate() +") )";
                return "Select HokmID as ID  , 0 as DefaultRank  From AhkamFakarat  A  "
                    + "\n" + "Where " + FakraNumCondition + " And Contains(A.Text , " + Predicate.BuildPredicate() + ") "
                    + "\n" + "Group By A.HokmID";
            }
            return null;
        }


        private static string TashToAhkam(string TashQuery,bool MadaNoCondition = false)
        {
            if (!MadaNoCondition)
            {
                return "SELECT TA.HokmID AS ID ,Sum(TQ.DefaultRank) AS DefaultRank  FROM  "
                    + "\n" + "( " + TashQuery + " ) TQ"
                    + "\n" + "JOIN dbo.Tashree3atAhkam TA  ON ta.TashID = TQ.ID "
                    + "\n" + "GROUP BY TA.HokmID ";
            }
            else
            {
                // result relate to mada  only
                return "SELECT TA.HokmID AS ID ,Sum(TQ.DefaultRank) AS DefaultRank  FROM  "
                + "\n" + "( " + TashQuery + " ) TQ"
                + "\n" + "JOIN dbo.Tashree3atAhkam TA  ON TA.MadaID= TQ.madaID  "
                + "\n" + "GROUP BY TA.HokmID ";

            }
           }

        public static string MadaSearch(AhkamMadaSearch ObjSearch , out string FakaratQuery , out List<FTSPredicate> TextMatches)
        {
            TextMatches = null;
            FakaratQuery = null;
            bool isMadaNoCondition = false; // result relate to mada  only


            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("SELECT T.ID , 0 AS DefaultRank FROM Tashree3at T ");
         
        

            Range MadaNoRange = new Range(ObjSearch.MadaNo, "TM.MadaNo");
            string MadaNoCondition = MadaNoRange.GetCondition();
            if (!string.IsNullOrWhiteSpace(MadaNoCondition))
            {
                // result relate to mada  only
                isMadaNoCondition = true;
                Builder.Length = 0;
                Builder.AppendLine("SELECT T.ID ,TM.ID as madaID, 0 AS DefaultRank FROM Tashree3at T ");

                Builder.AppendLine("Join Tashree3atMawad TM on TM.Tashree3ID = T.ID ");
            
            }
            Builder.AppendLine(" Where (1 = 1 )");

            if (!string.IsNullOrWhiteSpace(MadaNoCondition))
            {
                Builder.AppendLine("And " + MadaNoCondition);
            }

            Range[] TashRanges = {
                new Range(ObjSearch.TashNo,"T.TashNo")
                ,new Range(ObjSearch.TashYear,"T.TashYear")
                ,new Range(ObjSearch.CountryIds,"T.CountryID")
            };
            foreach(var R in TashRanges)
            {
                string Condition = R.GetCondition();
                if(!string.IsNullOrWhiteSpace(Condition))
                {
                    Builder.AppendLine(" AND (" + Condition + ")");
                }
            }

            

            if (ObjSearch.SearchPredicate != null && !ObjSearch.SearchPredicate.IsValid)
            {
                // No Text search return master Query Only 
                return TashToAhkam( Builder.ToString(),isMadaNoCondition);
            }
            else
            {
                List<string> MasterQueries = new List<string>();
                // clearing the builder to start building the text queries instead of creating another builder (which is expensive ! ) 
                MasterQueries.Add(Builder.ToString());
                Builder.Clear();

                if (!ObjSearch.SearchInMadaText && !ObjSearch.SearchInTashText)
                {
                    //if both defined as false , then search in both anyway !
                    ObjSearch.SearchInTashText = ObjSearch.SearchInMadaText = true;
                }

                string PredicateText = ObjSearch.SearchPredicate.BuildPredicate();

                string TashTextQuery = "", MadaTextQuery = "" , TextQuery = "";
                if (ObjSearch.SearchInTashText)
                {
                     TashTextQuery = "SELECT TS.[KEY] AS ID , TS.[RANK]  * 10 AS DefaultRank  FROM ContainsTable(Tashree3at , Name , " + PredicateText + ") TS";
                }
                if (ObjSearch.SearchInMadaText)
                {
                     MadaTextQuery = "SELECT tm.Tashree3ID AS ID  , Sum(TS.[RANK]) AS DefaultRank  FROM ContainsTable(Tashree3atMawad , *	 , " + PredicateText + ") TS "
                        + "\n" + " JOIN dbo.Tashree3atMawad tm ON tm.ID = TS.[KEY] GROUP BY tm.Tashree3ID ";
                }

                
                if(ObjSearch.SearchInMadaText && ObjSearch.SearchInTashText)
                {
                    TextQuery = "Select ID , Sum(DefaultRank) as  DefaultRank From ("
                        + "\n" + TashTextQuery
                        + "\n" + "Union All "
                        + "\n" + MadaTextQuery
                        + "\n" + " ) TT  "
                        + "Group By ID ";

                }
                else if (ObjSearch.SearchInTashText)
                {
                    TextQuery = TashTextQuery;
                }
                else if (ObjSearch.SearchInMadaText)
                {
                    TextQuery = MadaTextQuery;
                }

                string RetVal = "Select TTS.* From ("  + TextQuery + ") TTS Join (" +  MasterQueries[0]  +  ") MQ on MQ.ID = TTS.ID";
                RetVal = TashToAhkam(RetVal, isMadaNoCondition);
                return RetVal;
            }    
            
        }

        public static string FehresSearch(int FehresItemID , out string FakaratQuery , out List<FTSPredicate> TextMatches )
        {
            TextMatches = null;
            FakaratQuery = string.Format(@"
                                    Select AF.* From VW_AhkamFakarat AF Join @ResultsPage RP  on AF.HokmID = RP.ItemID
                                    JOIN FehresItemsDetails FID ON  FID.ServiceItemID = AF.ID 
                                    WHERE  FID.FehresItemID = {0} AND FID.ServiceID = 1 " , FehresItemID);



            return string.Format(@"
                                    SELECT AF.HokmID AS ID , Count(*) AS DefaultRank FROM FehresItemsDetails FID
                                    JOIN dbo.AhkamFakarat AF ON AF.ID = FID.ServiceItemID
                                    WHERE FID.FehresItemID = {0} AND FID.ServiceID = 1 
                                    GROUP BY AF.HokmID" , FehresItemID);
        }

        public static string ResolveTasfeyaQuery(IEnumerable<AhkamTasfeyaSelection> SelectedItems)
        {
            if (SelectedItems == null || SelectedItems.Count() == 0)
            {
                return "";
            }

            var Data = (
                        from x in SelectedItems
                        where !string.IsNullOrWhiteSpace(x.Parameter)
                        group x.Parameter by x.CategoryID into g
                        select new KeyValuePair<AhkamTasfeyaCategoryIds, string>
                        (g.Key, string.Join(",", g.ToList()))
                ).ToList();

            StringBuilder Builder = new StringBuilder();
            Data.ForEach(x => Builder.AppendLine( "\n And " + ResolveTasfeyaQuerySection(x.Key, x.Value).Predicate));

            return Builder.ToString();

        }
        public static string ResolveTasfeyaQuery(IEnumerable<AhkamTasfeyaSelection> SelectedItems , string InnerQuery)
        {
            return InnerQuery +  ResolveTasfeyaQuery(SelectedItems);
        }
        public class AhkamTasfeyaParamInfo
        {
            public string Predicate { get; set; }
            public bool IsSeparateQuery { get; set; } = false;
        }

        private static AhkamTasfeyaParamInfo ResolveTasfeyaQuerySection(AhkamTasfeyaCategoryIds Category , string Params)
        {

            switch (Category)
            {
                case AhkamTasfeyaCategoryIds.Country:
                    {
                        Range R = new Range(Params, "AMS.CountryID");
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = R.GetCondition() };
                        return Info;    
                    }                    
                case AhkamTasfeyaCategoryIds.Ma7kama:
                    {
                        Range R = new Range(Params, "AMS.MahkamaID");
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = R.GetCondition() };
                        return Info;
                    }
                case AhkamTasfeyaCategoryIds.CrimeType:
                    {
                    
                        Range R = new Range(Params, "FehresItemID");
                        string Query = " AMS.ID in (Select ItemID From ServicesFehresDetails Where ServiceID = 1 And FehresProgramID = 16 And " + R.GetCondition() + "  )";
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = Query };
                        return Info;
             
                    }
                case AhkamTasfeyaCategoryIds.RelatedTash:
                    {
                        Range R = new Range(Params, "TashID");
                        string Query = " AMS.ID in (Select HokmID From Tashree3atAhkam Where  " + R.GetCondition()  +  " )";
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = Query };
                        return Info;
                        
                    }
                case AhkamTasfeyaCategoryIds.Fehres:
                    {
                        Range R = new Range(Params, "FehresItemID");
                        string Query = " AMS.ID in (Select ItemID From ServicesFehresDetails Where ServiceID = 1 And FehresProgramID = 9 And " + R.GetCondition() + "  )";
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = Query };
                        return Info;
                        
                    }
                case AhkamTasfeyaCategoryIds.Defoo3:
                    {
                        Range R = new Range(Params, "FehresItemID");
                        string Query = " AMS.ID in (Select ItemID From ServicesFehresDetails Where ServiceID = 1 And FehresProgramID = 30 And " + R.GetCondition() + "  )";
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = Query };
                        return Info;
                    }
                case AhkamTasfeyaCategoryIds.Year:
                    {
                        Range R = new Range(Params, "DatePart(Year , AMS.CaseDate)");
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = R.GetCondition() };
                        return Info;
            
                    }
                case AhkamTasfeyaCategoryIds.JudgeYear:
                    {
                        Range R = new Range(Params, "AMS.CaseYear");
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = R.GetCondition() };
                        return Info;             
                    }
                case AhkamTasfeyaCategoryIds.Ma7kamaAction:
                    {
                        Range R = new Range(Params, "AMS.IfAgree");
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = R.GetCondition() };
                        return Info;
                    }
                case AhkamTasfeyaCategoryIds.Mana3y:
                    {
                        Range R = new Range(Params, "FehresItemID");
                        string Query = " AMS.ID in (Select ItemID From ServicesFehresDetails Where ServiceID = 1 And FehresProgramID = 29 And " + R.GetCondition() + "  )";
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = Query };
                        return Info;
                    }
                case AhkamTasfeyaCategoryIds.Da3waType:
                    {
                        Range R = new Range(Params, "FehresItemID");
                        string Query = " AMS.ID in (Select ItemID From ServicesFehresDetails Where ServiceID = 1 And FehresProgramID = 8 And " + R.GetCondition() + "  )";
                        AhkamTasfeyaParamInfo Info = new AhkamTasfeyaParamInfo
                        { IsSeparateQuery = false, Predicate = Query };
                        return Info;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            return null;
        }


        public static string GetOuterQuery(QueryCacher Cacher, AhkamSearchOptions Options, string CustomFakaratQuery = null , List<AhkamTasfeyaSelection> TasfeyaSelection = null)
        {
            StringBuilder builder = new StringBuilder(2048);

            QuerySortInfo Info = GetSortQuery(Options);
            string InnerQuery = Cacher.GetCachedQuery(Info.SortQuery , Info.ColumnName);
            if(TasfeyaSelection != null && TasfeyaSelection.Count > 0)
            {
                InnerQuery = ResolveTasfeyaQuery(TasfeyaSelection ,InnerQuery);
            }
            

            // Creating the temp table and adding the current Page Data
            builder.AppendFormat(
                @"Set NoCount on ;

                Declare @PageNo int ={0} , @PageSize int = {1} 
                Declare @ResultsPage Table(ItemID int Primary Key  ,  SortValue Sql_Variant )
                Insert Into @ResultsPage 
                Select  ItemID , MyRank as SortValue From 
                (
                    Select Row_Number() Over (Order By MyRank " + Info.Direction  + @") as Serial , *  From 
                    (
                        {2} 
                    ) ResultsTableInner  
                ) ResultsTableOuter 
                Where Serial > ((@PageNo - 1) * @PageSize) And Serial <= (@PageNo * @PageSize)"
            , Options.PageNo, Options.PageSize , InnerQuery ); 


      

            // Ahkam Data  (Master Data )
            builder.AppendLine(@"Select A.* From @ResultsPage RP  
                join VW_Ahkam A WITH(NOLOCK)  On A.ID = RP.ItemID  
                Order By RP.SortValue  " + Info.Direction );

            if(Options.DisplayMode == AhkamDisplayMode.Divs)
            {
                if (!string.IsNullOrEmpty(CustomFakaratQuery))
                {
                    builder.AppendLine(CustomFakaratQuery);
                }
                else
                {
                    // get the default Fakra for each hokm  
                    builder.AppendLine(@"Select AF.* From VW_AhkamFakarat AF Join @ResultsPage RP  on AF.HokmID = RP.ItemID Where AF.IsDefault = 1 ");
                }
            }
            return builder.ToString();
        }


        public static string GetSingleHokm(int ID )
        {
            return string.Format(@"Select A.* From VW_Ahkam A Where A.ID = {0}
                                    Select AF.* From VW_AhkamFakarat AF Where AF.HokmID = {0} Order By AF.MyOrder ", ID);
        }

        public static QuerySortInfo GetSortQuery(AhkamSearchOptions Options)
        {
            QuerySortInfo Info = new QuerySortInfo();
            Info.Direction = Options.SortDirection == SearchSortType.ASC ? " ASC " : " DESC ";
            Info.SortQuery = "Join Ahkam AMS on AMS.ID = QCR.ItemID ";
            switch (Options.SortBy)
            {
                case AhkamSortColumns.CaseNo:
                    {
                        Info.ColumnName = "AMS.CaseNo";                     
                        break;
                    }
                case AhkamSortColumns.CaseDate:
                    {
                        Info.ColumnName = "AMS.CaseDate";
                        break;
                    }
                case AhkamSortColumns.CaseYear:
                    {
                        Info.ColumnName = "AMS.CaseYear";
                        break;
                    }
                case AhkamSortColumns.TashCount:
                    {
                        Info.ColumnName = "AMS.TashCount";
                        break;
                    }
                case AhkamSortColumns.DateAdded:
                    {
                        Info.ColumnName = "AMS.DateAdded";
                        break;
                    }
                default:
                    {
                        Info.ColumnName = "QCR.DefaultRank";
                        Info.SortQuery = "";
                        break;
                    }
            }
            Info.SortQuery = "Join Ahkam AMS on AMS.ID = QCR.ItemID ";
            return Info;
        }

        public class QuerySortInfo
        {
           public string ColumnName { get; set; }
           public string SortQuery { get; set; }
           public string Direction { get; set; } 
        }

    }
}
