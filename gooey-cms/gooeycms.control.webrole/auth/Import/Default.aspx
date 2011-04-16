<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Import.Default" %>
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

<telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default"  runat="server"></telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxPanel ID="ImportPanel" LoadingPanelID="LoadingPanel" runat="server">
    <p>
    Input the url of the site to import. You will be able to add/remove pages prior to importing the content.<br />
    <asp:TextBox ID="TxtSiteUrl" runat="server" />&nbsp;
    <asp:Button ID="BtnCrawlSite" Text="Start" OnClientClick="if (!confirm('Are you sure you want to begin the site import process?\r\nDepending upon the size of your site, this may take a while.')) return false;" OnClick="BtnCrawlSite_Click" runat="server" /><br />
    <asp:Label ID="LblStatus" runat="server" />
    </p>
</telerik:RadAjaxPanel>
</asp:Content>