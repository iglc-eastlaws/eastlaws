using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eastlaws.Infrastructure
{
    public class Range
    {      
        public string SeparatorListPattern { get; set; } = @"\s*[,و]\s*";        
        public string SeparatorFromToPattern{ get; set; }  = @"\s*[\-]\s*";
        public string ItemPattern { get; set; } = @"[\d]{1,10}";
        public string Input { get; set; }
        private string ColumnName { get; }

        public Range(string Input , string ColumnName)
        {
            this.Input = Input.Trim();
            this.ColumnName = ColumnName;
        }

        public string GetCondition()
        {
            if(string.IsNullOrWhiteSpace(Input))
            {
                return "";
            }

            if(Regex.IsMatch(Input, @"^" +  ItemPattern + @"$",RegexOptions.Compiled))
            {
                return "("  + ColumnName + " = " + Input + " ) ";
            }
            string ListPattern = "^" + "(" + ItemPattern + "(" + SeparatorListPattern+ ")?"  + "){1,}" + "$";
            if(Regex.IsMatch(Input , ListPattern, RegexOptions.Compiled))
            {
                string[] Vals = Regex.Split(Input, SeparatorListPattern);
                StringBuilder builder = new StringBuilder();
                builder.Append(ColumnName + " IN (");
                int IncludedCount = 0;
                for(int i = 0; i < Vals.Length; i++)
                {
                    if(Regex.IsMatch(Vals[i] , ItemPattern))
                    {
                        builder.Append(Vals[i]);
                        builder.Append(",");
                        IncludedCount++;
                    }                   
                }
                if(IncludedCount == 0)
                {
                    return "";
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append(") ");
                return builder.ToString();
            }

            string FromToPattern = "^" + ItemPattern + SeparatorFromToPattern + ItemPattern + "$";
            if (Regex.IsMatch(Input, FromToPattern, RegexOptions.Compiled))
            {
                string[] Vals = Regex.Split(Input , SeparatorFromToPattern);
                return " (" + ColumnName + " >= " + Vals[0] + " And " + ColumnName + " <= " + Vals[1] + ") ";
            }


            // fail safe 
            Match M = Regex.Match(Input, ItemPattern);
            if(M != null && M.Success)
            {
                return "(" + ColumnName + " = " + M.Value + " ) ";
            }

            return "";
        }


    }
}
