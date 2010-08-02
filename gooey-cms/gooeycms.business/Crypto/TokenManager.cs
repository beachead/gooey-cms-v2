using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Crypto
{
    public class TokenManager
    {
        private const String Salt = "thisismysaltforgooeycmstokens";
        public static String Issue(String data, TimeSpan validFor)
        {
            data = Salt + "," + data + "," + DateTime.Now.Add(validFor).ToLongTimeString();
            TextEncryption crypto = new TextEncryption(GooeyConfigManager.TokenEncyrptionKey);
            return crypto.Encrypt(data);
        }

        public static Boolean IsValid(String expectedData, String encryptedData)
        {
            Boolean result = false;

            try
            {
                TextEncryption crypto = new TextEncryption(GooeyConfigManager.TokenEncyrptionKey);
                String data = crypto.Decrypt(encryptedData);

                String[] fields = data.Split(',');
                if (fields.Length == 3)
                {
                    String original = fields[1];
                    String timestamp = fields[2];

                    if (expectedData.EqualsCaseInsensitive(original))
                    {
                        DateTime dt = DateTime.Parse(timestamp);
                        if (dt > DateTime.Now)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception) { }

            return result;
        }
    }
}
