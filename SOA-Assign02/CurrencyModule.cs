/*
*  FILE          : CurrencyModule.cs
*  PROGRAMMER    : Randy Lefebvre 2256 and Bence Karner5307
*  DESCRIPTION   : Contains the currency module class. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOA_Assign02
{

    /*
    *   NAME    :   CurrencyModule
    *   PURPOSE :   This class is for interacting with financial data, and analyzing strings that represent
    *               money. 
    */
    class CurrencyModule
    {
        /*
         * Currency Name:           Alpha:  Major:  Minor:  Decimals    Format
         * Australian Dollar	    AUD	036	dollar	cent	2	        # ###.##
         * Bahamian Dollar	        BSD	044	dollar	cent	2	        #,###.##
         * Barbados Dollar	        BBD	052	dollar	cent	2
         * Belize Dollar	        BZD	084	dollar	cent	2	        #,###.##
         * Brunei Dollar	        BND	096	ringgit	sen	    2	        #,###.##
         * Canadian Dollar	        CAD	124	dollar	cent	2	        #,###.##
         * Cayman Islands Dollar	KYD	136	dollar	cent	2	        #,###.##
         * East Caribbean Dollar	XCD	951	dollar	cent	2	        #,###.##
         * Fiji Dollar	            FJD	242	dollar	cent	2	
         * Guyana Dollar	        GYD	328	dollar	cent	2	
         * Hong Kong Dollar	        HKD	344	dollar	cent	2	        #,###.##
         * Jamaican Dollar	        JMD	388	dollar	cent	2	        #,###.##
         * Liberian Dollar	        LRD	430	dollar	cent	2	
         * Namibian Dollar	        NAD	516	dollar	cent	2	 
         * New Zealand Dollar	    NZD	554	dollar	cent	2	        #,###.##
         * 
         * Supported Formats:
         * #,###.## American
         * #.###,## European
         * # ###.## Australian
         */
        internal static string DetermineCurrencyFormat()
        {

            return "DEBUG ISERT FORMAT HERE";
        }

    }//class
}//namespace
