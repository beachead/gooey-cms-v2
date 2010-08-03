using System.Collections.Generic;
using System.IO;
using Gooeycms.Business.Storage;
using Ionic.Zip;

namespace Gooeycms.Business.Compression
{
    public class ZipHandler
    {
        private byte[] compressedFile;
        private Stream zipstream = null;

        public ZipHandler(Stream zipstream)
        {
            this.zipstream = zipstream;
        }

        public ZipHandler(byte [] compressedFile)
        {
            this.compressedFile = compressedFile;
        }

        public IList<StorageFile> Decompress()
        {
            IList<StorageFile> results = new List<StorageFile>();

            ZipFile file;
            if (zipstream == null)
                file = ZipFile.Read(compressedFile);
            else
                file = ZipFile.Read(zipstream);

            using (file)
            {
                file.FlattenFoldersOnExtract = true;
                foreach (ZipEntry entry in file)
                {
                    StorageFile result = new StorageFile();
                    
                    //flatten all directories, since we don't currently support directories
                    int pos = entry.FileName.LastIndexOf("/");
                    if (pos >= 0)
                        result.Filename = entry.FileName.Substring(pos + 1);
                    else
                        result.Filename = entry.FileName;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        entry.Extract(ms);
                        result.Data = ms.ToArray();
                    }

                    results.Add(result);
                }
            }

            return results;
        }
    }
}
