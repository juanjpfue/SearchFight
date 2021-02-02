using System;
using System.IO;
using System.Net;
using System.Web;

namespace ClientCall
{
    public class HttpClient
    {


        public static string GET(string url, string header, string headervalue)

        {


            string result = "";
            WebRequest oRequest = WebRequest.Create(url);

            if (header != null && headervalue != null)
                oRequest.Headers[header] = headervalue;


            WebResponse oResponse = oRequest.GetResponse();
            using (var oSR = new StreamReader(oResponse.GetResponseStream()))
            {
                result = oSR.ReadToEnd().Trim();
            }
            return result;
        }

    }
}
