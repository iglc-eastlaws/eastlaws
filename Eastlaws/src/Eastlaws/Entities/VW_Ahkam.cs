using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
        public class VW_Ahkam
        {
            public int ID { get; set; }
            public int MahkamaID { get; set; }
            public int? CountryID { get; set; }
            public int? CaseNo { get; set; }
            public int? CaseYear { get; set; }
            public int? OfficeYear { get; set; }
            public string OfficeSuffix { get; set; }
            public int? PageNo { get; set; }
            public int? PartNo { get; set; }
            public int? IfAgree { get; set; }
            public int? ImagesCount { get; set; }
            public int? TashCount { get; set; }
            public int? TashMawadCount { get; set; }
            public int? CaseDateDay { get; set; }
            public int? CaseDateMonth { get; set; }
            public int? CaseDateYear { get; set; }
            public DateTime? CaseDate { get; set; }
            public string MahkamaName { get; set; }
            public string CountryName { get; set; }
            public string FullName { get; set; }    }
}

