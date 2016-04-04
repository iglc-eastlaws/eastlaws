using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws
{
    public enum MatchType
    {
        Gomla = 0 , AllWords = 1 , AnyWord  = 2
    }

    public class SimpleInputSearch
    {
        private string m_Input = null;

        public string Input
        {
            get
            {   
                return m_Input;
            }
            set
            {
                m_Input = value.Trim();
            }
        }
        MatchType Match{ get; set; } 
        public bool isEmpty
        {
            get
            {
                if (string.IsNullOrEmpty(Input) || Input.Trim() == "")
                {
                    return true;
                }
                if(Input.Length  < 3)
                {
                    return true;
                }
                return false;
            }
        }
    }

}
