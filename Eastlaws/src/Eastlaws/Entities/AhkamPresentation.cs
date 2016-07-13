using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Infrastructure;

namespace Eastlaws.Entities
{
    public class AhkamPresentation
    {
        public int ResultsCount { get; set; }
        public IEnumerable<VW_Ahkam> AhkamList { get; set; }
        public IEnumerable<VW_AhkamFakarat> FakaratList { get; set; }
        public bool IsValid { get; set; } = true;
        public QueryInfo QueryInfo { get; set; }
        public string QueryHash
        {
            get
            {
                if (this.QueryInfo != null)
                {
                    return this.QueryInfo.Hash;
                }
               return "";
            }
        }
        public int QueryID
        {
            get
            {
                if(this.QueryInfo != null)
                {
                    return this.QueryInfo.ID;
                }
               
                return -1;
            }
        }
        public string PresentationTitle { get; set; } = "";
        public List<FTSPredicate> TextPredicates { get;  set; }



        public AhkamPresentation()
        {
                
        }
    }
}
