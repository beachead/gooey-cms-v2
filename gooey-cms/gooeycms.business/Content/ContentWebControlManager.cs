using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Content;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasicFrame.WebControls;
using Gooeycms.Extensions;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Content
{
    public class ContentWebControlManager
    {
        /// <summary>
        /// Struct which holds the dynamic field information for the content type
        /// </summary>
        public struct ContentWebControl
        {
            public String Description;
            public CmsContentTypeField Field;
            public Boolean DisplayError;
            public Control @Control;
        }

        private static ContentWebControlManager instance = new ContentWebControlManager();
        private ContentWebControlManager() { }
        public static ContentWebControlManager Instance 
        {
            get { return ContentWebControlManager.instance; }
        }

        public IList<ContentWebControl> GetContentTypeControls(Data.Guid contentTypeGuid)
        {
            IList<ContentWebControl> results = new List<ContentWebControl>();

            CmsContentType type = ContentManager.Instance.GetContentType(contentTypeGuid);
            IList<CmsContentTypeField> fields = ContentManager.Instance.GetContentTypeFields(contentTypeGuid);
            foreach (CmsContentTypeField field in fields)
            {
                ContentWebControl result = new ContentWebControl(); ;
                switch (field.FieldType.ToLower())
                {
                    case CmsContentTypeField.Textbox:
                        TextBox textbox = new TextBox();
                        textbox.Attributes["dojoType"] = "dijit.form.ValidationTextBox";
                        textbox.Attributes["required"] = field.IsRequired.StringValue();
                        textbox.Width = Unit.Pixel(380);

                        result = this.GetDynamicField(textbox, field);
                        result.DisplayError = false;
                        break;
                    case CmsContentTypeField.Textarea:
                        TextBox textarea = new TextBox();
                        textarea.TextMode = TextBoxMode.MultiLine;
                        textarea.Rows = field.Rows;
                        textarea.Columns = field.Columns;
                        result = this.GetDynamicField(textarea, field);
                        break;
                    case CmsContentTypeField.Datetime:
                        BDPLite datetime = new BDPLite();
                        result = this.GetDynamicField(datetime, field);
                        break;
                    case CmsContentTypeField.Dropdown:
                        DropDownList list = new DropDownList();
                        result = this.GetDynamicField(list, field);
                        break;
                }

                results.Add(result);
            }

            if (type.IsFileType)
            {
                String description = "Select file to upload";

                Control upload = new FileUpload();
                upload.ID = "fileupload";

                CmsContentTypeField temp = new CmsContentTypeField();
                temp.Name = "Select file to upload";
                temp.SystemName = "file";

                ContentWebControl field = new ContentWebControl();
                field.Description = description;
                field.Control = upload;
                field.Field = temp;

                results.Add(field);
            }

            return results;
        }

        private ContentWebControl GetDynamicField(Control control, CmsContentTypeField field)
        {
            control.ID = ContentWebControlManager.GetControlId(field);

            if (control is DropDownList)
            {
                DropDownList temp = (DropDownList)control;
                foreach (String item in field.SelectOptions)
                {
                    ListItem listitem = new ListItem(item, item);
                    temp.Items.Add(listitem);
                }
            }

            ContentWebControl result = new ContentWebControl();
            result.Description = field.Description;
            result.Control = control;
            result.Field = field;
            result.DisplayError = true;

            return result;
        }

        public static String GetTextboxValue(Control control)
        {
            if (!(control is TextBox))
                throw new ArgumentException("Expected a TextBox control, but received " + control.GetType().FullName);

            TextBox textbox = (TextBox)control;
            return (textbox != null) ? textbox.Text : null;
        }

        public static String GetDateTimeValue(Control control)
        {
            String result;
            if (control is BDPLite)
            {
                BDPLite datetime = (BDPLite)control;
                result = datetime.SelectedDate.ToString();
            }
            else if (control is TextBox)
            {
                TextBox textbox = (TextBox)control;
                result = DateTime.Parse(textbox.Text).ToString();
            }
            else
                throw new ArgumentException("Expected a datetime control or value, but received " + control.GetType().FullName);

            return result;
        }

        public static String GetControlId(CmsContentTypeField field)
        {
            return String.Format("Dynamic_{0}_{1}", field.FieldType, field.SystemName.Replace(" ", "_"));
        }

        public static void SetControl(Control control, string value)
        {
            if (control is TextBox)
            {
                ((TextBox)control).Text = value;
            }
            else if (control is BDPLite)
            {
                if (String.IsNullOrEmpty(value))
                    value = DateTime.Now.ToString();
                try
                {
                    ((BDPLite)control).SelectedDate = DateTime.Parse(value);
                }
                catch (Exception)
                {
                    ((BDPLite)control).SelectedDate = DateTime.Now;
                }
            }
            else if (control is DropDownList)
            {
                ((DropDownList)control).SelectedValue = value;
            }
        }
    }
}
