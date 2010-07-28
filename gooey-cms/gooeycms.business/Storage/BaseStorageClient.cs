﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gooeycms.Business.Storage
{
    public abstract class BaseStorageClient : IStorageClient
    {
        public abstract void Save(String directory, String filename, byte[] data, Permissions permissions);
        public abstract void Delete(String directory, String filename);
        public abstract byte[] Open(String directory, String filename);
        public abstract IList<StorageFile> List(String directory);

        public void Save(String directory, String filename, String contents, Permissions permissions)
        {
            if (contents == null)
                contents = "";
            this.Save(directory, filename, Encoding.UTF8.GetBytes(contents), permissions);
        }

        public String OpenAsString(String directory, String filename)
        {
            byte[] data = this.Open(directory, filename);
            return Encoding.UTF8.GetString(data);
        }
    }
}
