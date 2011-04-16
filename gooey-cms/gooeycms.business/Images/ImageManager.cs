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
using Gooeycms.Data.Model.Page;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business.Images
{
    public class ImageManager
    {
        private static Regex BasicImagePattern = new Regex(@"[\w\d-_]+\.\w{3}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static List<String> ValidImageExtensions = new List<String>() { "png", "gif", "jpg", "jpeg", "swf", "bmp" };
        public static IDictionary<String, String> ImageMimeTypes = new Dictionary<String, String>();

        static ImageManager()
        {
            ImageMimeTypes.Add(".png", "image/png");
            ImageMimeTypes.Add(".gif", "image/gif");
            ImageMimeTypes.Add(".jpg", "image/jpeg");
            ImageMimeTypes.Add(".jpeg", "image/jpeg");
            ImageMimeTypes.Add(".bmp", "image/bmp");
            ImageMimeTypes.Add(".swf", "application/x-shockwave-flash");
        }

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

            IStorageClient client = StorageHelper.GetStorageClient();
            String container = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);
            foreach (StorageFile file in images)
            {
                if ((file.Filename.Length > 0) && (file.Data.Length > 0))
                {
                    client.Save(container, folder, file.Filename, file.Data, Permissions.Public);
                }
            }

            IList<StorageFile> results = new List<StorageFile>();

            //Add any new images to the database
            SaveImagesToDatabase(siteGuid, client, container, folder, images, results);

            return results;
        }

        public static void SaveImagesToDatabase(Data.Guid siteGuid, IStorageClient client, String container, String folder, IList<StorageFile> images, IList<StorageFile> results)
        {
            CmsImageDao dao = new CmsImageDao();
            using (Transaction tx = new Transaction())
            {
                foreach (StorageFile file in images)
                {
                    StorageFile actualFile;

                    if (results != null)
                        actualFile = client.GetInfo(container, folder, file.Filename);
                    else
                        actualFile = file;

                    if (actualFile.Exists())
                    {
                        CmsImage temp = dao.FindByUrl(actualFile.Url);
                        if (temp == null)
                        {
                            FileInfo info = new FileInfo(actualFile.Filename);

                            String mimetype = "image/png";
                            if (ImageMimeTypes.ContainsKey(info.Extension))
                                mimetype = ImageMimeTypes[info.Extension];

                            temp = new CmsImage();
                            temp.CloudUrl = actualFile.Url;
                            temp.ContentType = mimetype;
                            temp.Created = DateTime.Now;
                            temp.Directory = folder;
                            temp.Filename = actualFile.Filename;
                            temp.Guid = System.Guid.NewGuid().ToString();
                            temp.SubscriptionId = siteGuid.Value;
                            temp.Length = actualFile.Size;

                            dao.Save<CmsImage>(temp);
                        }

                        if (results != null)
                            results.Add(actualFile);
                    }
                }
                tx.Commit();
            }
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

        public CmsImage GetImageByGuid(Data.Guid siteGuid, Data.Guid guid)
        {
            return GetImageByGuid(siteGuid, guid, false);
        }

        public CmsImage GetImageByGuid(Data.Guid siteGuid, Data.Guid guid, bool loadImageData)
        {
            CmsImageDao dao = new CmsImageDao();
            CmsImage image = dao.FindBySiteAndGuid(siteGuid, guid);

            if (image != null)
                SetImageData(siteGuid, image);

            return image;
        }

        public CmsImage GetImageByNameAndDirectory(Data.Guid siteGuid, String filename, String directory, Boolean includeData)
        {
            CmsImageDao dao = new CmsImageDao();
            CmsImage image = dao.FindByNameAndDirectory(siteGuid, filename, directory);

            if ((image != null) && (includeData))
                SetImageData(siteGuid, image);

            return image;
        }

        private void SetImageData(Data.Guid siteGuid, CmsImage image)
        {
            String container = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);

            IStorageClient client = StorageHelper.GetStorageClient();
            byte[] data = client.Open(container, image.Directory, image.Filename);
            image.Data = data;
        }

        /// <summary>
        /// This method will check if the image found in the markup are actually
        /// already stored in the theme. If not, it will then check the page
        /// image area and copy the images from the page to the theme area.
        /// </summary>
        /// <param name="themeGuid">The theme to validate against</param>
        /// <param name="moveImagesToTheme"></param>
        /// <returns>List of invalid images which could not be found or moved</returns>
        public IList<String> ValidateAndMove(String content, Data.Guid siteGuid, Data.Guid themeGuid, bool moveImagesToTheme)
        {
            IList<String> missingImages = new List<String>();

            MatchCollection matches = BasicImagePattern.Matches(content);
            foreach (Match match in matches)
            {
                String imagename = match.Value;
                Boolean isImageExist = (ImageManager.Instance.GetImageByNameAndDirectory(siteGuid, imagename, themeGuid.Value, false) != null);

                //If the image doesn't exist in the themes directory, check the page directory
                if (!isImageExist)
                {
                    CmsImage image = ImageManager.Instance.GetImageByNameAndDirectory(siteGuid, imagename, null, true);

                    //Found the image in the page directory, copy it to the themes
                    if (image != null)
                    {
                        ImageManager.Instance.AddImage(themeGuid.Value, imagename, image.ContentType, image.Data);
                    }
                    else
                    {
                        missingImages.Add(imagename);
                    }
                }
            }

            return missingImages;
        }

        public void DeleteImage(Data.Guid siteGuid, Data.Guid imageGuid)
        {
            //Find the image based upon the guid
            CmsImageDao dao = new CmsImageDao();
            CmsImage image = dao.FindBySiteAndGuid(siteGuid, imageGuid);
            if (image != null)
            {
                String container = SiteHelper.GetStorageKey(SiteHelper.ImagesContainerKey, siteGuid.Value);

                //Attempt to delete from cloud storage first
                IStorageClient client = StorageHelper.GetStorageClient();
                if (client.ContainsSnapshots(container, image.Directory, image.Filename))
                    throw new ArgumentException("Unable to delete the image because it is currently being used by a site package");

                try
                {
                    client.Delete(container, image.Directory, image.Filename);
                    using (Transaction tx = new Transaction())
                    {
                        dao.Delete<CmsImage>(image);
                        tx.Commit();
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException("There was a problem deleting the image: " + e.Message);
                }
            }
        }
    }
}
