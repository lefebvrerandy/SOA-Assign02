using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using EventLogger;

namespace SOA_Assign02
{
    class FileManager
    {
        // This class variable will be used to hold all the web services.
        //  This class variable can be accessed by anyone to view and edit the web services
        public List<Tuple<string, string, string>> configList;

        public FileManager(string fileName)
        {
            LoadConfigurationFile(fileName);
        }




        public List<Tuple<string, string, string>> LoadConfigurationFile(string fileName)
        {

            // Check if configuration file exist
            try
            {
                StreamReader file = new StreamReader(fileName);
            }
            catch (Exception error)
            {
                // File does not exist. Return empty sorted list
                Logger.RecordError(error.Message);
                return new List<Tuple<string, string, string>>();
            }

            // Temp list<tuple> to hold our web services
            List<Tuple<string, string, string>> webServicePackage = new List<Tuple<string, string, string>>();

            string name = "";
            string request = "";
            string response = "";

            // Open up the file file and store the web services into the List<tuple>
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

            // Store the values into the list<tuple>
            webServicePackage.Add(Tuple.Create<string, string, string>(name, request, response));
            configList = webServicePackage;

            //DEBUG
            //int i = 0;
            //while (i < webServicePackage.Count)
            //{
            //    txt_output.Text = webServicePackage[i].Item1 + Environment.NewLine + Environment.NewLine +
            //        webServicePackage[i].Item2 + Environment.NewLine + Environment.NewLine +
            //        webServicePackage[i].Item3 + Environment.NewLine;
            //    i++;
            //}
            //i = 0;
            ////END DEBUG
            //while (i < webServicePackage.Count)
            //{
            //    cb_WebServiceList.Items.Add(webServicePackage[i].Item1.ToString());
            //    i++;
            //}

            return webServicePackage;
        }

        public string[] ParseWebService(string selectedServiceMethod, List<Tuple<string,string,string>> webPackage, string[] paramArray)
        {
            string[] parsedService = { "","","" };

            // From the webPackage, I need:
            //  url= "http://www.dneonline.com/XXXX.asmx"
            //  action= "http://tempuri.org/XXX"
            //  request= "<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body ><Add xmlns = ""http://tempuri.org/""> <intA>1</intA><intB>2</intB></Add></soap:Body></soap:Envelope>"

            foreach (var items in webPackage)
            {
                string WebPackageMethodName = items.Item1;

                if (string.Compare(selectedServiceMethod, WebPackageMethodName) == 0)
                {
                    parsedService[0] = FindURL(items.Item2);
                    parsedService[1] = FindAction(items.Item2);
                    parsedService[2] = FindRequest(items.Item2, paramArray);
                    break;
                }
            }


            return parsedService;
        }

        private string FindURL(string headerRequest)
        {
            string url = string.Empty;
            string post = string.Empty;
            string host = string.Empty;

            headerRequest = Regex.Replace(headerRequest, @"\t|\r", "");
            string[] headerRequestByLine = headerRequest.Split('\n');


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

            return host + post;
        }

        private string FindAction(string headerRequest)
        {
            string action = string.Empty;
            headerRequest = Regex.Replace(headerRequest, @"\t|\r", "");
            string[] headerRequestByLine = headerRequest.Split('\n');

            foreach (string line in headerRequestByLine)
            {
                if (line.Contains("SOAPAction"))
                {
                    string[] actionLine = line.Split(' ');
                    action = actionLine[1];
                    break;
                }
            }
            return action;
        }

        private string FindRequest(string headerRequest, string[] paramArray)
        {
            string request = string.Empty;
            bool soapBody = false;
            headerRequest = Regex.Replace(headerRequest, @"\t|\r", "");
            string[] headerRequestByLine = headerRequest.Split('\n');

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

        public int DetermineParamAmount(string selectedServiceMethod, List<Tuple<string, string, string>> webPackage)
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
            expectedString = Regex.Replace(expectedString, @"\t|\n|\r", ""); ;
            expectedResults.LoadXml(expectedString);
            XmlNodeList node = expectedResults.ChildNodes;
            var rootNode = node.Item(1);

            //  This is soap:Envelope/soap:Body/
            var resultNode = rootNode.FirstChild.FirstChild;
            int amountOfParamsNeeded = resultNode.ChildNodes.Count;
            
            


            return amountOfParamsNeeded;
        }

        //public int DetermineParam(string selectedServiceMethod, List<Tuple<string, string, string>> webPackage)
        //{
        //    int amoutOfParams = 0;
        //    bool xmlSection = false;

        //    foreach (var items in webPackage)
        //    {
        //        string WebPackageExpectedResult = items.Item1;

        //        if (string.Compare(selectedServiceMethod, WebPackageExpectedResult) == 0)
        //        {
        //            string[] expectedResultsString = items.Item3.Split('\n');
        //            foreach (string line in expectedResultsString)
        //            {
        //                // We've found the format that we are expecting for the results.
        //                // Lets store that
        //                if (line.Contains("<?xml version="))
        //                {
        //                    xmlSection = true;
        //                }
        //                if (xmlSection)
        //                {
        //                    expectedString += line;
        //                }
        //            }
        //            break;
        //        }
        //    }

        //    return amoutOfParams;
        //}
    }
}
