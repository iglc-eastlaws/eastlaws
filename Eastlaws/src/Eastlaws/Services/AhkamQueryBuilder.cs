using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static string AdvancedSearch(string FTSPredicate)
        {
            return "";
        }

        public static string GetOuterQuery(string InnerQuery , int PageSize  = 10, int PageNo = 1 )
        {
            StringBuilder builder = new StringBuilder();
            // Creating the temp table and adding the current Page Data
            builder.Append("Declare @PageNo int ="  + PageSize + " , @PageSize int = " + PageSize);
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
