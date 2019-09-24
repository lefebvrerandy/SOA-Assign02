/*
*  FILE          : FileManager.cs
*  PROGRAMMER    : Randy Lefebvre 2256 and Bence Karner5307
*  DESCRIPTION   : Contains the FileManager class, which provides the program with basic I/O functions like creating, and reading text. 
*                  Also contains methods for creating, and interacting with xml documents
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using EventLogger;

namespace SOA_Assign02
{

    /*  
     *  NAME    : FileManager
     *  PURPOSE : This class is used to provide IO support to the application. It contains methods for reading, 
     *            writing, and interacting with text and xml files. 
     */
    class FileManager
    {
        public List<Tuple<string, string, string>> configList;  //Used to hold all the details of the web services


        public FileManager()
        {
            CreateValidationFile(Constants.VALIDATION_FILEPATH);
            LoadConfigurationFile(Constants.CONFIG_FILEPATH);
        }


        /*
        *   METHOD        : CreateValidationFile
        *   DESCRIPTION   : Checks if the validations file exists, if not, create one with defaults for the provided service
        *   PARAMETERS    : string filename : file path to the file containing the validations
        *   RETURNS       : bool : Returns true if the file exists, or was successfully created
        */
        internal bool CreateValidationFile(string filename)
        {
            //Check if the validation file exist
            if (!File.Exists(filename))
            {

                //Validations not present, create a file with defaults
                string[] defaultValidations = {
                    @"^(\d+);(\d+)",
                    @"^(\d+);(\d+)",
                    @"^(\d+);(\d+)",
                    @"^(\d+);(\d+)",
                    @"^(\d{0,3}.\d{0,3}.\d{0,3}.\d{0,3});(0)$",
                    @"^(\w+)$",
                    "",
                    "" };
                try
                {
                    File.WriteAllLines(Constants.VALIDATION_FILEPATH, defaultValidations);
                }
                catch (Exception error)
                {
                    Logger.RecordError(error.Message);
                    return false;
                }
            }
            return true;
        }


        /*
        *   METHOD        : ReadAllLines
        *   DESCRIPTION   : Reads in all the lines from a file
        *   PARAMETERS    : string filename : file path to the file containing the validations
        *   RETURNS       : string[] : Returns all the lines read from the file
        */
        internal string[] ReadAllLines(string filepath)
        {
            return File.ReadAllLines(filepath); ;
        }


        /*
        *   METHOD        : LoadConfigurationFile
        *   DESCRIPTION   : Reads in the service details from the config file
        *   PARAMETERS    : string filename : file path to the file containing the validations
        *   RETURNS       : List<Tuple<string, string, string>> : Contains the service name, request , response for each service
        */
        public List<Tuple<string, string, string>> LoadConfigurationFile(string fileName)
        {
            string name = "";
            string request = "";
            string response = "";
            List<Tuple<string, string, string>> webServicePackage = new List<Tuple<string, string, string>>();

            try
            {
                // Open up the file and store the web services into the List<tuple>
                using (StreamReader file = new StreamReader(fileName))
                {
                    string line = "";
                    int section = 0;
                    int holdSection = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        // Determine which section we are currently reading

                        if (line == "name=")
                        {
                            // This if statement checks if we've already stored an entry.
                            //  If we have, store the currently obtained entry and retrieve the next
                            if (section == 2)
                            {
                                webServicePackage.Add(Tuple.Create<string, string, string>(name, request, response));
                                name = "";
                                request = "";
                                response = "";
                            }
                            section = 0;
                            line = "";
                        }
                        else if (line == "request=")
                        {
                            section = 1;
                            line = "";
                        }
                        else if (line == "response=")
                        {
                            section = 2;
                            line = "";
                        }
                        else if (line.StartsWith("#"))
                        {
                            //This is a comment. Ignore the line but store the section for later
                            holdSection = section;
                            section = 3;
                        }
                        else if (line.StartsWith("@END FILE"))
                        {
                            break;
                        }


                        // Determine which section we are currently in, and store the value into the
                        //  proper string
                        if (section == 0)
                        {
                            name += line;
                            if (line != "")
                                name += "\r\n";
                        }
                        else if (section == 1)
                        {
                            request += line;
                            if (line != "")
                                request += "\r\n";
                        }
                        else if (section == 2)
                        {
                            response += line;
                            if (line != "")
                                response += "\r\n";
                        }
                        else if (section == 3)
                        {
                            section = holdSection;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                // File does not exist. Return empty sorted list
                Logger.RecordError(error.Message);
                return new List<Tuple<string, string, string>>();
            }

            // Store the values into the list<tuple>
            webServicePackage.Add(Tuple.Create<string, string, string>(name, request, response));
            configList = webServicePackage;
            return webServicePackage;
        }


        /*
        *   METHOD        : ParseWebService
        *   DESCRIPTION   : Sets the url, action, and request corresponding to the service
        *   PARAMETERS    : string selectedServiceMethod : Selected web service from the combo box
        *                   List<Tuple<string,string,string>> webPackage :  All the contents of the web service combo box
        *                   string[] paramArray : Arguments entered by the user into the form
        *   RETURNS       : string[] : Contains the url[0], action[1], and request[2] of the web request
        */
        public string[] ParseWebService(string selectedServiceMethod, List<Tuple<string,string,string>> webPackage, string[] paramArray)
        {
            string[] parsedService = { "","","" };

            /*
             * From the webPackage, we need:
             * url= "http://www.dneonline.com/XXXX.asmx"
             * action= "http://tempuri.org/XXX"
             * request= "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body ><Add xmlns = ""http://tempuri.org/""> <intA>1</intA><intB>2</intB></Add></soap:Body></soap:Envelope>"
             */
            foreach (var items in webPackage)
            {
                string WebPackageMethodName = items.Item1;

                if (string.Compare(selectedServiceMethod, WebPackageMethodName) == 0)
                {
                    parsedService[0] = FindURL(items.Item2);                            //Pull the host and service from the header request
                    parsedService[1] = FindAction(items.Item2);                         //Pull the SOAPaction from the header request
                    parsedService[2] = FindRequest(items.Item2, paramArray);            //Pull the xml request portion from the soap:envelope
                    break;
                }
            }
            return parsedService;
        }


        /*
        *   METHOD        : FindURL
        *   DESCRIPTION   : Finds the host address, and target service details from the complete headerRequest string
        *   PARAMETERS    : string headerRequest : Contains the entire header for making the SOAP request
        *   RETURNS       : string : Contains the Host and POST details of the selected service
        */
        private string FindURL(string headerRequest)
        {
            string post = string.Empty;
            string host = string.Empty;

            headerRequest = Regex.Replace(headerRequest, @"\t|\r", "");
            string[] headerRequestByLine = headerRequest.Split('\n');


            //Pull the host, and service information from the header request string
            int itemsFound = 0;
            foreach (string line in headerRequestByLine)
            {

                if (itemsFound == 2)
                    break;
                if (line.Contains("Host:"))
                {
                    string[] hostLine = line.Split(' ');
                    if (!hostLine[1].Contains("http://"))
                        host += "http://";
                    host += hostLine[1];
                    itemsFound++;
                }
                if (line.Contains("POST"))
                {
                    string[] postLine = line.Split(' ');
                    post = postLine[1];
                    itemsFound++;
                }
            }

            /* Example: 
            * host = /DictService/DictService.asmx
            * post = http://services.aonaware.com
            */
            return host + post;
        }


        /*
        *   METHOD        : FindAction
        *   DESCRIPTION   : Pulls the SOAP action string from the complete header request
        *   PARAMETERS    : string headerRequest : Contains the entire header for making the SOAP request             
        *   RETURNS       : string : Contains the SOAP action of the selected service
        */
        private string FindAction(string headerRequest)
        {
            string action = string.Empty;
            headerRequest = Regex.Replace(headerRequest, @"\t|\r", "");
            string[] headerRequestByLine = headerRequest.Split('\n');


            //Scan the header request argument, and look for the SOAP action sub string
            foreach (string line in headerRequestByLine)
            {
                if (line.Contains("SOAPAction"))
                {
                    string[] actionLine = line.Split(' ');
                    action = actionLine[1];
                    break;
                }
            }

            //Example: SOAPAction: \"http://services.aonaware.com/webservices/Define\"
            return action;
        }


        /*
        *   METHOD        : FindRequest
        *   DESCRIPTION   : Finds the XML details of the SOAP envelope
        *   PARAMETERS    : string headerRequest : Contains the entire header for making the SOAP request           
        *   RETURNS       : string : Contains the XML defining the SOAP request specifics
        */
        private string FindRequest(string headerRequest, string[] paramArray)
        {
            string request = string.Empty;
            bool soapBody = false;
            headerRequest = Regex.Replace(headerRequest, @"\t|\r", "");
            string[] headerRequestByLine = headerRequest.Split('\n');


            //Find the inner contents of the soap envelope tags
            for(int i = 0; i < headerRequestByLine.Count() ; i++)
            {
                int o = headerRequestByLine.Count();
                if (headerRequestByLine[i].Contains("<soap:Envelope"))
                {
                    soapBody = true;
                }
                else if (headerRequestByLine[i].Contains("</soap:Envelope>"))
                {
                    soapBody = false;
                    request += headerRequestByLine[i];
                }

                if (soapBody)
                {
                    request += headerRequestByLine[i];
                }
            }

            // convert the XML string to Xml NodeList
            XmlDocument convertedToXml = new XmlDocument();
            convertedToXml.LoadXml(request);
            XmlNodeList node = convertedToXml.ChildNodes;

            // Start the node in body
            var bodyNode = node.Item(0).FirstChild.FirstChild;
            int j = 0;
            foreach (XmlNode child in bodyNode)
            {
                child.InnerText = paramArray[j++];
            }
            request = convertedToXml.OuterXml;
            return request;
        }


        /*
        *   METHOD        : DetermineParamAmount
        *   DESCRIPTION   : Used to count the number of parameters specified by the service
        *   PARAMETERS    : string selectedServiceMethod: Users selected service
        *                   List<Tuple<string, string, string>> webPackage : Details of the selected service
        *   RETURNS       : string : Count of how many parameters are required by the service
        */
        public Tuple<int,string> DetermineParamAmount(string selectedServiceMethod, List<Tuple<string, string, string>> webPackage)
        {
            XmlDocument expectedResults = new XmlDocument();
            string expectedString = string.Empty;
            bool xmlSection = false;
            foreach (var items in webPackage)
            {
                string WebPackageExpectedResult = items.Item1;

                if (string.Compare(selectedServiceMethod, WebPackageExpectedResult) == 0)
                {
                    string[] expectedResultsString = items.Item2.Split('\n');
                    foreach (string line in expectedResultsString)
                    {
                        // We've found the format that we are expecting for the results.
                        // Lets store that
                        if (line.Contains("<?xml version="))
                        {
                            xmlSection = true;
                        }
                        if (xmlSection)
                        {
                            expectedString += line;
                        }
                    }
                    break;
                }
            }
            expectedString = Regex.Replace(expectedString, @"\t|\n|\r", "");
            expectedResults.LoadXml(expectedString);
            XmlNodeList node = expectedResults.ChildNodes;
            var rootNode = node.Item(1);

            //  This is soap:Envelope/soap:Body/
            XmlNode resultNode = rootNode.FirstChild.FirstChild;
            int amountOfParamsNeeded = resultNode.ChildNodes.Count;

            int j = 0;
            string dataTypeOfParameters = string.Empty;
            foreach(XmlNode child in resultNode)
            {
                if (j >= 1)
                {
                    dataTypeOfParameters += ";";
                }
                dataTypeOfParameters += child.InnerText;
                j++;
            }
            


            return (Tuple.Create(amountOfParamsNeeded, dataTypeOfParameters));
        }


        // Clean up the list<tuple> thats holding our element names and their values.
        // This will get rid of all the junk elements that are parents and have no value.
        //  We dont want to show these
        public void RemoveUselessElements(List<Tuple<string, string>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i].Item1.Count() > 1) && (list[i + 1].Item2.Count() < 1))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
