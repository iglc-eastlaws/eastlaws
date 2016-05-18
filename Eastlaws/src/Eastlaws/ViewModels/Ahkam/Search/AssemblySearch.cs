using Eastlaws.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.ViewModels.Ahkam
{
    public class SearchParms
    {
        public AssemblySearch AssemblySearch { get; set; }
        public SearchTools SearchTools { get; set; }
        public List<AhkamTasfeyaSelection> ahkamTasfeya { get; set; }
 
    }

    public class SarchTasfyaParms
    {
        public AssemblySearch AssemblySearch { get; set; }
        public List<AhkamTasfeyaSelection> ahkamTasfeyaList { get; set; }
        public string TasfeyaSearchText { get; set; } = "";
        public int SelectedCatgID { get; set; }


    }

    public class SearchTools
    {
        public int PageNo { get; set; } 
        public int Sort { get; set; }
        public int pageSize { get; set; }
        public bool Latest { get; set; } = false;
        public int Days { get; set; } = 10;
        public int typeView { get; set; } = 1;
        public int SortDir { get; set; } = 1;

    }

        public class AssemblySearch
    {
        public int? alltextSearchType { get; set; } = 0;
        public string Alltext { get; set; } = "";
        public int? MabdaaSearchType { get; set; } = 0;
        public string Mabdaa { get; set; } = "";
        public int? waka23SearchType { get; set; } = 0;
        public string waka23 { get; set; } = "";
        public int? hay2aSearchType { get; set; } = 0;
        public string hay2a { get; set; } = "";
        public int? hyseytSearchType { get; set; } = 0;
        public string hyseyt { get; set; } = "";
        public string dateGalsaFrom { get; set; } = "";
        public string dateGalsaTo { get; set; } ="";
        public string caseYear { get; set; } = "";
        public string caseNo { get; set; } = "";
        public string pageNo { get; set; } = "";
        public string officeYear { get; set; } = "";
        public string partNo { get; set; } = "";
        public string country { get; set; } = "";
        public string mahakem { get; set; } = "";
        public string MahkamaReplay { get; set; } = "";
        public string omarGroup { get; set; } = "";
    }
}
