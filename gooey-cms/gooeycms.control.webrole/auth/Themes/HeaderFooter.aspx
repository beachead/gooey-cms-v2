<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="HeaderFooter.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Themes.HeaderFooter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <asp:Label ID="LblStatus" runat="server" />
    <br />
    <asp:Button ID="BtnSave2" Text="Save" OnClick="BtnSave_Click" runat="server" />
    <div dojoType="dijit.TitlePane" title="Header Template">
        <beachead:Editor ID="TxtHeader" ShowPreviewWindow="false" runat="server" />
    </div>
    <br />
    <div dojoType="dijit.TitlePane" title="Footer Template">
        <beachead:Editor ID="TxtFooer" ShowPreviewWindow="false" runat="server" />
    </div>
    <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
</asp:Content>
