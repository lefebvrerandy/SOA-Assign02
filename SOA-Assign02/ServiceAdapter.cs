using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace SOA_Assign02
{
    public class ServiceAdapter
    {
        
        
        public static List<string> CallWebService(string url, string action, string soapEnvelope)
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
            string soapResult = string.Empty;
            List<string> resultsList = new List<string>();
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
                soapResult = e.Message.ToString();
                unableToConnect = true;
            }

            if (!unableToConnect)
            {
                ///DEBUGGING
                XmlDocument results = new XmlDocument();
                results.LoadXml(soapResult);
                //XmlNodeList nodes = expectedResults.ChildNodes;
                //XmlNodeList nodes = expectedResults.DocumentElement.SelectNodes(@"/soap:Envelope/soap:Body");
                XmlNodeList node = results.ChildNodes;
                // Start the node in body
                var bodyNode = node.Item(1).FirstChild;

                

                // Find the lowest node
                bool lowestNodeBool = false;
                var lowestNode = bodyNode;
                var lowestNodeTest = bodyNode;
                int answerNodeCount = 0;
                List<XmlNode> listOfNodes = new List<XmlNode>();
                while(!lowestNodeBool)
                {
                    lowestNodeTest = lowestNodeTest.FirstChild;
                    if (lowestNodeTest != null)
                    {
                        var testing = lowestNodeTest.ChildNodes;
                        if (testing.Count > 1)
                        {
                            //Has multiple children. Need to iterate through them all
                            foreach (XmlNode child in lowestNodeTest.ChildNodes)
                            {
                                string textValueFromNode = child.InnerText.ToString();
                                resultsList.Add(textValueFromNode);
                            }
                            break;
                        }
                        else
                        {
                            lowestNode = lowestNodeTest;
                        }
                    }
                    else
                    {
                        lowestNodeBool = true;
                    }
                }
                try
                {
                    resultsList.Add(lowestNode.Value.ToString());
                }
                catch (Exception e)
                { }

                ///END DEBUGGING
            }


            return resultsList;
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
