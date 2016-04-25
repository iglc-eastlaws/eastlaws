using Eastlaws.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Eastlaws.Services
{
    public enum AhkamTasfeyaCategories
    {
        Country = 0, Ma7kama = 1, CrimeType = 2, RelatedTash = 3, Fehres = 4, Defoo3 = 5, Year = 6, JudgeYear = 7, Ma7kamaAction = 8
    }
    public class TasfeyaItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public AhkamTasfeyaCategories CategoryID { get; set; }
    }

    public class AhkamTasfeya
    {
        private const int MAX_RECORD_COUNT_PER_CATEGORY = 100;


        public static IEnumerable<TasfeyaItem> List (int QueryID , AhkamSearchTypes SearchType, out List<AhkamTasfeyaCategories> UsedCategories , string TasfeyaFilter = "" , string PreviousTasfeyaQuery  = "", AhkamTasfeyaCategories? CategorySender = null )
        {
            List<AhkamTasfeyaCategories> Cats = new List<AhkamTasfeyaCategories>();

            Cats.Add(AhkamTasfeyaCategories.Country);
            Cats.Add(AhkamTasfeyaCategories.Ma7kama);
            Cats.Add(AhkamTasfeyaCategories.JudgeYear);
            Cats.Add(AhkamTasfeyaCategories.Year);
   

            bool IsSafyList = !string.IsNullOrWhiteSpace(PreviousTasfeyaQuery);
            string SafyListQuery = "";
            if (IsSafyList)
            {
                if (CategorySender.HasValue)
                {
                    Cats.Remove(CategorySender.Value);
                }

                SafyListQuery = 
                    @"Create  Table #IDS(ID int Primary Key ) ; 
                      Insert Into #IDS(ID ) 
                      Select QCR.ItemID From Ahkam A Join EastlawsUsers..QueryCacheRecords QCR  on A.ID = QCR.ItemID
                      Where  QCR.MasterID=" + QueryID + " And "+ PreviousTasfeyaQuery + " \n\n\n";
            }


            string MasterQuery = string.Join("\n Union All \n " ,  (from c in Cats
                                                  select GetCategoryQuery(QueryID, c, TasfeyaFilter, IsSafyList)
                                                  ).ToArray()
                                  );

            MasterQuery = string.Format("Select * From \n (\n{0}\n) \n TasfeyaList Order By CategoryID , SortValue Desc "  , MasterQuery);

            if (IsSafyList)
            {
                MasterQuery = SafyListQuery + MasterQuery;
            }

            var Con  = DataHelpers.GetConnection(DbConnections.Data);
            var List =  Con.Query<TasfeyaItem>(MasterQuery);
            UsedCategories = Cats;
            return List;
        }

        private static string GetCategoryQuery(int MasterQueryID , AhkamTasfeyaCategories Cat , string Filter, bool isSafyList = false)
        {
            string SafyListJoinClause = (isSafyList) ? " Join #IDS I on I.ID = QCR.ItemID "  : "";
            switch (Cat )
            {
                
                case AhkamTasfeyaCategories.Country:
                    {
                        string ProcessedFilter = "";
                        FTSPredicate P = new FTSPredicate(Filter, FTSSqlModes.AND);
                        if (P.IsValid)
                        {
                            ProcessedFilter = " And Contains (C.Name , " + P.BuildPredicate() + ")";
                        }
                        return string.Format(
                               @"Select Top {0} C.ID , C.Name , Count(*)  as Count , 0 as CategoryID , Count(*) as SortValue  From Countries C 
                                Join Ahkam A on A.CountryID = C.ID
                                Join EastlawsUsers..QueryCacheRecords QCR With (Nolock)  On QCR.ItemID = A.ID 
                                {3}
                                Where QCR.MasterID  = {1}  {2}
                                Group By C.ID , C.Name 
                                Order By Count(*) desc "
                                , MAX_RECORD_COUNT_PER_CATEGORY, MasterQueryID , ProcessedFilter , SafyListJoinClause);
                        
  
                    }
                case AhkamTasfeyaCategories.Ma7kama:
                    {
                        string ProcessedFilter = "";
                        FTSPredicate P = new FTSPredicate(Filter, FTSSqlModes.AND);
                        if (P.IsValid)
                        {
                            ProcessedFilter = " And Contains (M.Name , " + P.BuildPredicate() + ")";
                        }

                        return string.Format(
                            @"Select Top {0} M.ID , M.Name , Count(*) as Count , 1 as CategoryID , Count(*) as SortValue From AhkamMahakem M 
                            Join Ahkam A on A.MahkamaID = M.ID
                            Join EastlawsUsers..QueryCacheRecords QCR With (Nolock)  On QCR.ItemID = A.ID 
                            {3}
                            Where QCR.MasterID  = {1}  {2}  
                            Group By M.ID , M.Name 
                            Order By Count(*) desc "
                            , MAX_RECORD_COUNT_PER_CATEGORY, MasterQueryID, ProcessedFilter, SafyListJoinClause);
                    }
                case AhkamTasfeyaCategories.JudgeYear:
                    {
                        string ProcessedFilter = "";

                        return string.Format(
                            @"Select Top {0} CaseYear as ID , Cast(A.CaseYear as varchar(32)) as Name , Count(*) as Count , 7 as CategoryID , Count(*) as SortValue From Ahkam A
                                               Join EastlawsUsers..QueryCacheRecords QCR With (Nolock)  On QCR.ItemID = A.ID 
                                               {3}
                                               Where QCR.MasterID  = {1}  {2}  
                                               Group By A.CaseYear
                                               Order By Count(*) desc ", MAX_RECORD_COUNT_PER_CATEGORY, MasterQueryID, ProcessedFilter, SafyListJoinClause);
                    }
                case AhkamTasfeyaCategories.Year:
                    {
                        string ProcessedFilter = "";
                        return string.Format(
                            @"Select Top {0} Datepart(Year , A.CaseDate )  as ID , Cast(Datepart(Year , A.CaseDate ) as varchar(32)) as Name , Count(*) as Count , 6 as CategoryID , Count(*) as SortValue From Ahkam A
                            Join EastlawsUsers..QueryCacheRecords QCR With (Nolock)  On QCR.ItemID = A.ID 
                            {3}
                            Where QCR.MasterID  = {1}  And A.CaseDate is not null  {2}  
                            Group By Datepart(Year , A.CaseDate )
                            Order By Count(*) desc ", MAX_RECORD_COUNT_PER_CATEGORY, MasterQueryID, ProcessedFilter, SafyListJoinClause);
                    }                    
                default:
                    {
                        return "";               
                    }
            }
        }

        public static string GetCategoryTitle(AhkamTasfeyaCategories theType)
        {
            string Title = "";
            switch (theType)
            {
                case AhkamTasfeyaCategories.Country:
                    {
                        Title = "بالدولة"; break;
                    }
                case AhkamTasfeyaCategories.Ma7kama:
                    {
                        Title = "بالمحكمة"; break;
                    }
                case AhkamTasfeyaCategories.CrimeType:
                    {
                        Title = "بنوع الدعوى/ الجريمة"; break;
                    }
                case AhkamTasfeyaCategories.RelatedTash:
                    {
                        Title = "بالقانون"; break;
                    }
                case AhkamTasfeyaCategories.Fehres:
                    {
                        Title = "بتصنيف المبادئ"; break;
                    }
                case AhkamTasfeyaCategories.Defoo3:
                    {
                        Title = "بالدفوع"; break;
                    }
                case AhkamTasfeyaCategories.Year:
                    {
                        Title = "بسنة الحكم"; break;
                    }
                case AhkamTasfeyaCategories.JudgeYear:
                    {
                        Title = "بالسنة القضائية"; break;
                    }
                case AhkamTasfeyaCategories.Ma7kamaAction:
                    {
                        Title = "بحكم المحكمة فى الدعوى"; break;
                    }
            }
            return Title;
        }


    }
}
