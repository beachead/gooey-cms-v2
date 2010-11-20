using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Gooeycms.Business.Compression;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Util;
using Kaliko.ImageLibrary;
using System.Text;
using System.Text.RegularExpressions;
using Gooeycms.Extensions;

namespace Gooeycms.Business.Images
{
    public class ImageManager
    {
        public static List<String> ValidImageExtensions = new List<String>() { "png", "gif", "jpg", "jpeg" };
        public static Boolean IsValidImageType(String imagename)
        {
            Boolean result = false;
            if (imagename != null)
                result = (ValidImageExtensions.Exists(d => imagename.ToLower().EndsWith(d)));

            return result;
        }


        private static ImageManager instance = new ImageManager();
        private ImageManager() { }
        public static ImageManager Instance
        { 
            get { return ImageManager.instance; } 
        }

        public IList<StorageFile> AddImage(String folder, String filename, String contentType, byte[] contents)
        {
            return AddImage(CurrentSite.Guid, folder, filename, contentType, contents);
        }

        /// <summary>
        /// Adds a new image to the site
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="contents"></param>
        public IList<StorageFile> AddImage(Data.Guid siteGuid, String folder, String filename, String contentType, byte[] contents)
        {
            IList<StorageFile> images = new List<StorageFile>();
            if (filename.ToLower().EndsWith("zip"))
            {
                ZipHandler handler = new ZipHandler(contents);
                images = handler.Decompress();
                
                //Make sure that the images are valid, removing any invalid ones
                for (int i = images.Count - 1; i != 0; i--)
                {
                    StorageFile file = images[i];
                    if (!IsValidImageType(file.Filename))
                        images.RemoveAt(i);
                }
            }
            else
            {
                if (!IsValidImageType(filename))
                    throw new ArgumentException("The specified image is not a supported image type. The filename must end with one of the approved extensions: " + ValidImageExtensions.AsString(",") + ". Filename:" + filename);
                StorageFile file = new StorageFile();
                file.Filename = filename;
                file.Data = contents;
                file.ContentType = contentType;
                images.Add(file);
            }

            //Generate thumbnails
            IList<StorageFile> results = new List<StorageFile>(images);
            foreach(StorageFile file in images) {
                if ((file.Filename.Length > 0) && (file.Data.Length > 0))
                    GenerateThumbnail(file, results);
            }
            images = null;

            IStorageClient client = StorageHelper.GetStorageClient();
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);
            foreach (StorageFile file in results)
            {
                if ((file.Filename.Length > 0) && (file.Data.Length > 0))
                {
                    client.Save(imageDirectory, folder, file.Filename, file.Data, Permissions.Public);
                }
            }


            return GetAllImagePaths(siteGuid,folder);
        }

        private void GenerateThumbnail(StorageFile file, IList<StorageFile> results)
        {
            if (file.Data.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(file.Data))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        try
                        {
                            KalikoImage image = new KalikoImage(stream);
                            int width = 60;
                            int height = 60;

                            image.BackgroundColor = Color.White;
                            KalikoImage thumb = image.GetThumbnailImage(width, height, ThumbnailMethod.Pad);
                            thumb.SaveJpg(output, 75);

                            FileInfo info = new FileInfo(file.Filename);

                            StorageFile thumbnail = new StorageFile();
                            thumbnail.Filename = info.Name.Replace(info.Extension, "") + "-thumb.jpg";
                            thumbnail.Data = output.ToArray();

                            results.Add(thumbnail);
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException("The uploaded content is not supported. Must be a valid image type (png,jpeg or gif)");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the full paths to the images
        /// </summary>
        /// <returns></returns>
        public IList<StorageFile> GetAllImagePaths(String folder)
        {
            return GetAllImagePaths(CurrentSite.Guid, folder);
        }

        /// <summary>
        /// Retrieves the full paths to the images
        /// </summary>
        /// <returns></returns>
        public IList<StorageFile> GetAllImagePaths(Data.Guid siteGuid, String folder)
        {
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);

            IStorageClient client = StorageHelper.GetStorageClient();
            IList<StorageFile> allimages = client.List(imageDirectory, folder);

            IList<StorageFile> results = new List<StorageFile>();

            //Filter out the thumbnails
            foreach (StorageFile temp in allimages)
            {
                if (!temp.Filename.Contains("-thumb.jpg"))
                    results.Add(temp);
            }

            return results;
        }

        /// <summary>
        /// Retrieves the full paths to the images
        /// </summary>
        /// <returns></returns>
        public IList<StorageFile> GetImagesWithData(Data.Guid siteGuid, String folder)
        {
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);

            IStorageClient client = StorageHelper.GetStorageClient();
            IList<StorageFile> allimages = client.List(imageDirectory, folder);

            IList<StorageFile> results = new List<StorageFile>();

            //Filter out the thumbnails
            foreach (StorageFile temp in allimages)
            {
                if (!temp.Filename.Contains("-thumb.jpg"))
                {
                    temp.Data = client.Open(imageDirectory, folder, temp.Filename);
                    results.Add(temp);
                }
            }


            return results;
        }

        internal void DeleteAllImages(Data.Guid siteGuid, string folder)
        {
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);
            IStorageClient client = StorageHelper.GetStorageClient();
            if (client.ContainsSnapshots(imageDirectory,folder))
                throw new ArgumentException("Can not delete this folder because snapshots exist for the images");

            client.Delete(imageDirectory, folder);
        }

        internal Boolean ContainsSnapshots(Data.Guid siteGuid, string folder)
        {
            String imageDirectory = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);
            IStorageClient client = StorageHelper.GetStorageClient();

            return (client.ContainsSnapshots(imageDirectory, folder));
        }
    }
}
