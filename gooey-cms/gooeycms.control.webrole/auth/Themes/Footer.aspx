<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Footer.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.Footer" %>
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
    <div dojoType="dijit.TitlePane" title="Footer Template">
        <beachead:Editor ID="TxtFooter" ShowPreviewWindow="false" runat="server" />
    </div>
    <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
</asp:Content>
