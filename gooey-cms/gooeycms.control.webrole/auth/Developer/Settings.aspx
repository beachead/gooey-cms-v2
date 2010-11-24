<%@ Page Title="" Language="C#" MasterPageFile="~/SecureNoNavigation.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Settings" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../scripts/store.js"></script>

    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
</asp:Content>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="developer" navItem="settings" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
Upload your organization's logo.
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
<div style="width:900px;">
   <table>
        <tr>
            <td><asp:Label ID="LblStatus" runat="server" /></td>
        </tr>
        <tr>
            <td valign="top"><asp:Image ID="LogoSrc" runat="server" />&nbsp;Upload Logo:
            <asp:FileUpload ID="LogoFile" runat="server" /><asp:Button ID="BtnUploadLogo" OnClick="BtnUploadLogo_Click" Text="Upload" runat="server" />
            </td>
      </tr>
    </table>
    <hr />

    </div>

</asp:Content>
