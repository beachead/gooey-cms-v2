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
        StorageFile GetFile(string directory, string actualfilename);

        IList<StorageFile> List(String directory);

        StorageFile GetInfo(String directory, String filename);

        StorageContainer GetContainerInfo(String container);

        void AddMetadata(String key, String value);
        void SetMetadata(string directory, string actualFilename);
    }
}
