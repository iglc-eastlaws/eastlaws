using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws
{
    public static class Global
    {
        public static string ConnectionStringData
        {
            get
            {
                return "Server=192.168.1.251;Database=EastlawsData;User ID=DevUser;Password=DevUser123456";
            }
        }
        public static string ConnectionStringUsers
        {
            get
            {
                return "Server=192.168.1.251;Database=EastlawsUsers;User ID=DevUser;Password=DevUser123456";
            }
        }
    }



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
