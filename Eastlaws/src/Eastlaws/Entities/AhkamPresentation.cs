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
                return this.QueryInfo.Hash;
            }
        }
        public int QueryID
        {
            get
            {
                return this.QueryInfo.ID;
            }
        }        

        public AhkamPresentation()
        {
                
        }
    }
}
