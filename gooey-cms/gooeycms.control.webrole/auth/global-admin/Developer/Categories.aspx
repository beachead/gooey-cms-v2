<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Developer.Categories" %>

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
    Manage the categories which are available on the developer package page:
    <br /><br />
    
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
    <telerik:RadAjaxPanel ID="AjaxPanel" LoadingPanelID="LoadingPanel"  runat="server">
    <div style="min-height:250px;">
    <table>
        <tr>
            <td>
                <telerik:RadListBox ID="LstCategories" AutoPostBackOnDelete="true"  AllowDelete="true" OnDeleting="LstCategories_Deleting" SelectionMode="Multiple" Skin="Default" runat="server" />
            </td>
            <td style="vertical-align:top;">
                <asp:TextBox ID="TxtAddItem" runat="server" />&nbsp;<asp:Button ID="BtnAddItem" Text="Add Category" OnClick="BtnAddItem_Click" runat="server" /><br />
            </td>
        </tr>
    </table>   
    </div>
    </telerik:RadAjaxPanel>
</asp:Content>
