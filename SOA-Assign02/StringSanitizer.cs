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
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, string.Empty);
        }

        internal static string RemoveLetters(string input)
        {
            Regex r = new Regex("", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, string.Empty);
        }

    }//class
}//namespace
