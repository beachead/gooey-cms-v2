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

namespace Gooeycms.Business.Content
{
    /// <summary>
    /// This is a utility class used by the datasources.
    /// </summary>
    public class ContentManagerDataAdapter
    {
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
            PopulateFields(dynamicControls, content, null);

            CmsContentDao dao = new CmsContentDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsContent>(content);
                tx.Commit();
            }

            return content;
        }

        private void PopulateFields(Control parent, CmsContent item, String oldFilename)
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

                    CmsContentField filenameField = new CmsContentField();
                    CmsContentField mimeTypeField = new CmsContentField();

                    //Save the file to the filesystem
                    /*
                    bool allowOverwrite = false;
                    if (oldFilename != null)
                        allowOverwrite = true;
                    */
                    //TODO Actually save the file---filehandler.Save(filename, mimeType, data, allowOverwrite);

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
                }

                CmsContentField field = new CmsContentField();
                field.Name = typeField.SystemName;
                field.ObjectType = typeField.ObjectType;
                field.Value = result;

                field.Parent = item;
                item.AddField(field);
            }
        }

        public CmsContent GetContent(Data.Guid guid)
        {
            CmsContentDao dao = new CmsContentDao();
            return dao.FindByGuid(guid);
        }
    }
}
