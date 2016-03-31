using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws
{
    //************************ I suppose   

    // searchOptions
    public enum TextSearchMatch
    {
        Gomla = 0 , AllWords = 1 , AnyWord  = 2
    }

    public class simpleInputSearch
    {
        public string Filter { get; set; } // input

        TextSearchMatch Match { get; set; } // searchOptions

        public bool isEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(Filter) || Filter.Trim() == "")
                {
                    return false;
                }
                return true;
            }
        }

    }

}
