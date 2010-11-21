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
    <div>
        Manage Roles:
        <br />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Skin="Windows7" runat="server" />
        <telerik:RadListBox ID="LstRoles" CheckBoxes="true" Width="300" OnItemCheck="LstRoles_Checked" AutoPostBack="true" Skin="Windows7" runat="server"></telerik:RadListBox>
    </div>
    </form>
</body>
</html>
