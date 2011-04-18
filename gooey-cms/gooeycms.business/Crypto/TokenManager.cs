using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model;
using Beachead.Persistence.Hibernate;
using Gooeycms.Extensions;

namespace Gooeycms.Business.Crypto
{
    public class TokenManager
    {
        private const String Salt = "thisismysaltforgooeycmstokens";
        private const Int32 DefaultMaxUses = 1;

        public static String Issue(String data, TimeSpan validFor, Int32 maxUses)
        {
            if (data.Length > 90)
                throw new ArgumentException("The data for the token cannot be longer than 90 characters");

            DateTime issued = UtcDateTime.Now;
            DateTime expires = issued.Add(validFor);

            data = Guid.NewGuid().ToString() + "," + data + "," + expires.Ticks;
            
            TextEncryption crypto = new TextEncryption(GooeyConfigManager.TokenEncyrptionKey);
            String result = crypto.Encrypt(data);

            SecurityToken token = new SecurityToken();
            token.Token = result;
            token.Issued = issued;
            token.Expires = expires;
            token.Uses = 0;
            token.MaxUses = maxUses;

            SaveToDatabase(token);

            return result;
        }

        public static String Issue(String data, TimeSpan validFor)
        {
            return Issue(data, validFor, DefaultMaxUses);
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
                        //DateTime dt = DateTime.Parse(timestamp);
                        //if (dt > DateTime.Now)
                        //{
                            //check the database to make sure this token hasn't been used
                            SecurityToken temp = GetFromDatabase(encryptedData);
                            if (temp != null)
                            {
                                int uses = temp.Uses + 1;
                                if (uses <= temp.MaxUses)
                                {
                                    //Update the use count in the database
                                    temp.Uses = uses;
                                    SaveToDatabase(temp);

                                    result = true;
                                }
                            }
                        //}
                    }
                }
            }
            catch (Exception) { }

            return result;
        }

        private static SecurityToken GetFromDatabase(String token)
        {
            SecurityTokenDao dao = new SecurityTokenDao();
            return dao.FindByToken(token);
        }

        private static void SaveToDatabase(SecurityToken token)
        {
            SecurityTokenDao dao = new SecurityTokenDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<SecurityToken>(token);
                tx.Commit();
            }
        }

        internal static void Invalidate(String token)
        {
            SecurityToken dbToken = GetFromDatabase(token);
            dbToken.Expires = UtcDateTime.Now;
            dbToken.MaxUses = 0;
            dbToken.Uses = Int32.MaxValue;

            SaveToDatabase(dbToken);
        }
    }
}
