<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit-roles.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Users.edit_roles" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
    <title>Edit Roles</title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="LstRoles">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="LstRoles" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div style="padding-left:15px;padding-top:20px;">
        Manage Roles:
        
        <div style="padding:10px;">
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Skin="Default" runat="server" />
            <telerik:RadListBox ID="LstRoles" CheckBoxes="true" Width="300" OnItemCheck="LstRoles_Checked" AutoPostBack="true" Skin="Default" runat="server"></telerik:RadListBox>
        </div>
    </div>
    <div style="padding: 15px 0 0 15px;">
    Role Descriptions:</div>
    <div style="padding:15px; font-size:11px;">
    <b>Site Administrator Role</b>:<br />Can control the key settings of your site, add new users and is the the most powerful role available.
    <br /><br />
    <b>Page Editors</b>:<br />Can create/edit and delete site pages.
    <br /><br />
    <b>Content Editors</b>:<br />Can create/edit and delete content types. I.E. News, Events, etc.
    <br /><br />
    <b>Promotion Manager</b>:<br />Can promote pages or content types live.
    <br /><br />
    <b>Campaign Manager</b>:<br />Can create/edit and delete marketing campaigns, export marketing leads, update Google Analytics, and update the Salesforce configuration for the site.
    </div>
    </form>
</body>
</html>
