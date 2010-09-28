using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Content
{
    public class ContentFileUploadImpl
    {
        private static String[] validExtension = new String[] { "pdf","doc","docx","txt","xls" };

        public static Boolean IsValidFileType(String filename)
        {
            Boolean result = true;
            try
            {
                FileInfo info = new FileInfo(filename);
                String extension = info.Extension.Replace(".","").Trim();
                if (!validExtension.Contains<String>(extension))
                    result = false;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public static String ValidExtensionsString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (String extension in validExtension)
                {
                    builder.Append(extension).Append(" ");
                }
                return builder.ToString();
            }
        }

        internal void Save(Data.Guid siteGuid, byte[] data, string filename, bool overwrite)
        {
            IStorageClient client = StorageHelper.GetStorageClient();

            String container = siteGuid + "-uploads";
            String directory = StorageClientConst.RootFolder;

            //Make sure the file doesn't already exist if we're not allowed to overwrite
            if (!overwrite)
            {
                StorageFile file = client.GetFile(container, directory, filename);
                if (file.Exists())
                    throw new ArgumentException("The filename " + filename + " already exists and may not be used again.");
            }

            client.Save(container, directory, filename, data, Permissions.Private);
        }

        internal void Read(Data.Guid siteGuid, String filename, Stream stream)
        {
            String container = siteGuid + "-uploads";
            String directory = StorageClientConst.RootFolder;

            IStorageClient client = StorageHelper.GetStorageClient();
            client.Read(container, directory, filename, stream);
        }

        internal StorageFile GetInfo(Data.Guid guid, string filename)
        {
            String container = guid + "-uploads";
            String directory = StorageClientConst.RootFolder;

            IStorageClient client = StorageHelper.GetStorageClient();
            return client.GetInfo(container, directory, filename);
        }
    }
}
