using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.IO;

namespace SOA_Assign02
{
    public class ServiceAdapter
    {
        
        
        public static string CallWebService(string url, string action, string soapEnvelope)
        {
            /// These are hardcoded values and will need to be changed
            //var _url = "http://www.dneonline.com/calculator.asmx";
            var _url = url;
            //var _action = "http://tempuri.org/Add";
            var _action = action;

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(soapEnvelope);
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
            }

            //DEBUGGING
            XmlDocument results = new XmlDocument();
            results.LoadXml(soapResult);
            //XmlNodeList nodes = expectedResults.ChildNodes;
            //XmlNodeList nodes = expectedResults.DocumentElement.SelectNodes(@"/soap:Envelope/soap:Body");
            XmlNodeList node = results.ChildNodes;
            var test = node.Item(1);

            // This should give me the answer for calculator
            //  This is soap:Envelope/soap:Body/AddResponse/AddResult
            var test2 = test.FirstChild.FirstChild.FirstChild.FirstChild;
            soapResult = test2.Value.ToString();
            //END DEBUGGING


            return soapResult;
        }

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

        private static XmlDocument CreateSoapEnvelope(string soapEnvelope)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            /// White space may be an issue
            soapEnvelopeDocument.LoadXml(soapEnvelope);

            //soapEnvelopeDocument.LoadXml(@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Body ><Add xmlns = ""http://tempuri.org/""> <intA>1</intA><intB>2</intB></Add></soap:Body></soap:Envelope>");


            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
