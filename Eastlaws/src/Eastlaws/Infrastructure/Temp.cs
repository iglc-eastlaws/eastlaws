using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eastlaws.Infrastructure
{
    public struct myMatch
    {
        public int Index;
        public int Length;
        public string Value;

        public myMatch(int index, string value)
        {
            this.Index = index;
            this.Value = value;
            this.Length = value.Length;
        }
    }

    public class Temp
    {
        public const string className = "highlightedSearchText";

        public static string getHighlight(Match M)
        {
            return "<span class='" + className + "'>" + M.Value + "</span>";

        }
        public static string HighlightText(string input, string highlight, FTSSqlModes theMode = FTSSqlModes.None)
        {

            try
            {


                if (highlight == "") return input;
                if (input == "") return "";
            
                if (highlight == "") return input;

                ProcessingStart:
                highlight = Regex.Replace(highlight, @"\bال", "(ا|ل)ل");
                highlight = Regex.Replace(highlight, "[اأإآ]", "[اأإآ]");
                highlight = Regex.Replace(highlight, "[يى]", "[يى]");

                if (theMode != FTSSqlModes.None && !highlight.Contains(' ')) theMode = FTSSqlModes.None;
                if (theMode == FTSSqlModes.None)
                {
                    highlight = Regex.Replace(highlight, @"[\s]{1,}", @"[\s]{1,}");
                    return Regex.Replace(input, highlight, new MatchEvaluator(getHighlight));
                }
                else
                {
                    string[] words = highlight.Split(' ');
                    foreach (string myWord in words)
                    {
                        input = Regex.Replace(input, myWord, new MatchEvaluator(getHighlight));
                    }
                    return input;
                }
            }
            catch { return input; }
        }
        public static string HighlightTextTrim(string input, string highlight, FTSSqlModes theMode, int RequiredLength = 100)
        {


            if (highlight == "") return input.Substring(0, Math.Min(RequiredLength, input.Length));
            if (input == "") return "";


            //string className = "highlightedSearchText";

           // highlight = FTSPhraseProcess(highlight);// Processing The Phrase 
                                                    // Converting the Phrase into a Valid Regex Pattern

        ProcessingStart:
            highlight = Regex.Replace(highlight, @"\bال", "(ا|ل)ل");
            highlight = Regex.Replace(highlight, "[اأإآ]", "[اأإآ]");
            highlight = Regex.Replace(highlight, "[يى]", "[يى]");

            if (theMode != FTSSqlModes.None && !highlight.Contains(' ')) theMode = FTSSqlModes.None;



            if (theMode == FTSSqlModes.None)
            {
                highlight = Regex.Replace(highlight, @"[\s]{1,}", @"[\s]{1,}");

                Match myMatch = Regex.Match(input, highlight);
                if (myMatch.Success)
                {

                    string actualValue = myMatch.ToString();
                    if (RequiredLength >= input.Length)
                    {
                        return input.Replace(actualValue, "<span class='" + className + "'>" + actualValue + "</span>");
                    }

                    int matchIndex = myMatch.Index;
                    int matchLength = actualValue.Length;

                    int Distance = RequiredLength - matchLength;
                    int distanceHalf = (int)Math.Floor(Distance / 2d);

                    int startPoint, endPoint;



                    if (distanceHalf <= matchIndex)
                    {
                        startPoint = matchIndex - distanceHalf;
                        endPoint = Math.Min(matchLength + Distance, input.Length - startPoint);
                    }
                    else
                    {
                        startPoint = 0;
                        endPoint = Math.Min(RequiredLength, input.Length);
                    }
                    return input.Substring(startPoint, endPoint).Replace(actualValue, "<span class='" + className + "'>" + actualValue + "</span>");
                }
                else
                {
                    return input.Substring(0, Math.Min(RequiredLength, input.Length));


                }

            }
            else
            {
                highlight = string.Join("|", highlight.Split(' ').Distinct());

                if (input.Length <= RequiredLength)
                {
                    foreach (Match m in Regex.Matches(input, highlight))
                    {
                        if (m.Success)
                        {
                            input = input.Replace(m.ToString(), "<span class='" + className + "'>" + m.ToString() + "</span>");
                        }
                    }
                    return input.ToString();
                }
                else
                {
                    List<myMatch> myMatches = new List<myMatch>();
                    foreach (Match m in Regex.Matches(input, highlight))
                    {
                        if (m.Success)
                        {
                            myMatches.Add(new myMatch(m.Index, m.ToString()));
                        }
                    }
                    if (myMatches.Count > 0)
                    {
                        if (myMatches.Count == 1)
                        {
                            // Converting to sentence Matching instead of re-writing the logic 
                            highlight = myMatches[0].Value.ToString();
                            theMode = FTSSqlModes.None;
                            goto ProcessingStart;
                        }
                        else
                        {

                            int chunkStart = myMatches[0].Index,
                                chunkEnd = myMatches[myMatches.Count - 1].Index + myMatches[myMatches.Count - 1].Length,
                                chunkLength = (chunkEnd - chunkStart);
                            bool isOneChunkContainAll = chunkLength <= RequiredLength;

                            if (isOneChunkContainAll)
                            {
                                int Distance = RequiredLength - chunkLength;
                                int distanceHalf = (int)Math.Floor(Distance / 2d);
                                int startPoint, endPoint;
                                startPoint = Math.Max(chunkStart - distanceHalf, 0);
                                endPoint = Math.Min(chunkLength + Distance, input.Length - startPoint);
                                input = input.Substring(startPoint, endPoint);

                                myMatches.ForEach(x => input = input.Replace(x.Value, "<span class='" + className + "'>" + x.Value + "</span>"));
                                return input;
                            }
                            else
                            {
                                input = input.Substring(chunkStart, Math.Min(RequiredLength, input.Length - chunkStart));
                                myMatches = new List<myMatch>();
                                foreach (Match m in Regex.Matches(input, highlight))
                                {
                                    if (m.Success)
                                    {
                                        myMatches.Add(new myMatch(m.Index, m.ToString()));
                                    }
                                }

                                myMatches.ForEach(x => input = input.Replace(x.Value, "<span class='" + className + "'>" + x.Value + "</span>"));
                                return input;
                            }
                        }
                    }
                    else
                    {
                        return input.Substring(0, Math.Min(RequiredLength, input.Length));
                    }
                }
            }
        }

    }
}
