using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Content;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Util;
using Gooeycms.Business.Membership;
using System.Web.UI.WebControls;
using Gooeycms.Business.Web;
using System.Web.UI;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Cache;

namespace Gooeycms.Business.Content
{
    /// <summary>
    /// This is a utility class used by the datasources.
    /// </summary>
    public class ContentManagerDataAdapter
    {
        public IList<CmsContentType> GetGlobalContentTypes()
        {
            return ContentManager.Instance.GetGlobalContentTypes();
        }

        public IList<CmsContentType>  GetContentTypes()
        {
            return ContentManager.Instance.GetContentTypes();
        }

        public IList<CmsContentTypeField> GetContentTypeFields(String guid)
        {
            return ContentManager.Instance.GetContentTypeFields(guid);
        }

        public IList<CmsContent> GetExistingContent(String contentTypeFilter)
        {
            CmsContentType type = null;
            if (String.IsNullOrEmpty(contentTypeFilter))
                type = ContentManager.Instance.GetContentType(contentTypeFilter);

            return ContentManager.Instance.GetExistingContent(type);
        }


        public IList<CmsContent> GetUnapprovedContent()
        {
            return ContentManager.Instance.GetUnapprovedContent();
        }
    }

    public enum ContentTypeFilter
    {
        IncludeGlobalTypes,
        DoNotIncludeGlobalTypes
    }

    public class ContentManager
    {
        private static ContentManager instance = new ContentManager();
        private static IList<CmsContentTypeField> defaultFields = new List<CmsContentTypeField>();

        private ContentManager() { }
        public static ContentManager Instance 
        {
            get { return ContentManager.instance; }
        }

        /*
        public static IList<CmsContentTypeField> GetSystemDefaultFields()
        {
            if (defaultFields.Count == 0)
            {
                CmsContentTypeField field = new CmsContentTypeField();
                field.SystemName = "title";
                field.Name = "Title";
                field.ObjectType = "System.String";
                field.FieldType = "Textbox";
                field.Description = "[default] The title of the content";
                field.IsRequired = true;
                field.IsSystemDefault = true;
                defaultFields.Add(field);

                field = new CmsContentTypeField();
                field.SystemName = "description";
                field.Name = "Description";
                field.Description = "[default] The description of the content";
                field.FieldType = "Textbox";
                field.ObjectType = "System.String";
                field.IsRequired = true;
                field.IsSystemDefault = true;
                defaultFields.Add(field);
            }

            return defaultFields;
        }
         * */

        public IList<CmsContentType> GetGlobalContentTypes()
        {
            CmsContentTypeDao dao = new CmsContentTypeDao();
            return dao.FindGlobalContentTypes();
        }

        public IList<CmsContentType> GetContentTypes()
        {
            return GetContentTypes(CurrentSite.Guid);
        }

        public IList<CmsContentType> GetContentTypes(ContentTypeFilter filter)
        {
            return GetContentTypes(CurrentSite.Guid, filter);
        }

        public IList<CmsContentType> GetContentTypes(Data.Guid siteGuid)
        {
            return GetContentTypes(siteGuid, ContentTypeFilter.DoNotIncludeGlobalTypes);
        }

        public IList<CmsContentType> GetContentTypes(Data.Guid siteGuid, ContentTypeFilter filter)
        {
            CmsContentTypeDao dao = new CmsContentTypeDao();

            List<CmsContentType> results = new List<CmsContentType>();
            if (filter == ContentTypeFilter.IncludeGlobalTypes)
            {
                IList<CmsContentType> global = dao.FindGlobalContentTypes();
                if (global != null)
                    results.AddRange(global);
            }

            IList<CmsContentType> local = dao.FindBySite(siteGuid);
            if (local != null)
                results.AddRange(local);

            return results;
        }

        public void AddContentType(CmsContentType type)
        {
            AddContentType(CurrentSite.Guid, type);
        }

        public void AddContentType(Data.Guid siteGuid, CmsContentType type)
        {
            if (String.IsNullOrEmpty(type.Guid))
                type.Guid = Data.Guid.Create().Value;

            if (!type.IsGlobalType)
                type.SubscriptionId = siteGuid.Value;

            //make sure this name doesn't already exist
            CmsContentTypeDao dao = new CmsContentTypeDao();

            CmsContentType existing = dao.FindBySiteAndName(type.SubscriptionId, type.Name);
            if (existing != null)
                throw new ArgumentException("This type name already exists and may not be used again.");

            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsContentType>(type);
                tx.Commit();
            }
        }

