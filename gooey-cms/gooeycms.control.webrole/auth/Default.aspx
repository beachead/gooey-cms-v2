<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.Auth.Default" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>       
            <li class="">&nbsp;WELCOME TO GOOEY CMS</li>     
        </ul>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>WELCOME TO GOOEY CMS</h1>

    <p>
        <strong><a href="./content/">Content</a></strong><br />
        Create new content types or manage existing content for your site such as news or events.
    </p>
    <p>
        <strong><a href="./pages/">Pages</a></strong><br />
        Manage your site's pages, meta-tags and images.
    </p>
     <p>
        <strong><a href="./campaigns/">Campaigns</a></strong><br />
        Manage your site's phone calls, marketing campaigns, export marketing leads and configure Google Analytics, Salesforce or your phone number settings.
    </p>
        <p>
        <strong><a href="./promotion/">Promotion</a></strong><br />
        View the latest version of your site on your dedicated staging server, and promote pages or content live when ready.
    </p>

    <p>
        <strong><a href="./manage.aspx">Site</a></strong><br />
        Manage your site's subscription plan and options, language, and domain name.
    </p>
     <p>
        <strong><a href="./themes/">Themes</a></strong><br />
        Manage your site's header, footer, templates and themes.
    </p>
    <p>
        <strong><a href="./users/">Users</a></strong><br />
        Manage your sites administrators, editors, or writers.
    </p>
    <p>
        <strong><a href="./dashboard.aspx">Dashboard</a></strong><br />
        Manage your website purchases, and apply them to a new or existing subscription plan.
    </p>
    <p>
        <strong><a href="./developer">Developers</a></strong><br />
        Package up a website you've developed to sell in the <a href="http://store.gooeycms.com">Gooey CMS Store</a>, and embed a website for sale on your own website.
    </p>

</asp:Content>

