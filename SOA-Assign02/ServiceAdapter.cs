using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Diagnostics;
using System.Dynamic;

namespace SOA_Assign02
{
    public class ServiceAdapter
    {


        /*
        *   METHOD        : InsertSoapEnvelopeIntoWebRequest
        *   DESCRIPTION   : Calls the selected web service, and returns its response
        *   PARAMETERS    : string url : Url of the service
        *                   string action : Service to call at the web server
        *                   string soapEnvelope : Complete soap request envelope
        *   RETURNS       : List<Tuple<string, string>> : Contains the web servers response to our request
        *   
        *   Note: Taken from KBBWrite. (2011). Client to send SOAP request and receive response. Retrieved from https://stackoverflow.com/questions/4791794/client-to-send-soap-request-and-receive-response
        */
        public static List<Tuple<string, string>> CallWebService(string url, string action, string soapEnvelope)
        {
            //var _url = "http://www.dneonline.com/calculator.asmx";
            //var _action = "http://tempuri.org/Add";
            var _url = url;
            var _action = action;

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(soapEnvelope);
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult = string.Empty;
            List<Tuple<string, string>> resultsList = new List<Tuple<string, string>>();
            bool unableToConnect = false;
            
            try
            {
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        unableToConnect = false;
                    }
                }
            }
            catch(Exception e)
            {
                soapResult = e.Message;
                unableToConnect = true;
            }

            if (!unableToConnect)
            {
                XmlDocument results = new XmlDocument();
                results.LoadXml(soapResult);

                resultsList = GetListOfElements(results);
            }

            return resultsList;
        }

        /*
        *   METHOD        : InsertSoapEnvelopeIntoWebRequest
        *   DESCRIPTION   : Creates the HttpWebRequest object and adds the HTTP header, contentType, action, and url of the selected service
        *   PARAMETERS    : string url : Url of the selected service
        *                   string action : Requested soap action
        *   RETURNS       : HttpWebRequest : Object containing connection details for the service
        */
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            // Header information
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }


        /*
        *   METHOD        : InsertSoapEnvelopeIntoWebRequest
        *   DESCRIPTION   : Converts the soap request string, into an XML object
        *   PARAMETERS    : string soapEnvelope : soap request in string form             
        *   RETURNS       : XmlDocument : XML object of the soap string
        */
        private static XmlDocument CreateSoapEnvelope(string soapEnvelope)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();

            soapEnvelopeDocument.LoadXml(soapEnvelope);

            return soapEnvelopeDocument;
        }


        /*
        *   METHOD        : InsertSoapEnvelopeIntoWebRequest
        *   DESCRIPTION   : Packages the soap envelope in to the web request object
        *   PARAMETERS    : XmlDocument soapEnvelopeXml : document containing the soap request details
        *                   HttpWebRequest webRequest : Web request object
        *   RETURNS       : DEBUG : 
        */
        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        public static List<Tuple<string,string>> GetListOfElements(XmlDocument results)
        {
            List<Tuple<string, string>> elementsAndValue = new List<Tuple<string, string>>();
            XmlTextReader rdr = new XmlTextReader(new StringReader(results.OuterXml));
            while (rdr.Read())
            {
                if ((rdr.NodeType == XmlNodeType.Element) || (rdr.HasValue))
                {
                    elementsAndValue.Add(Tuple.Create(rdr.LocalName, rdr.Value));
                }
            }

            return elementsAndValue;
        }
    }
}
