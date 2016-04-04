using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Services
{
    public class AhkamQueryBuilder
    {
        public static string GeneralSearch(string FTSPredicate)
        {
            return "Select F.HokmID as ID , Sum(CT.[Rank]) as TextRank From  "
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
    }
}
