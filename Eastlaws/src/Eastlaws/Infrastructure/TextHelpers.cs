using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eastlaws.Infrastructure
{
    public enum Build_Option{ and , or }
 
    public class TextHelpers
    {
        static public StringBuilder Build_and(string InputText, Build_Option buildoption)
        {

            StringBuilder Result = new StringBuilder();
            if (InputText.Trim() != string.Empty)
            {
                string[] pieces = InputText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string piec in pieces)
                {
                    Result.AppendFormat(@" ""{0}"" {1} ", piec, buildoption);
                }

                if (Result.Length > 0)
                {
                    var lastOptionLenth = buildoption.ToString().Length + 2;
                    Result = Result.Remove(Result.Length - lastOptionLenth, lastOptionLenth);
                }
            }
            return Result;
        }

        static public string RemoveNoiseWords(string InputText)
        {
            return Regex.Replace(InputText.ToString(), @"(مع)|(و)|(هو)|(كما)|(على)|(ألى)|(إلى)|(الى)|(في)|(فى)|(من)", string.Empty);
        }
    }
}
