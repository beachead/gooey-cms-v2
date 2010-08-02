using System;
using System.Web.UI;
using Gooeycms.Business.Pages;

namespace Beachead.Web.CMS.controls
{
    public partial class MarkupEditor : System.Web.UI.UserControl
    {
        protected static String EditorScriptPath;
        protected static String EditorCssPath;
        protected static String RootUrl;
        private Boolean isShowToolbar = true;
        private bool useStandardImageTags = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (EditorScriptPath == null)
            {
                EditorScriptPath = Page.ResolveUrl("~/scripts/contenteditor.js");
                EditorCssPath = Page.ResolveUrl("~/controls/ContentEditor/contenteditor.css");
                RootUrl = Page.ResolveUrl("~");
            }

            Anthem.Manager.Register(this);

            this.ToolbarPanel.Visible = ShowToolbar;
        }

        public Boolean UseStandardImageTags
        {
            get { return this.useStandardImageTags; }
            set { this.useStandardImageTags = value; }
        }

        public Boolean ShowToolbar
        {
            get { return this.isShowToolbar; }
            set { this.isShowToolbar = value; }
        }

        public short TabIndex
        {
            set { this.PageMarkupText.TabIndex = value; }
            get { return this.PageMarkupText.TabIndex; }
        }

        public String Text
        {
            get { return this.PageMarkupText.Text; }
            set { this.PageMarkupText.Text = value; }
        }

        [Anthem.Method]
        public String Preview_Click()
        {
            IPreviewable preview = (IPreviewable)this.Page;
            String url = preview.Save();

            return url;
            //Anthem.Manager.AddScriptForClientSideEval("DisplayPreview('" + url + "');");
        }
    }
}