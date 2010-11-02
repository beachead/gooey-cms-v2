using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gooeycms.Data.Collections;

namespace Gooeycms.Data.Model.Content
{
    [Serializable]
    public class CmsContent : BasePersistedItem
    {
        public virtual CmsContentType @ContentType { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual String Culture { get; set; }
        public virtual String Author { get; set; }
        public virtual String ApprovedBy { get; set; }
        public virtual String Guid { get; set; }
        public virtual DateTime LastSaved { get; set; }
        public virtual DateTime PublishDate { get; set; }
        public virtual DateTime ExpireDate { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual Boolean RequiresRegistration { get; set; }
        public virtual String RegistrationPage { get; set; }
        public virtual String Content { get; set; }

        private IList fields = new ArrayList();
        internal virtual IList _Fields
        {
            get { return (IList)this.fields; }
            set { this.fields = value; }
        }
        public virtual IList<CmsContentField> Fields
        {
            get { return new List<CmsContentField>(new ListAdapter<CmsContentField>(this.fields)); }
        }
        public virtual void AddField(CmsContentField field)
        {
            field.Parent = this;
            this.fields.Add(field);
        }
        public virtual void RemoveField(CmsContentField field)
        {
            this.fields.Remove(field);
            field.Parent = null;
        }

        public virtual String Title
        {
            get
            {
                String titleField = ContentType.TitleFieldName;
                return FindField(titleField).Value;
            }
        }

        public virtual String TagsAsString()
        {
            //TODO Actually return the tags as a string
            return "";
        }

        public virtual CmsContentField FindField(String key)
        {
            CmsContentField result = null;

            if (key.ToLower().Equals("id"))
            {
                result = new CmsContentField();
                result.Name = "Id";
                result.Value = this.Id.ToString();
                result.ObjectType = "System.String";
            }
            else if (key.ToLower().Equals("content"))
            {
                result = new CmsContentField();
                result.Name = "Content";
                result.Value = this.Content.ToString();
                result.ObjectType = "System.String";
            }
            else
            {
                result = ((List<CmsContentField>)this.Fields).Find(
                   delegate(CmsContentField f) { return f.Name.ToLower().Equals(key.ToLower()); });
            }

            return result;
        }
    }
}
