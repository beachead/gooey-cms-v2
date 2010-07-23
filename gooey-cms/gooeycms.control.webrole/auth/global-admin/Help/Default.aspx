<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Help.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
Edit Existing: <asp:DropDownList ID="ExistingHelpPath" runat="server" />&nbsp;<asp:Button ID="BtnEdit" Text="Edit" OnClick="BtnEdit_Click" runat="server" />
<br /><hr /><br />
<a href="Default.aspx">New Help Page</a><br />
<asp:HiddenField ID="ExistingHelpId" runat="server" />
Path: <asp:TextBox ID="TxtPath" runat="server" /><br />
HTML:<br />
<asp:TextBox ID="TxtContent" TextMode="MultiLine" Columns="150" Rows="20" runat="server" />
<br />
<asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
</asp:Content>
