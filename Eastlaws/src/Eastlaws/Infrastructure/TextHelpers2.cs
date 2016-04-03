using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Infrastructure
{

    public class FTSPredicate
    {
        private string m_Input = "";
        private string m_Output = "";
        private bool isValid = false;
        private MatchType m_Type = MatchType.Gomla;
        
        

        public string Input
        {
            get { return m_Input; }
        }
        public string Output
        {
            get
            {

                return m_Output;
            }
        }


        public FTSPredicate(string Input)
        {
            m_Input = Input;
        }
        public FTSPredicate(string Input , MatchType Type )
        {
            m_Input = Input;
            this.m_Type = Type;
        }

        public string BuildPredicate(MatchType Type)
        {
            string Value = "";
            RemoveSqlInjection();
            if (Type == MatchType.Gomla)
            {
                
            }
            else
            {
                string Operator = " And ";
                RemoveSpecialChars();
                string[] Vals = m_Input.Split(' ');

                if(Type == MatchType.AllWords) // AND 
                {

                }
                else // OR 
                {
                    Operator = " OR ";
                }

                Value = "( " + string.Join( Operator , Vals) + " )";

            }
            m_Output = Value;
            return Value;
        }
        private string RemoveSpecialChars()
        {
            m_Input = m_Input.Replace("!", "");
            return m_Input;
        }
        private string RemoveSqlInjection()
        {
            m_Input = m_Input.Replace("'", "");
            return m_Input;
        }
        private string RemoveNoiseWords()
        {
            m_Input = m_Input.Replace("'", "");
            return m_Input;
        }




    }
}
