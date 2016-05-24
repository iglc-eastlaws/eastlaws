using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Infrastructure;

namespace Eastlaws.Services
{
    public class AhkamMadaSearch
    {
       public bool SearchInMadaText { get; set; } = true;
       public bool SearchInTashText { get; set; } = true;
       public FTSPredicate SearchPredicate { get; set; } = null;
       public string CountryIds { get; set; } = "";
       public string TashNo { get; set; } = "";
       public string TashYear { get; set; } = "";
       public string MadaNo { get; set; } = "";

    }
}
