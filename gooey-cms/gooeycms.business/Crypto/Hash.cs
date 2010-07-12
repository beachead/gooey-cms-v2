using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Gooeycms.Business.Crypto
{
    public static class Hash
    {
        /// <summary>
        /// Generates a MD5 hash of the given string.
        /// 
        /// Shamelessly taken and used from:
        /// http://blog.stevex.net/c-code-snippet-creating-an-md5-hash-string/
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string MD5(string str)
        {    
            // First we need to convert the string into bytes, which    
            // means using a text encoder.    
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();   
            
            // Create a buffer large enough to hold the string    
            byte[] unicodeText = new byte[str.Length * 2];    
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true); 
   
            // Now that we have a byte array we can ask the CSP to hash it    
            MD5 md5 = new MD5CryptoServiceProvider();    
            byte[] result = md5.ComputeHash(unicodeText); 
   
            // Build the final string by converting each byte   
            // into hex and appending it to a StringBuilder   
            StringBuilder sb = new StringBuilder();    
            for (int i=0;i<result.Length;i++)    
            {           
                sb.Append(result[i].ToString("X2"));    
            }   
 
            // And return it    
            return sb.ToString();
        }
    }
}
