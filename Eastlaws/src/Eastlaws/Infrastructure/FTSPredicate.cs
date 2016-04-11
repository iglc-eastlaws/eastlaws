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


        //Escaped : (' , * ,  ! , " , # , ; , " , . )
        private static Regex m_RgxSpecialChars = new Regex("['!\\*#;\\\"@]", RegexOptions.Compiled);



        public FTSSqlModes SqlMode { get; set; } = FTSSqlModes.None;
        public bool IsValid { get; private set; } = false;
        public string Output { get; private set; }

        public string Input
        { get; }
        

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
            this.Output =  BuildPredicate(Input, SqlMode);
            return this.Output;
        }

        public string BuildPredicate(string Input  , FTSSqlModes  Mode)
        {
            string retValue =  RemoveSpecialChars(Input);                   
            if (Mode == FTSSqlModes.None)
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

        private string RemoveSpecialChars(string Input)
        {
            return m_RgxSpecialChars.Replace(Input, "");
        }
    }

}
