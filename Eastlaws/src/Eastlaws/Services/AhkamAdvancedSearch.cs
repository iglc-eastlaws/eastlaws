using Eastlaws.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Services
{
    public class AhkamAdvancedSearch
    {
        public FTSPredicate PredicateAny { get; set; } = new FTSPredicate("");

        public FTSPredicate PredicateMabade2 { get; set; } = new FTSPredicate("");
        public FTSPredicate PredicateWakae3  { get; set; } = new FTSPredicate("");
        public FTSPredicate PredicateDestoreya { get; set; } = new FTSPredicate("");
        public FTSPredicate PredicateHay2a  { get; set; } = new FTSPredicate("");
        public FTSPredicate PredicateMantoo2 { get; set; } = new FTSPredicate("");
        public FTSPredicate PredicateHaytheyat { get; set; } = new FTSPredicate("");


        public string CountryIDs{get;set;}
        public string Ma7akemIds{get;set;}
        public string CaseNo{get;set;}
        public string CaseYear{get;set;}
        public string PartNo{get;set;}
        public string PageNo{get;set;}
        public string OfficeYear{get;set;}
        public string IFAgree{get;set;}
        public string OfficeSuffix{get;set;}
        public string CaseDatefrom{get;set;}
        public string CaseDateTo { get; set; }
    }
}
