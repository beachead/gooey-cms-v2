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
                foreach (ZipEntry entry in file)
                {
                    StorageFile result = new StorageFile();
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
