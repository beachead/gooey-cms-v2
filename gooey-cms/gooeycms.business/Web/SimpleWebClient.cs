using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Gooeycms.Business.Web
{
    public class SimpleWebClient
    {
        public static byte[] GetResponse(Uri url)
        {
            byte[] buffer = new byte[4096];
            byte[] data;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, count);
                        } while (count != 0);

                        data = ms.ToArray();
                    }
                }
            }

            buffer = null;
            return data;
        }
    }
}
