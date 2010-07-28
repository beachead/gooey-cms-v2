using System;
using System.Collections.Generic;
using Gooeycms.Business.Compression;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Images
{
    public class ImageManager
    {
        private static ImageManager instance = new ImageManager();
        private ImageManager() { }
        public static ImageManager Instance
        { 
            get { return ImageManager.instance; } 
        }

        public void AddImage(String filename, String contentType, byte [] contents)
        {
            AddImage(CurrentSite.Guid, filename, contentType, contents);
        }

        /// <summary>
        /// Adds a new image to the site
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="contents"></param>
        public void AddImage(Data.Guid siteGuid, String filename, String contentType, byte [] contents)
        {
            IList<StorageFile> images = new List<StorageFile>();
            if (filename.ToLower().EndsWith("zip"))
            {
                ZipHandler handler = new ZipHandler(contents);
                images = handler.Decompress();
            }
            else
            {
                StorageFile file = new StorageFile();
                file.Filename = filename;
                file.Data = contents;
                file.ContentType = contentType;
                images.Add(file);
            }

            IStorageClient client = StorageHelper.GetStorageClient();
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesDirectoryKey, siteGuid.Value);
            foreach (StorageFile file in images)
            {
                client.Save(imageDirectory, file.Filename, file.Data, Permissions.Public);
            }
        }

        /// <summary>
        /// Retrieves the full paths to the images
        /// </summary>
        /// <returns></returns>
        public IList<StorageFile> GetAllImagePaths()
        {
            return GetAllImagePaths(CurrentSite.Guid);
        }

        /// <summary>
        /// Retrieves the full paths to the images
        /// </summary>
        /// <returns></returns>
        public IList<StorageFile> GetAllImagePaths(Data.Guid siteGuid)
        {
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesDirectoryKey, siteGuid.Value);

            IStorageClient client = StorageHelper.GetStorageClient();
            return client.List(imageDirectory);
        }
    }
}
