/*
*  FILE          : StringSanitizer.cs
*  PROGRAMMER    : Randy Lefebvre 2256 and Bence Karner5307
*  DESCRIPTION   : Contains the StringSanitizer class which allows the program to better handle string validation
*/

using System;
using System.Text.RegularExpressions;

namespace SOA_Assign02
{

    /*
    *   NAME    :   StringSanitizer
    *   PURPOSE :   Used to sanitize the users input, before its included in the SOAP envelope.
    *               provides methods for removing special characters, digits, and word characters. 
    */
    internal static class StringSanitizer
    {

        //DEBUG ADD REFERENCE BEFORE SUBMISSION 
        //https://stackoverflow.com/questions/4418279/regex-remove-special-characters
        internal static string RemoveSpecialCharacters(string input)
        {
            //|(?<=['\"])s)
            Regex regexResult = new Regex("(?:[^a-z0-9,.])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return regexResult.Replace(input, string.Empty);
        }

        internal static string RemoveLetters(string input)
        {
            Regex regexResult = new Regex("(?:[^0-9,.])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return regexResult.Replace(input, string.Empty);
        }


        internal static string FormatForMoney(string input)
        {
            Regex regexResult = new Regex("", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return regexResult.Replace(input, string.Empty);
        }

    }//class
}//namespace
