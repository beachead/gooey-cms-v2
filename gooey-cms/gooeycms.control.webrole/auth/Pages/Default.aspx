<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Default" %>
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
    function onClientContextMenuShowing(sender, args) {
        var treeNode = args.get_node();
        treeNode.set_selected(true);
        //enable/disable menu items
        setMenuItemsState(args.get_menu(), treeNode);
    }

    //this method disables the appropriate context menu items
    function setMenuItemsState(menu, treeNode) {
        if (treeNode.get_value() == '/') {
            var item = menu.findItemByValue('DeleteDirectory');
            item.set_enabled(false);
        }
    }


    function onClientContextMenuItemClicking(sender, args) {
        var menuItem = args.get_menuItem();
        var treeNode = args.get_node();
        menuItem.get_menu().hide();

        switch (menuItem.get_value()) {
            case "NewPage":
                if (treeNode.get_text() == '--New Folder--') {
                    alert('You must rename the directory from its default value before creating sub-pages');
                    treeNode.startEdit();
                    args.set_cancel(true);
                }
                break;
            case "Edit":
                openEditWindow(treeNode.get_value());
                break;
            case "ManageImages":
                var wnd = window.radopen("ImageBrowser.aspx?pid=" + treeNode.get_value(), null);
                var width = 715;
                wnd.set_title("Image Manager");
                wnd.set_width(width);
                wnd.set_height(500);
                wnd.moveTo(10, 10);
                break;
            case "ManageCss":
                var wnd = window.radopen("Stylesheet.aspx?a=edit&pid=" + treeNode.get_value(), null);
                var width = 925;
                wnd.set_title("CSS: " + treeNode.get_value());
                wnd.set_width(width);
                wnd.moveTo(10, 10);
                break;
            case "ManageJavascript":
                var wnd = window.radopen("Javascript.aspx?a=edit&pid=" + treeNode.get_value(), null);
                var width = 925;
                wnd.set_title("Javascript: " + treeNode.get_value());
                wnd.set_width(width);
                wnd.moveTo(10, 10);
                break;
            case "EditMeta":
                var wnd = window.radopen("Metatags.aspx?a=edit&pid=" + treeNode.get_value(), null);
                var width = 700;
                var height = 400;
                wnd.set_title("Meta Tags: " + treeNode.get_value());
                wnd.set_width(width);
                wnd.set_height(height);
                wnd.moveTo(10, 10);
                break;
            case "Rename":
                var result = confirm('Warning: Renaming this page will break any links, redirects, or registrations you have that reference this page.\n\nWould you like to continue?');
                if (result)
                    treeNode.startEdit();
                else
                    args.set_cancel(true);
                break;
            case "NewFolder":
                break;
            case "DeleteDirectory":
                if (treeNode.get_value() != '/') {
                    var result = confirm("Are you sure you want to delete " + treeNode.get_text() + " and all subfolders and pages?\n\nWARNING: This action can not be undone!");
                    if (result) {
                        result = confirm("Please re-confirm that you would like to delete " + treeNode.get_text() + " and all subfolders and pages?\n\nWARNING: This action can not be undone!");
                        args.set_cancel(!result);
                    }
                } else {
                    alert('You can not delete the root folder');
                    args.set_cancel(true);
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
                openEditWindow(node.get_value());
                break;
        }
    }

    function onClientNodeDropping(sender, args) {
        var destination = args.get_destNode();
        var source = args.get_sourceNode();
    }

    function openEditWindow(page) {
        var wnd = window.radopen("Editor.aspx?a=edit&pid=" + page, null);
        var width = window.document.body.clientWidth - 50;
        var height = window.document.body.clientHeight - 10;
        wnd.set_title(page);
//        wnd.set_width(width);
//        wnd.set_height(height);
        wnd.moveTo(10, 10);
        wnd.maximize();
    }

    //-->
    </script>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
    <telerik:RadAjaxPanel ID="UpdatePanel" LoadingPanelID="LoadingPanel" runat="server">
    <asp:LinkButton ID="BtnAddNewPage" OnClick="BtnAddNewPage_Click" Text="Add New Page" runat="server" />
    <br /><br />
    <telerik:RadTreeView ID="PageTreeView" Skin="Default" EnableDragAndDrop="true" EnableDragAndDropBetweenNodes="true" 
                            OnNodeDataBound="PageTreeview_NodeDataBound"  
                            OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                            OnClientDoubleClick="onClientDoubleClick"
                            OnClientNodeDropping="onClientNodeDropping"
                            OnContextMenuItemClick="PageTreeView_ContextMenuItemClick"
                            OnClientContextMenuShowing="onClientContextMenuShowing"
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
                    <telerik:RadMenuItem Value="ManageImages" Text="Manage Images" ImageUrl="~/Images/Vista/9.gif" ></telerik:RadMenuItem>
                    <telerik:RadMenuItem Value="ManageCss" Text="Manage CSS" ImageUrl="~/Images/Vista/9.gif" ></telerik:RadMenuItem>
                    <telerik:RadMenuItem Value="ManageJavascript" Text="Manage Javascript" ImageUrl="~/Images/Vista/9.gif" ></telerik:RadMenuItem>
                    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem Value="EditMeta" Text="Edit Header Tags" ImageUrl="~/Images/Vista/9.gif" ></telerik:RadMenuItem>
                    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem Value="DeletePage" Text="Delete" ImageUrl="~/Images/Vista/7.gif" ></telerik:RadMenuItem>
                </Items>
            </telerik:RadTreeViewContextMenu>
        </ContextMenus>
    </telerik:RadTreeView>
    </telerik:RadAjaxPanel>

<telerik:RadWindowManager ID="Singleton" Skin="Default" DestroyOnClose="true" Height="600" Modal="false" KeepInScreenBounds="true" ShowContentDuringLoad="false" AutoSize="false" VisibleStatusbar="false" Behaviors="Close,Move,Resize,Minimize,Maximize" runat="server" EnableShadow="true">
</telerik:RadWindowManager>
</asp:Content>
