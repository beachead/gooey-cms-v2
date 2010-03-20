<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="gooeycms.webrole.control.auth.Default" %>
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
            <li class=""><a href="./internationalization/">LANGUAGES</a></li>            
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
This management area allows you to manage your global site configurations and available content types.
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
<h1 class="main">Site Administration</h1>
<a href="./themes/">Themes</a>
<div class="content-item">Manage your site themes and templates</div>

<a href="./configuration/default.aspx">Site Configuration</a>
<div class="content-item">Manage site-wide configuration and settings.</div>

<a href="./structure/">Site Structure</a>
<div class="content-item">Manage the site structure and organization. Add/Edit/Delete paths and pages.</div>

<a href="./content/">Content</a>
<div class="content-item">Manage the available CMS content types and tags.</div>

<a href="./internationalization/">Internationalization</a>
<div class="content-item">Manage the languages that are available on the site and resource files.</div>

</asp:Content>

