using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Web;
using System.IO;

namespace Gooeycms.Business.Web
{
    /// <summary>
    /// Makes a web FORM POST to the specified URL.
    /// Shamelessly taken from: http://www.dijksterhuis.org/simple-class-to-submit-post-a-web-form-from-csharp/
    /// </summary>
    class WebPostRequest
    {
        WebRequest theRequest;
        HttpWebResponse theResponse;
        ArrayList theQueryData;

        public WebPostRequest(string url)
        {
            theRequest = WebRequest.Create(url);
            theRequest.Method = "POST";
            theQueryData = new ArrayList();
        }

        public void Add(string key, string value)
        {
            theQueryData.Add(String.Format("{0}={1}", key, HttpUtility.UrlEncode(value)));
        }

        public string GetResponse()
        {
            // Set the encoding type
            theRequest.ContentType = "application/x-www-form-urlencoded";

            // Build a string containing all the parameters
            string Parameters = String.Join("&", (String[])theQueryData.ToArray(typeof(string)));
            theRequest.ContentLength = Parameters.Length;

            // We write the parameters into the request
            using (StreamWriter sw = new StreamWriter(theRequest.GetRequestStream()))
            {
                sw.Write(Parameters);
            }

            String result;
            // Execute the query
            theResponse = (HttpWebResponse)theRequest.GetResponse();
            using (StreamReader sr = new StreamReader(theResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

    }

}