        public void AddContentTypeField(CmsContentType type, CmsContentTypeField field)
        {
            CmsContentTypeDao dao = new CmsContentTypeDao();
            field.Parent = type;

            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsContentTypeField>(field);
                tx.Commit();
            }
        }

        public CmsContentType GetContentType(Data.Guid guid)
        {
            CmsContentTypeDao dao = new CmsContentTypeDao();
            return dao.FindByGuid(guid);
        }

        public IList<CmsContentTypeField> GetContentTypeFields(Data.Guid contentTypeGuid)
        {
            CmsContentTypeDao dao = new CmsContentTypeDao();
            return dao.FindFieldsByContentTypeGuid(contentTypeGuid);
        }

        public CmsContentTypeField GetContentTypeField(Data.Guid contentTypeGuid, int fieldKey)
        {
            CmsContentTypeDao dao = new CmsContentTypeDao();
            return dao.FindFieldByContentTypeAndKey(contentTypeGuid, fieldKey);
        }

        public void Delete(CmsContent content)
        {
            if (content != null)
            {
                CmsContentDao dao = new CmsContentDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsContent>(content);
                    tx.Commit();
                }
            }
        }

        public void Delete(CmsContentType contentType)
        {
            if (contentType != null)
            {
                CmsContentTypeDao dao = new CmsContentTypeDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsContentType>(contentType);
                    tx.Commit();
                }
            }
        }

        public void DeleteField(Data.Guid contentTypeGuid, int fieldKey)
        {
            CmsContentTypeField field = GetContentTypeField(contentTypeGuid, fieldKey);
            if (field != null)
            {
                CmsContentTypeDao dao = new CmsContentTypeDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsContentTypeField>(field);
                    tx.Commit();
                }
            }
        }

        public IList<CmsContent> GetFileContent()
        {
            return GetFileContent(CurrentSite.Guid);
        }

        public IList<CmsContent> GetFileContent(Data.Guid siteGuid)
        {
            CmsContentDao dao = new CmsContentDao();
            return dao.FindFilesBySite(siteGuid);
        }

        public IList<CmsContent> GetExistingContent(CmsContentType filter)
        {
            return GetExistingContent(CurrentSite.Guid, filter);
        }

        public IList<CmsContent> GetExistingContent(Data.Guid siteGuid, CmsContentType filter)
        {
            CmsContentDao dao = new CmsContentDao();
            IList<CmsContent> results;
            if (filter == null)
            {
                results = dao.FindAllContent(siteGuid);
            }
            else
            {
                results = dao.FindContentByType(siteGuid,filter);
            }

            return results;
        }

        public CmsContent CreateContent(CmsContent content, System.Web.UI.WebControls.Table dynamicControls)
        {
            if (content.SubscriptionId == null)
                throw new ApplicationException("The subscription id must not be null. This is a programming error.");

            IList<CmsContentField> fields = new List<CmsContentField>();
            PopulateFields(content.SubscriptionId,dynamicControls, content, null);

            CmsContentDao dao = new CmsContentDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsContent>(content);
                tx.Commit();
            }

            return content;
        }

        private void PopulateFields(Data.Guid siteGuid, Control parent, CmsContent item, String oldFilename)
        {
            if (item.ContentType.IsFileType)
            {
                FileUpload upload = (FileUpload)ControlHelper.FindRecursive(parent, "fileupload");
                if ((!upload.HasFile) && (oldFilename == null))
                    throw new ArgumentException("This content type requires that a file be uploaded");

                if (upload.HasFile)
                {
                    byte[] data = upload.FileBytes;
                    String filename = (oldFilename == null) ? upload.FileName : oldFilename;
                    String mimeType = upload.PostedFile.ContentType;

                    Boolean overwrite = false;
                    if (!String.IsNullOrWhiteSpace(oldFilename))
                        overwrite = true;

                    if (!ContentFileUploadImpl.IsValidFileType(filename))
                        throw new ArgumentException("The specified filetype is not currently supported by the CMS. Valid file types are:" + ContentFileUploadImpl.ValidExtensionsString);

                    ContentFileUploadImpl handler = new ContentFileUploadImpl();
                    handler.Save(siteGuid,data, filename, overwrite);

                    CmsContentField filenameField = new CmsContentField();
                    CmsContentField mimeTypeField = new CmsContentField();

                    filenameField.Name = "filename";
                    filenameField.ObjectType = "System.String";
                    filenameField.Value = filename;
                    filenameField.Parent = item;

                    mimeTypeField.Name = "mimetype";
                    mimeTypeField.ObjectType = "System.String";
                    mimeTypeField.Value = mimeType;
                    mimeTypeField.Parent = item;

                    item.AddField(filenameField);
                    item.AddField(mimeTypeField);
                }
            }

            //Loop through each expected id and build up the CMS content item
            IList<CmsContentTypeField> typeFields = ContentManager.Instance.GetContentTypeFields(item.ContentType.Guid);
            foreach (CmsContentTypeField typeField in typeFields)
            {
                String id = ContentWebControlManager.GetControlId(typeField);
                String[] parts = id.Split('_');
                String objectType = parts[1];
                String propertyName = parts[2];

                String result = null;
                Control control = ControlHelper.FindRecursive(parent, id);
                switch (objectType.ToLower())
                {
                    case CmsContentTypeField.Textbox:
                    case CmsContentTypeField.Textarea:
                        result = ContentWebControlManager.GetTextboxValue(control);
                        break;
                    case CmsContentTypeField.Datetime:
                        result = ContentWebControlManager.GetDateTimeValue(control);
                        break;
                    case CmsContentTypeField.Dropdown:
                        result = ContentWebControlManager.GetDropdownValue(control);
                        break;
                }

                CmsContentField field = new CmsContentField();
                field.Name = typeField.SystemName;
                field.ObjectType = typeField.ObjectType;
                field.Value = result;

                field.Parent = item;
                item.AddField(field);
            }
        }

        public CmsContent GetContent(Data.Guid siteGuid, Data.Guid contentGuid)
        {
            CmsContentDao dao = new CmsContentDao();
            return dao.FindByGuid(siteGuid,contentGuid);
        }

        public CmsContent GetContent(Data.Guid guid)
        {
            return GetContent(CurrentSite.Guid, guid);
        }

        public void Update(CmsContent item, Table table)
        {
            //Updating always unapproved the item
            item.IsApproved = false;

            //Save the filename, so we can restore it
            String filename = null;
            if (item.ContentType.IsFileType)
                filename = item.FindField("filename").Value;

            //Remove all of the existing items
            foreach (CmsContentField field in item.Fields)
            {
                if (!(field.Name.Equals("filename")) &&
                     !(field.Name.Equals("mimetype")))
                {
                    item.RemoveField(field);
                }
            }
            PopulateFields(item.SubscriptionId,table, item, filename);

            Save(item);
        }

        public void Save(CmsContent item)
        {
            CmsContentDao dao = new CmsContentDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsContent>(item);
                tx.Commit();
            }

            SitePageCacheRefreshInvoker.InvokeRefresh(item.SubscriptionId, SitePageRefreshRequest.PageRefreshType.All);
        }

        public CmsContent GetFile(String filename)
        {
            return GetFile(CurrentSite.Guid, filename);
        }

        public CmsContent GetFile(Data.Guid siteGuid, string filename)
        {
            CmsContentDao dao = new CmsContentDao();
            return dao.FindByFilename(siteGuid, filename);
        }

        internal static string DecryptFilename(Data.EncryptedValue encryptedDownload)
        {
            return TextEncryption.Decode(encryptedDownload.Value);
        }

        internal static string EncryptFilename(string filename)
        {
            return TextEncryption.Encode(filename);
        }

        internal IList<CmsContent> GetAllContent(Data.Guid siteGuid)
        {
            CmsContentDao dao = new CmsContentDao();
            return dao.FindAllContent(siteGuid);
        }

        public IList<CmsContent> GetUnapprovedContent(Data.Guid siteGuid)
        {
            CmsContentDao dao = new CmsContentDao();
            return dao.FindUnapprovedContent(siteGuid);
        }

        public IList<CmsContent> GetUnapprovedContent()
        {
            return GetUnapprovedContent(CurrentSite.Guid);
        }

        public void Approve(Data.Guid siteGuid, Data.Guid contentGuid, String approvedBy)
        {
            CmsContent content = GetContent(siteGuid,contentGuid);
            if (content != null)
            {
                content.Author = approvedBy;
                content.IsApproved = true;
                Save(content);
            }
        }

        public void Approve(Data.Guid guid, string approvedBy)
        {
            Approve(CurrentSite.Guid, guid, approvedBy);
        }
    }
}
