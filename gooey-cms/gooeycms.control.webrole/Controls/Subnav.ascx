<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Subnav.ascx.cs" Inherits="Gooeycms.Webrole.Control.Controls.Subnav" %>

<asp:MultiView ID="mvSubnav" runat="server">

    <asp:View ID="globaladmin" runat="server">
        <ul>
            <li><asp:HyperLink ID="globaladmin_default" NavigateUrl="~/auth/global-admin/default.aspx" Text="General Settings" runat="server" /></li>
        </ul>
    </asp:View>

    <!-- Dashboard -->
    <asp:View ID="dashboard" runat="server">
        <ul>
            <li><asp:HyperLink ID="dashboard_home" navigateUrl="~/auth/dashboard.aspx" Text="Dashboard" runat="server" /></li>
            <li><asp:HyperLink ID="dashboard_new" NavigateUrl="http://store.gooeycms.net/signup/" Text="Register New Site" runat="server"/></li>
            <li class="last"><asp:HyperLink ID="dashboard_purchase" NavigateUrl="http://store.gooeycms.net/" Text="Purchase Sites" runat="server" /></li> 
        </ul>    
    </asp:View>

    <!-- Developers -->
    <asp:View ID="developer" runat="server">
        <ul>
            <li><asp:HyperLink ID="developer_home" NavigateUrl="~/auth/developer/default.aspx" Text="Developers Home" runat="server" /></li>
            <li><asp:HyperLink ID="developer_new" NavigateUrl="~/auth/developer/site.aspx" Text="Package a New Site" runat="server" OnPreRender="AppendGUID" /></li>
            <li class="last"><asp:HyperLink ID="developer_settings" NavigateUrl="~/auth/developer/settings.aspx" Text="Developer Settings" runat="server" /></li>
        </ul>    
    </asp:View>    

    <!-- Manage Content -->
    <asp:View ID="content" runat="server">
        <ul>
            <li><asp:HyperLink ID="content_home" NavigateUrl="~/auth/Content/Default.aspx" Text="Manage Content" runat="server" /></li>
            <li><asp:HyperLink ID="content_new" NavigateUrl="~/auth/Content/Add.aspx" Text="Add Content" runat="server" /></li>
            <li><asp:HyperLink ID="content_promtion" NavigateUrl="~/auth/Promotion/Default.aspx" Text="Promotion" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="content_contenttypes" NavigateUrl="~/auth/Content/ContentTypes.aspx" Text="Manage Content Types" runat="server" /></li>
        </ul>
    </asp:View>

    <!-- Manage  Pages -->
    <asp:View ID="pages" runat="server">
        <ul>
            <li><asp:HyperLink ID="pages_home" NavigateUrl="~/auth/Pages/Default.aspx" Text="Manage Pages" runat="server" /></li>
            <li><asp:HyperLink ID="pages_promotion" NavigateUrl="~/auth/Promotion/Default.aspx" Text="Promotion" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="pages_manageredirects" NavigateUrl="~/auth/Pages/Redirects.aspx" Text="Manage Redirects" runat="server" /></li>
        </ul>
    </asp:View>

    <asp:View ID="users" runat="server">
        <ul>
            <li class="last"><asp:HyperLink ID="users_default" NavigateUrl="~/auth/Users/Default.aspx" Text="Manage Users" runat="server" /></li>
        </ul>
    </asp:View>

    <!-- Manage  Promotion -->
    <asp:View ID="promotion" runat="server">
        <ul>
            <li><asp:HyperLink ID="promotion_promotion" NavigateUrl="~/auth/Promotion/Default.aspx" Text="Promotion" runat="server" /></li>
            <li><asp:HyperLink ID="promotion_managepages" NavigateUrl="~/auth/Pages/Default.aspx" Text="Manage Pages" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="promotion_managecontent" NavigateUrl="~/auth/Content/Default.aspx" Text="Manage Content" runat="server" /></li>
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
            <% if (Gooeycms.Business.Util.CurrentSite.Restrictions.IsJavascriptAllowed) { %>
            <li><asp:HyperLink ID="templates_javascript" NavigateUrl="~/auth/Themes/Javascript.aspx" Text="JavaScript" runat="server" OnPreRender="AppendQuerystring" /></li>
            <% } %>
            <li class="last"><asp:HyperLink ID="templates_images" NavigateUrl="~/auth/Themes/ImageBrowser.aspx" Text="Images" runat="server" OnPreRender="AppendQuerystring" onclick="window.open(this.href, 'imageBrowser','width=700,height=450,statusbar=no,menubar=no,centerscreen=yes');return false;" /></li>
        </ul>
    </asp:View>

    <asp:View ID="global_admin_developer" runat="server">
        <ul>
            <li><asp:HyperLink ID="global_admin_developer_approval" NavigateUrl="~/auth/global-admin/Developer/Default.aspx" Text="Approve Packages" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="global_admin_developer_categories" NavigateUrl="~/auth/global-admin/Developer/Categories.aspx" Text="Manage Categories" runat="server" /></li>
        </ul>
    </asp:View>

    <asp:View ID="global_admin_contenttypes" runat="server">
        <ul>
            <li><asp:HyperLink ID="global_admin_contenttypes_default" NavigateUrl="~/auth/global-admin/ContentTypes/Default.aspx" Text="Content Types" runat="server" /></li>
        </ul>
    </asp:View>

    <asp:View ID="global_admin_subscriptions" runat="server">
        <ul>
            <li><asp:HyperLink ID="global_admin_subscriptions_pending" NavigateUrl="~/auth/global-admin/Subscriptions/default.aspx" Text="Pending Renewals" runat="server" /></li>
            <li class="last"><asp:HyperLink ID="global_admin_subscriptions_all" NavigateUrl="~/auth/global-admin/Subscriptions/AllSubscriptions.aspx" Text="All Subscriptions" runat="server" /></li>
        </ul>
    </asp:View>

</asp:MultiView>
