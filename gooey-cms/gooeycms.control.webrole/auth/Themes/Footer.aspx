<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Footer.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.Footer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="Default.aspx">MANAGE THEMES</a></li>
            <li class="last"><a href="./AddNewTheme.aspx">Add Theme</a></li>           
        </ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>Template Footer</h1>
    <asp:Label ID="LblStatus" runat="server" />
    <br />

    <div dojoType="dijit.TitlePane" title="Footer Template">
        <beachead:Editor ID="TxtFooter" ShowPreviewWindow="false" runat="server" />
    </div>

    <div class="controls">
        <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
    </div>

</asp:Content>
