<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Default2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <script type="text/javascript" language="javascript">
        //<!--
        function onClientContextMenuItemClicking(sender, args) {
            var menuItem = args.get_menuItem();
            var treeNode = args.get_node();
            menuItem.get_menu().hide();

            switch (menuItem.get_value()) {
                case "Edit":
                    window.location = 'Edit.aspx?a=edit&pid=' + treeNode.get_value();
                    break;
                case "Rename":
                    treeNode.startEdit();
                    break;
                case "NewFolder":
                    break;
                case "DeleteDirectory":
                    var result = confirm("Are you sure you want to delete " + treeNode.get_text() + " and all subfolders and pages?\n\nWARNING: This action can not be undone!");
                    if (result) {
                        result = confirm("Please re-confirm that you would like to delete " + treeNode.get_text() + " and all subfolders and pages?\n\nWARNING: This action can not be undone!");
                        args.set_cancel(!result);
                    }
                    break;
                case "DeletePage":
                    var result = confirm("Are you sure you want to delete " + treeNode.get_text() + "?\n\nWARNING: This action can not be undone!");
                    args.set_cancel(!result);
                    break;
            }
        }

        function onClientDoubleClick(sender, args) {
            var node = args.get_node();
            var category = node.get_category();
            switch (category) {
                case "page":
                    window.location = 'Edit.aspx?a=edit&pid=' + node.get_value();
                    break;
            }
        }

        function onClientNodeDropping(sender, args) {
            var destination = args.get_destNode();
            var source = args.get_sourceNode();
        }
        //-->
    </script>

    <telerik:RadAjaxPanel ID="UpdatePanel" runat="server">
    <telerik:RadTreeView ID="PageTreeView" Skin="Windows7" EnableDragAndDrop="true" EnableDragAndDropBetweenNodes="true" 
                         OnNodeDataBound="PageTreeview_NodeDataBound"  
                         OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                         OnClientDoubleClick="onClientDoubleClick"
                         OnClientNodeDropping="onClientNodeDropping"
                         OnContextMenuItemClick="PageTreeView_ContextMenuItemClick"
                         OnNodeEdit="PageTreeView_NodeEdit"
                         OnNodeDrop="PageTreeView_NodeDrop"
                         runat="server">
        <ContextMenus>
            <telerik:RadTreeViewContextMenu ID="DirectoryContextMenu" runat="server">
                <Items>
                    <telerik:RadMenuItem Value="Rename" Text="Rename ..." ImageUrl="~/Images/Vista/rename.gif"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Value="NewFolder" Text="New Folder" ImageUrl="~/Images/Vista/12.gif"></telerik:RadMenuItem>
                    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem Value="DeleteDirectory" Text="Delete" ImageUrl="~/Images/Vista/7.gif" ></telerik:RadMenuItem>
                </Items>
            </telerik:RadTreeViewContextMenu>

            <telerik:RadTreeViewContextMenu ID="PageContextMenu" runat="server">
                <Items>
                    <telerik:RadMenuItem Value="Edit" Text="Edit ..." ImageUrl="~/Images/Vista/9.gif"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Value="Rename" Text="Rename ..." ImageUrl="~/Images/Vista/rename.gif"></telerik:RadMenuItem>
                    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem Value="DeletePage" Text="Delete" ImageUrl="~/Images/Vista/7.gif" ></telerik:RadMenuItem>
                </Items>
            </telerik:RadTreeViewContextMenu>
        </ContextMenus>
    </telerik:RadTreeView>
    </telerik:RadAjaxPanel>
</asp:Content>
