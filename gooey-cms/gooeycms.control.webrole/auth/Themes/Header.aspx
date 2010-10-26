<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Header.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.HeaderFooter" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="templates" navItem="header" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <h1>Template Header</h1>
    <asp:Label ID="LblStatus" runat="server" />
    <br />
    <asp:Button ID="BtnSave2" Text="Save" OnClick="BtnSave_Click" runat="server" />
    <div dojoType="dijit.TitlePane" title="Header Template">
        <beachead:Editor ID="TxtHeader" ShowPreviewWindow="false" runat="server" />
    </div>
    <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
</asp:Content>
