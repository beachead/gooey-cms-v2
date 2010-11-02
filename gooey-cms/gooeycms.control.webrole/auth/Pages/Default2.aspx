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
                //window.location = 'Edit.aspx?a=edit&pid=' + treeNode.get_value();
                var splitter = $find("<%= RadSplitter1.ClientID %>");
                var pane = splitter.getPaneById("<%= MainPane.ClientID %>");
                if (pane) {
                    pane.set_contentUrl('EditInline.aspx?a=edit&pid=' + treeNode.get_value());
                }
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


<telerik:RadSplitter id="RadSplitter1" SkinID="Outlook" runat="server" Height="700px" width="100%">
    <telerik:RadPane id="LeftPane" runat="server" width="22" Scrolling="None">
        <telerik:RadSlidingZone id="SlidingZone1" runat="server" width="22" DockedPaneId="PageListingPane">
            <telerik:RadSlidingPane id="PageListingPane" title="Page Manager" EnableResize="false" DockOnOpen="true" width="150px" runat="server">
                <telerik:RadAjaxPanel ID="UpdatePanel" runat="server">
                <telerik:RadTreeView ID="PageTreeView" Skin="Windows7" EnableDragAndDrop="true" EnableDragAndDropBetweenNodes="true" 
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
                                <telerik:RadMenuItem Value="DeletePage" Text="Delete" ImageUrl="~/Images/Vista/7.gif" ></telerik:RadMenuItem>
                            </Items>
                        </telerik:RadTreeViewContextMenu>
                    </ContextMenus>
                </telerik:RadTreeView>
                </telerik:RadAjaxPanel>
            </telerik:RadSlidingPane>
        </telerik:RadSlidingZone>
    </telerik:RadPane>
    <telerik:RadSplitBar id="RadSplitbar1" runat="server"></telerik:RadSplitBar>
    <telerik:RadPane id="MainPane" Scrolling="Both" ContentUrl="" runat="server">
    </telerik:RadPane>
</telerik:RadSplitter>
</asp:Content>
