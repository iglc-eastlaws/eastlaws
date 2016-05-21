using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eastlaws.Infrastructure
{

    public enum FTSSqlModes
    { None =  0  , AND = 1 , OR = 2 }


    public class FTSPredicate
    {
        //Escapes : (' , * ,  ! , " , # , ; , " , . )
        private static Regex m_RgxSpecialChars = new Regex("['!\\*#;\\\"@]", RegexOptions.Compiled);

        // Escapes :  weird (redundant) patterns like  (..........) and (!!!!!!!) starting from more than 3 chars 
        private static Regex m_RgxRedundantChars = new Regex(@"(.)\1{3,}", RegexOptions.Compiled);

        // Prepares the string to be a valid arabic phrase  Only  "و العطف"  is Implemnted  
        private static Regex m_RgxArabicPhrase = new Regex(@"", RegexOptions.Compiled);

        // Min and max valid lengths after filtering the predicate 
        private const int MIN_PREDICATE_LENGTH = 2;
        private const int MAX_PREDICATE_LENGTH = 1000;

        

        public string Input { get; }
        public string Output { get; private set; } = null;    
        public FTSSqlModes SqlMode { get; set; } = FTSSqlModes.None;
        private bool IsPredicateBuilt { get; set; } = false;
        public object Tag { get; set; }
        public bool IsValid
        {
            get
            {
                if (!this.IsPredicateBuilt)
                {
                    this.BuildPredicate();
                }
                return !string.IsNullOrEmpty(this.Output);
            }
        }



        public FTSPredicate(string Input)
        {
            this.Input = Input;
 
        }
        public FTSPredicate(string Input , FTSSqlModes Mode)
        {
            this.Input = Input;
            SqlMode = Mode;
        }
        public FTSPredicate(string Input , MatchType MatchingType)
        {
            this.Input = Input;
            SqlMode = GetSQLMode(MatchingType);
        }


        private FTSSqlModes GetSQLMode(MatchType MatchingType)
        {
            switch (MatchingType)
            {
                case MatchType.AllWords: return FTSSqlModes.AND;
                case MatchType.Gomla: return FTSSqlModes.None;
                case MatchType.AnyWord: return FTSSqlModes.OR;
            }
            return FTSSqlModes.None;
        }

        public string BuildPredicate()
        {
            if(this.IsPredicateBuilt)
            {
                return this.Output;
            }
            this.Output =  BuildPredicate(Input, SqlMode);
            this.IsPredicateBuilt = true;
            return this.Output;
        }

        public override string ToString()
        {
            return this.BuildPredicate();
        }



        /// <summary>
        /// Constructs and Validates the FTS Predicate string to be appended as a clause in SQL Query 
        /// Returns null if the Input string is invalid 
        /// </summary>
        /// <param name="Input">Input word / Phrase  </param>
        /// <param name="Mode">Sql server FTS Mode : (And , OR , None) , None treats the input as one unit (Phrase ) </param>
        /// <returns></returns>
        public static string BuildPredicate(string Input  , FTSSqlModes  Mode)
        {
            if (string.IsNullOrWhiteSpace(Input))
            {
                return null;
            }
            else
            {
                Input = Input.Trim();
            }

            string retValue =  RemoveSpecialChars(Input);
            retValue = RemoveWeirdPatterns(retValue);
            retValue = PrepareArabicPhrase(retValue);



            if (string.IsNullOrWhiteSpace(retValue) || retValue.Length <  MIN_PREDICATE_LENGTH || retValue.Length > MAX_PREDICATE_LENGTH )
            {
                return null;
            }

            if (Mode == FTSSqlModes.None || !retValue.Contains(' '))
            {
                retValue = " '\"" + retValue + "\"' ";
            }
            else
            {
                string Operator =  " " + Mode.ToString()  + " ";               
                string[] Vals = Input.Split(new char[] { ' '}  , StringSplitOptions.RemoveEmptyEntries);
                retValue = " '" + string.Join( Operator , Vals) + "' ";
            }          
            return retValue;
        }

        private static string RemoveSpecialChars(string Input)
        {
            return m_RgxSpecialChars.Replace(Input, "");
        }
   
        private static string RemoveWeirdPatterns(string Input)
        {
            return m_RgxRedundantChars.Replace(Input, "");        
        }
        // todo : Implement this method !
        private static string PrepareArabicPhrase(string Input)
        {
            return Input;
        }
    }

}
