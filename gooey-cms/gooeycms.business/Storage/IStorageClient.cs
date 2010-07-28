using System;
using System.Collections.Generic;

namespace Gooeycms.Business.Storage
{
    public enum Permissions
    {
        Private,
        Public
    }

    public interface IStorageClient
    {
        void Save(String directory, String filename, byte[] data, Permissions permissions);
        void Save(String directory, String filename, String contents, Permissions permissions);

        void Delete(String directory, String filename);

        byte [] Open(String directory, String filename);
        String OpenAsString(String directory, String filename);

        IList<StorageFile> List(String directory);
    }
}
