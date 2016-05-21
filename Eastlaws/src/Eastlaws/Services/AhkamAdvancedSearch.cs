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
        public FTSPredicate PredicateWakae3 { get; set; } = new FTSPredicate("");
        public FTSPredicate PredicateDestoreya { get; set; } = new FTSPredicate("");
        public FTSPredicate PredicateHay2a { get; set; } = new FTSPredicate("");
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


        public List<FTSPredicate> GetValidPredicates()
        {

            PredicateAny.Tag = 5000;
            PredicateMabade2.Tag = 1;
            PredicateDestoreya.Tag = -50;
            PredicateWakae3.Tag = -1;
            PredicateMantoo2.Tag = -3;//canceled
            PredicateHaytheyat.Tag = -2;
            PredicateHay2a.Tag = 0;

            FTSPredicate[] MyArray = { PredicateAny, PredicateMabade2, PredicateDestoreya, PredicateWakae3, PredicateMantoo2, PredicateHaytheyat, PredicateHay2a };

            List<FTSPredicate> retVal = new List<FTSPredicate>();
            for (int i = 0; i < MyArray.Length; i++)
            {
                if (MyArray[i].IsValid)
                {
                    retVal.Add(MyArray[i]);
                }
            }

            return retVal;
        }
    }

 


}
