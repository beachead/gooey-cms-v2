<%@ Page Title="" Language="C#" MasterPageFile="~/gooey-static/Static.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="Gooeycms.webrole.sites.gooey_static._404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <b>404 Error</b>: We're sorry the page you are looking for could not be found.
    <br /><br />
    You may want to contact the site administrator to ensure that the page has been approved.
    <br /><br /><br /><br /><br />
    <hr />
    <br />
    Do you host your site on Gooey and wondering why you're seeing this page? <br /><br />
    This is the default page that users will see if they request a page which does not exist or has not been approved.<br />
    You can configure a custom 404 page that matches the theme of your site by creating a page under your root directory named 404.aspx.

</asp:Content>
