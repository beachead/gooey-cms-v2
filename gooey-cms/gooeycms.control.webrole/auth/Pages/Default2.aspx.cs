using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Site;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;
using Telerik.Web.UI;
using System.Data;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Crypto;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public static class RadTreeExtensions
    {
        public static Boolean IsDirectory(this RadTreeNode node)
        {
            return (node.Category.Equals(CmsSiteMap.NodeTypes.Directory.ToString()));
        }

        public static Boolean IsPage(this RadTreeNode node)
        {
            return (node.Category.Equals(CmsSiteMap.NodeTypes.Page.ToString()));
        }
    }

    public partial class Default2 : System.Web.UI.Page
    {
        protected const String RootNodeValue = "&lt;root&gt;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<CmsSitePath> paths = CmsSiteMap.Instance.GetAllPaths(CurrentSite.Guid);
                this.PageTreeView.DataTextField = "Name";
                this.PageTreeView.DataValueField = "Url";
                this.PageTreeView.DataFieldID = "Url";
                this.PageTreeView.DataFieldParentID = "Parent";
                this.PageTreeView.DataSource = paths;
                this.PageTreeView.DataBind();
            }
        }

        protected void PageTreeview_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            CmsSitePath path = (CmsSitePath)e.Node.DataItem;

            if (path.IsDirectory)
            {
                if (path.Depth == 1)
                {
                    e.Node.Expanded = true;
                    e.Node.Text = RootNodeValue;
                }

                e.Node.ContextMenuID = "DirectoryContextMenu";
                e.Node.ImageUrl = "~/Images/Vista/folder.png";
                e.Node.Category = CmsSiteMap.NodeTypes.Directory.ToString();
            }
            else
            {
                e.Node.AllowDrop = false;
                e.Node.ContextMenuID = "PageContextMenu";
                e.Node.ImageUrl = "~/Images/Vista/aspx.png";
                e.Node.Category = CmsSiteMap.NodeTypes.Page.ToString();
            }
            e.Node.ToolTip = e.Node.GetFullPath("/");
        }

        protected void PageTreeView_NodeEdit(object sender, Telerik.Web.UI.RadTreeNodeEditEventArgs e)
        {
            String oldpath = e.Node.Value;
            Boolean isRename = !(e.Node.Text.Contains("--New"));
            String fixedName = e.Text.Replace(" ", "-");
            e.Node.Text = fixedName;

            String current = e.Node.Text;
            String previous = GetNodeUrl(e.Node.Prev);
            String next = GetNodeUrl(e.Node.Next);
            String parent = GetNodeUrl(e.Node.ParentNode);

            CmsSitePath path = null;
            if (!isRename)
            {
                if (e.Node.IsDirectory())
                    path = CmsSiteMap.Instance.AddChildDirectory(parent, current);
                else if (e.Node.IsPage())
                {
                    path = CmsSiteMap.Instance.AddNewPage(parent, current);
                    AddNewDefaultPage(parent, current);
                }
            }
            else //perform a rename of the path
            {
                String newpath = GetNodeUrl(e.Node);
                PageManager.Instance.Rename(oldpath, newpath);
                path = CmsSiteMap.Instance.GetPath(newpath);
            }

            e.Node.Text = path.Name;
            e.Node.Value = path.Url;
        }

        private void AddNewDefaultPage(string parent, string current)
        {
            String fullurl = CmsSiteMap.PathCombine(parent, current);

            CmsPage page = new CmsPage();
            page.Guid = System.Guid.NewGuid().ToString();
            page.Url = fullurl;
            page.UrlHash = TextHash.MD5(page.Url).Value;
            page.SubscriptionId = CurrentSite.Guid.Value;
            page.DateSaved = DateTime.Now;
            page.IsApproved = false;
            page.Author = LoggedInUser.Username;
            page.Culture = CurrentSite.Culture;
            page.Title = "My New Page";
            page.Content = "# My New Page";
            page.Keywords = "";
            page.Description = "My Default Description";
            page.Template = CurrentSite.GetDefaultTemplateName();
            page.OnBodyLoad = "";

            PageManager.PublishToWorker(page, PageTaskMessage.Actions.Save);
        }

        private String GetNodeUrl(RadTreeNode node)
        {
            String result = null;
            if (node != null)
                result = node.GetFullPath("/").Replace(RootNodeValue,"").Replace("//","/");

            if (String.IsNullOrEmpty(result))
                result = CmsSiteMap.RootPath;

            return result;
        }

        protected void PageTreeView_NodeDrop(object sender, Telerik.Web.UI.RadTreeNodeDragDropEventArgs e)
        {
            RadTreeNode dest = e.DestDragNode;
            RadTreeNode src = e.SourceDragNode;
            RadTreeViewDropPosition pos = e.DropPosition;

            if (dest != null)
            {
                String oldparent = this.GetNodeUrl(src.ParentNode);
                String newparent = this.GetNodeUrl(dest.ParentNode);

                String pagename = src.Text;
                String current = this.GetNodeUrl(src);
                String destination = this.GetNodeUrl(dest);

                //Check if we need to rename the page
                Boolean rename = !(oldparent.Equals(newparent));

                String newpath;
                switch (pos)
                {
                    case RadTreeViewDropPosition.Above: //Sibling of the destination
                        src.Owner.Nodes.Remove(src);
                        dest.InsertBefore(src);
                        newpath = GetNodeUrl(src);

                        if (rename)
                            PageManager.Instance.Rename(current, newpath);
                        CmsSiteMap.Instance.Reorder(newparent, destination, newpath, -1);
                        break;
                    case RadTreeViewDropPosition.Below:
                        src.Owner.Nodes.Remove(src);
                        dest.InsertAfter(src);
                        newpath = GetNodeUrl(src);

                        if (rename)
                            PageManager.Instance.Rename(current, newpath);
                        CmsSiteMap.Instance.Reorder(newparent, destination, newpath, 0);
                        break;
                    case RadTreeViewDropPosition.Over:
                        if (!src.IsAncestorOf(dest))
                        {
                            src.Owner.Nodes.Remove(src);
                            dest.Nodes.Add(src);


                            String newpage = GetNodeUrl(src);
                            String parent = GetNodeUrl(dest);
                            PageManager.Instance.Rename(current, newpage);
                            CmsSiteMap.Instance.Reorder(parent, null, newpage, 0);
                        }
                        break;
                }
            }

            dest.Expanded = true;
            src.TreeView.UnselectAllNodes();
        }

        protected void PageTreeView_ContextMenuItemClick(object sender, Telerik.Web.UI.RadTreeViewContextMenuEventArgs e)
        {
            switch (e.MenuItem.Value)
            {
                case "NewFolder":
                    InsertNewClientNode(e.Node, "--New Folder--", "~/Images/Vista/folder.png", true, "DirectoryContextMenu",CmsSiteMap.NodeTypes.Directory);
                    break;
                case "NewPage":
                    InsertNewClientNode(e.Node, "--New Page--", "~/Images/Vista/aspx.png", false, "PageContextMenu", CmsSiteMap.NodeTypes.Page);
                    break;
                case "DeletePage":
                    DeleteAndRefreshPage(e.Node);
                    break;
                case "DeleteDirectory":
                    DeleteAndRefreshFolder(e.Node);
                    break;
                default:
                    break;
            }
        }

        private void DeleteAndRefreshFolder(RadTreeNode node)
        {
            String fullpath = this.GetNodeUrl(node);

            CmsSitePath folder = CmsSiteMap.Instance.GetPath(fullpath);
            PageManager.Instance.DeleteFolder(folder);

            node.Remove();
        }

        private void DeleteAndRefreshPage(RadTreeNode node)
        {
            String fullpath = this.GetNodeUrl(node);
            CmsPage page = PageManager.Instance.GetLatestPage(fullpath);
            if (page == null) 
            {
                page = new CmsPage();
                page.Url = fullpath;
                page.UrlHash = TextHash.MD5(page.Url).Value;
            }
            PageManager.Instance.DeleteAll(page);

            node.Remove();
        }

        private void InsertNewClientNode(RadTreeNode parent, String text, String image, bool allowDrop, String contextMenu, CmsSiteMap.NodeTypes nodeType)
        {
            RadTreeNode newFolder = new RadTreeNode();
            newFolder.Text = text;
            newFolder.Selected = true;
            newFolder.ImageUrl = image;
            newFolder.AllowDrop = allowDrop;
            newFolder.Value = System.Guid.NewGuid().ToString();
            newFolder.ContextMenuID = contextMenu;
            newFolder.Category = nodeType.ToString();
            parent.Nodes.Add(newFolder);
            parent.Expanded = true;

            //find the node by its Value and edit it when page loads
            string js = "Sys.Application.add_load(editNode); function editNode(){ ";
            js += "var tree = $find(\"" + PageTreeView.ClientID + "\");";
            js += "var node = tree.findNodeByValue('" + newFolder.Value + "');";
            js += "if (node) node.startEdit();";
            js += "Sys.Application.remove_load(editNode);};";

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "nodeEdit", js, true);
        }
    }
}