<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Default2" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="pages" NavItem="" />
</asp:Content>

<asp:Content ID="localCSS" ContentPlaceHolderID="localCSS" runat="server">
<style type="text/css">
.rtDropAbove, 
.rtDropBelow {
    border-width: 3px !important;
}
</style>
</asp:Content>

<asp:Content ID="ContentLocalJs" ContentPlaceHolderID="localJS" runat="server">
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
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    

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
                    <telerik:RadMenuItem Value="NewPage" Text="New Page" ImageUrl="~/Images/Vista/notes.gif"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Value="NewFolder" Text="New Folder" ImageUrl="~/Images/Vista/12.gif"></telerik:RadMenuItem>
                    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem Value="Rename" Text="Rename ..." ImageUrl="~/Images/Vista/rename.gif" PostBack="false"></telerik:RadMenuItem>
    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem Value="DeleteDirectory" Text="Delete" ImageUrl="~/Images/Vista/7.gif" ></telerik:RadMenuItem>
                </Items>
            </telerik:RadTreeViewContextMenu>

            <telerik:RadTreeViewContextMenu ID="PageContextMenu" runat="server">
                <Items>
                    <telerik:RadMenuItem Value="Edit" Text="Edit ..." ImageUrl="~/Images/Vista/9.gif"></telerik:RadMenuItem>
                    <telerik:RadMenuItem Value="Rename" Text="Rename ..." ImageUrl="~/Images/Vista/rename.gif" PostBack="false"></telerik:RadMenuItem>
                    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem Value="DeletePage" Text="Delete" ImageUrl="~/Images/Vista/7.gif" ></telerik:RadMenuItem>
                </Items>
            </telerik:RadTreeViewContextMenu>
        </ContextMenus>
    </telerik:RadTreeView>
    </telerik:RadAjaxPanel>
</asp:Content>
