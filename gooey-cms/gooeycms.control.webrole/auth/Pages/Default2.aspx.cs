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

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Default2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<CmsSitePath> paths = CmsSiteMap.Instance.GetAllPaths(CurrentSite.Guid);
                this.PageTreeView.DataTextField = "Url";
                this.PageTreeView.DataFieldID = "Url";
                this.PageTreeView.DataFieldParentID = "Parent";
                this.PageTreeView.DataSource = paths;
                this.PageTreeView.DataBind();
            }
        }

        protected void PageTreeview_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            CmsSitePath path = (CmsSitePath)e.Node.DataItem;
            e.Node.ToolTip = path.Url;

            if (path.IsDirectory)
            {
                if (path.Depth == 1)
                    e.Node.Expanded = true;
                else
                    e.Node.ContextMenuID = "DirectoryContextMenu";
                e.Node.ImageUrl = "~/Images/Vista/folder.png";
                e.Node.Category = "directory";
                e.Node.Value = path.Url;
            }
            else
            {
                e.Node.ContextMenuID = "PageContextMenu";
                e.Node.ImageUrl = "~/Images/Vista/aspx.png";
                e.Node.Category = "page";
                e.Node.Value = Server.UrlEncode(path.Url);
            }
        }

        protected void PageTreeView_NodeEdit(object sender, Telerik.Web.UI.RadTreeNodeEditEventArgs e)
        {
            e.Node.Text = e.Text;
        }

        protected void PageTreeView_NodeDrop(object sender, Telerik.Web.UI.RadTreeNodeDragDropEventArgs e)
        {
            RadTreeNode dest = e.DestDragNode;
            RadTreeNode src = e.SourceDragNode;
            RadTreeViewDropPosition pos = e.DropPosition;

            if (dest != null)
            {
                switch (pos)
                {
                    case RadTreeViewDropPosition.Above: //Sibling of the destination
                        src.Owner.Nodes.Remove(src);
                        dest.InsertBefore(src);
                        break;
                    case RadTreeViewDropPosition.Below:
                        src.Owner.Nodes.Remove(src);
                        dest.InsertAfter(src);
                        break;
                    case RadTreeViewDropPosition.Over:
                        if (!src.IsAncestorOf(dest))
                        {
                            src.Owner.Nodes.Remove(src);
                            dest.Nodes.Add(src);
                        }
                        break;
                }
            }

            dest.Expanded = true;
            src.TreeView.ClearSelectedNodes();
        }

        protected void PageTreeView_ContextMenuItemClick(object sender, Telerik.Web.UI.RadTreeViewContextMenuEventArgs e)
        {
            switch (e.MenuItem.Value)
            {
                case "NewFolder":
                    InsertNewClientNode(e.Node);
                    break;
                default:
                    break;
            }
        }

        private void InsertNewClientNode(RadTreeNode parent)
        {
            RadTreeNode newFolder = new RadTreeNode("New Folder");
            newFolder.Selected = true;
            newFolder.ImageUrl = "~/Images/Vista/folder.png";
            newFolder.Value = System.Guid.NewGuid().ToString();
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