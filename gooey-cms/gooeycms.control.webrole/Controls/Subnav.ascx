<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Subnav.ascx.cs" Inherits="Gooeycms.Webrole.Control.Controls.Subnav" %>

<asp:MultiView ID="mvSubnav" runat="server">

    <asp:View ID="manageTemplates" runat="server">
        <ul>
            <li><asp:HyperLink ID="manageTheme" NavigateUrl="~/auth/Themes/Default.aspx" Text="Manage Theme" runat="server" /></li>
            <li><asp:HyperLink ID="templates" NavigateUrl="~/auth/Themes/Templates.aspx" Text="Templates" runat="server" OnPreRender="appendTid" /></li>
            <li><asp:HyperLink ID="header" NavigateUrl="~/auth/Themes/Header.aspx" Text="Header" runat="server" OnPreRender="appendTid" /></li>
            <li><asp:HyperLink ID="footer" NavigateUrl="~/auth/Themes/Footer.aspx" Text="Footer" runat="server" OnPreRender="appendTid" /></li>
            <li><asp:HyperLink ID="css" NavigateUrl="~/auth/Themes/Stylesheet.aspx" Text="CSS" runat="server" OnPreRender="appendTid" /></li>
            <li><asp:HyperLink ID="javascript" NavigateUrl="~/auth/Themes/Javascript.aspx" Text="JavaScript" runat="server" OnPreRender="appendTid" /></li>
            <li class="last"><asp:HyperLink ID="images" NavigateUrl="~/auth/Themes/ImageBrowser.aspx" Text="Images" runat="server" OnPreRender="appendTid" /></li>
        </ul>
    </asp:View>

</asp:MultiView>
