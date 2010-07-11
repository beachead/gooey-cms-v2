using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using Microsoft.WindowsAzure.StorageClient;

namespace Gooeycms.Business.Storage
{
    public class AzureBlobStorageClient : BaseStorageClient
    {
        public override void Save(String directory, string filename, byte[] data)
        {
        }

        public override void Delete(String directory, string filename)
        {
            throw new NotImplementedException();
        }

        public override byte[] Open(String directory, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
