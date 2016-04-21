using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.ViewModels.Ahkam.Search
{
    public class AssemblySearch
    {
        public int? alltextSearchType { get; set; }
        public string Alltext { get; set; }
        public int? MabdaaSearchType { get; set; }
        public string Mabdaa { get; set; }
        public int? waka23SearchType { get; set; }
        public string waka23 { get; set; }
        public int? hay2aSearchType { get; set; }
        public string hay2a { get; set; }
        public int? hyseytSearchType { get; set; }
        public string hyseyt { get; set; }
        public string dateGalsaFrom { get; set; }
        public string dateGalsaTo { get; set; }
        public int? caseYear { get; set; }
        public int? pageNo { get; set; }
        public int? officeYear { get; set; }
        public int? partNo { get; set; }
        public int? country { get; set; }
        public string mahakem { get; set; }
    }
}
