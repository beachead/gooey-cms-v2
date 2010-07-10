using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Storage
{
    public interface IStorageClient
    {
        void Save(String directory, String filename, byte[] data);
        void Save(String directory, String filename, String contents);

        void Delete(String directory, String filename);

        byte [] Open(String directory, String filename);
        String OpenAsString(String directory, String filename);
    }
}
