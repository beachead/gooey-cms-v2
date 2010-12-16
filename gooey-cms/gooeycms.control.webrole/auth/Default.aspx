<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.Auth.Default" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>       
            <li class=""><a href="./themes/">THEMES</a></li>  
            <li class=""><a href="#" onclick="window.open('./themes/imageupload.aspx','','width=600,height=600,scrollbars=yes'); return false;">IMAGES</a></li>  
            <li class=""><a href="./structure/">SITE NAVIGATION</a></li>            
            <li class=""><a href="./configuration/">SITE SETTINGS</a></li>          
            <li class=""><a href="./content/">CONTENT TYPES & TAGS</a></li>
            <li class="last"><a href="./internationalization/">LANGUAGES</a></li>            
        </ul>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>SITE ADMINISTRATION</h1>

    <p>This management area allows you to manage your global site configurations and available content types.</p>
    
    <p>
        <strong><a href="./themes/">Themes</a></strong><br />
        Manage your site themes and templates.
    </p>

    <p>
        <strong><a href="./configuration/default.aspx">Site Configuration</a></strong><br />
        Manage site-wide configuration and settings.
    </p>

    <p>
        <strong><a href="./structure/">Site Structure</a></strong><br />
        Manage the site structure and organization. Add/Edit/Delete paths and pages.
    </p>

    <p>
        <strong><a href="./content/">Content</a></strong><br />
        Manage the available CMS content types and tags.
    </p>

    <p>
        <strong><a href="./internationalization/">Internationalization</a></strong><br />
        Manage the languages that are available on the site and resource files.
    </p>

</asp:Content>

