using Dapper;
using Eastlaws.Infrastructure;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Eastlaws.Services
{
    public class GeneralSearchQuery
    {

        public static List<Data> Search(string q)
        {
            using (SqlConnection _db = new DataHelpers2(DbConnections.GeneralSearch).GetConnection())
            {
                var orderBy = "ID";//"country desc"; 
                string textQuery = TextHelpers.Build_and(q, Build_Option.and).ToString();
                string searchRest = "";// " and progID = 1 ";
                var PageFrom = 1;
                var PageTo = 50;
                return _db.Query<Data>(@"with tbl_result as
                                        (
	                                        select ROW_NUMBER() over (Order by @orderBy) as RowNum,t2.*
	                                        from dbo.Data_1 as t1
	                                        CROSS APPLY fn_SearchResult(t1.ProgID,t1.recID) as t2
	                                        where contains(t1.Text,@textQuery) " + searchRest + ""
                                        +@")select serviceID, datatext, countryID, countryName from tbl_result
                                           where RowNum between @PageFrom and @PageTo", new { orderBy, textQuery, PageFrom, PageTo }).ToList();


                //return _db.Query<Data>(@"with tbl_search as 
                //                    (
                //                     select ROW_NUMBER() over (Order by @orderBy) as RowNum,t1.ID
                //                     from dbo.Data_1 as t1 where contains(t1.Text,@textQuery) " + searchRest + ""
                //                        + @"),
                //                    tbl_IDs as
                //                    (
                //                        select ID from tbl_search where RowNum between @PageFrom and @PageTo
                //                    ),
                //                    tbl_result as
                //                    (
                //                        select t2.*
                //                        from dbo.Data_1 as t1
                //                        CROSS APPLY fn_SearchResult(t1.ProgID, t1.recID) as t2
                //                        where t1.ID in (select ID from tbl_IDs)
                //                    )
                //                    select serviceID,datatext,countryID,countryName from tbl_result", new { orderBy, textQuery, PageFrom, PageTo }).ToList();
            }
        }


        public static int SearchCount(string q)
        {
            using (SqlConnection _db = new DataHelpers2(DbConnections.GeneralSearch).GetConnection())
            {
                int Result = 0;
                string textQuery = TextHelpers.Build_and(q, Build_Option.and).ToString();
                string searchRest = "";// " and progID = 1 ";
                Result = _db.Query<int>(@"select COUNT(*)  from dbo.Data_1 as t1 where contains(t1.Text,@textQuery) " + searchRest + "", new { textQuery }).First();
                return Result;
            }
        }


        public class Data
        {
            public int serviceID { get; set; }
            public string datatext { get; set; }
            public int countryID { get; set; }
            public string countryName { get; set; }
        }

        //public static List<Data> Search(string q)
        //{
        //    string searchq = TextHelpers.Build_and(q, Build_Option.and).ToString();
        //    return _db.Query<Data>(@"select top 10 [Text],Prog_ID from dbo.GS_Data_T where contains(Text, @searchq) and Prog_ID = 2 ", new { searchq }).ToList();
        //}


 
    }


 
}
