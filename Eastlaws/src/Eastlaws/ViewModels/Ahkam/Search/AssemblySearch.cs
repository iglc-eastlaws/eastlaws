using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.ViewModels.Ahkam
{
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
