﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Subnav.ascx.cs" Inherits="Gooeycms.Webrole.Control.Controls.Subnav" %>

<asp:MultiView ID="mvSubnav" runat="server">

    <!-- Manage Content -->
    <asp:View ID="content" runat="server">
        <ul>
            <li><asp:HyperLink ID="content_home" NavigateUrl="~/auth/Content/Default.aspx" Text="Manage Content" runat="server" /></li>
            <li><asp:HyperLink ID="content_new" NavigateUrl="~/auth/Content/Add.aspx" Text="Add Content" runat="server" /></li>
            <li><asp:HyperLink ID="content_promtion" NavigateUrl="~/auth/Content/Promotion.aspx" Text="Promotion" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="content_contenttypes" NavigateUrl="~/auth/Content/ContentTypes.aspx" Text="Manage Content Types" runat="server" /></li>
        </ul>
    </asp:View>

    <!-- Manage  Pages -->
    <asp:View ID="pages" runat="server">
        <ul>
            <li><asp:HyperLink ID="pages_home" NavigateUrl="~/auth/Pages/Default.aspx" Text="Manage Pages" runat="server" /></li>
            <li><asp:HyperLink ID="pages_new" NavigateUrl="~/auth/Pages/Edit.aspx" Text="Add New Page" runat="server" /></li>
            <li><asp:HyperLink ID="pages_promotion" NavigateUrl="~/auth/Pages/Promotion.aspx" Text="Promotion" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="pages_manageredirects" NavigateUrl="~/auth/Pages/Redirects.aspx" Text="Manage Redirects" runat="server" /></li>
        </ul>
    </asp:View>

    <!-- Manage  Campaigns -->
    <asp:View ID="campaigns" runat="server">
        <ul>
            <li><asp:HyperLink ID="campaigns_listing" NavigateUrl="~/auth/Campaigns/Default.aspx" Text="Manage Campaigns" runat="server" /></li>
            <li><asp:HyperLink ID="campaigns_new" NavigateUrl="~/auth/Campaigns/Create.aspx" Text="Create Campaign" runat="server" /></li>
            <li><asp:HyperLink ID="campaigns_leadreport" NavigateUrl="~/auth/Campaigns/Leads.aspx" Text="Lead Report" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="campaigns_campaignsettings" NavigateUrl="~/auth/Campaigns/Settings.aspx" Text="Campaign Settings" runat="server" /></li>
        </ul>
    </asp:View>

    <!-- Manage  Themes -->
    <asp:View ID="themes" runat="server">
        <ul>
            <li><asp:HyperLink ID="themes_theme" NavigateUrl="~/auth/Themes/Default.aspx" Text="Manage Themes" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="themes_add" NavigateUrl="~/auth/Themes/Add.aspx" Text="Add Theme" runat="server" /></li>
        </ul>
    </asp:View>

    <!-- Manage  Themes: Templates -->
    <asp:View ID="templates" runat="server">
        <ul>
            <li><asp:HyperLink ID="templates_theme" NavigateUrl="~/auth/Themes/Default.aspx" Text="Manage Themes" runat="server" /></li>
            <li><asp:HyperLink ID="templates_templates" NavigateUrl="~/auth/Themes/Templates.aspx" Text="Templates" runat="server" OnPreRender="AppendQuerystring" /></li>
            <li><asp:HyperLink ID="templates_header" NavigateUrl="~/auth/Themes/Header.aspx" Text="Header" runat="server" OnPreRender="AppendQuerystring" /></li>
            <li><asp:HyperLink ID="templates_footer" NavigateUrl="~/auth/Themes/Footer.aspx" Text="Footer" runat="server" OnPreRender="AppendQuerystring" /></li>
            <li><asp:HyperLink ID="templates_stylesheets" NavigateUrl="~/auth/Themes/Stylesheet.aspx" Text="Stylesheets" runat="server" OnPreRender="AppendQuerystring" /></li>
            <li><asp:HyperLink ID="templates_javascript" NavigateUrl="~/auth/Themes/Javascript.aspx" Text="JavaScript" runat="server" OnPreRender="AppendQuerystring" /></li>
            <li class="last"><asp:HyperLink ID="templates_images" NavigateUrl="~/auth/Themes/ImageBrowser.aspx" Text="Images" runat="server" OnPreRender="AppendQuerystring" onclick="window.open(this.href, 'imageBrowser','width=700,height=450,statusbar=no,menubar=no,centerscreen=yes');return false;" /></li>
        </ul>
    </asp:View>

    <asp:View ID="global_admin_developer" runat="server">
        <ul>
            <li><asp:HyperLink ID="global_admin_developer_approval" NavigateUrl="~/auth/global-admin/Developer/Default.aspx" Text="Approve Packages" runat="server" /></li>
        </ul>
    </asp:View>

</asp:MultiView>