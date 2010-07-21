using System;
using System.IO;
using System.Reflection;

namespace Gooeycms.Business.Util
{
    public class EmbeddedResource
    {
        /// <summary>
        /// Reads an embedded resource from the currently executing assembly
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static String Read(String resource)
        {
            String result = String.Empty;

            Assembly assem = Assembly.GetExecutingAssembly();
            using (TextReader reader = new StreamReader(assem.GetManifestResourceStream(resource)))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
