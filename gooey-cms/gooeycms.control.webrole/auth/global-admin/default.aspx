<%@ Page Title="" Language="C#" MasterPageFile="~/SecureNoNavigation.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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

    <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>

    <div>
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
        <telerik:RadAjaxPanel ID="SiteSetupPanel" LoadingPanelID="LoadingPanel" runat="server">
        <table>
            <tr>
                <td><b>GooeyCMS Site Setup Status</b></td>
            </tr>
            <tr>
                <td>Deployment Mode</td>
                <td><asp:Image ID="DevModeStatusImage" runat="server" /></td>
                <td><asp:Label ID="LblDevMode" runat="server" /></td>            
            </tr>
            <tr>
                <td>Membership Roles</td>
                <td><asp:Image ID="MembershipStatusImage" runat="server" /></td>
                <td><asp:LinkButton ID="BtnSetupMembership" OnClick="BtnSetupMembership_Click" Text="Setup" runat="server" /></td>
            </tr>
            <tr>
                <td>Flash Support</td>
                <td><asp:Image ID="FlashStatusImage" runat="server" /></td>
                <td><asp:LinkButton ID="BtnSetupFlash" OnClick="BtnSetupFlash_Click" Text="Configure Flash Support" runat="server" /></td>
            </tr>
            <tr>
                <td>Paypal Payment Mode:</td>
                <td><asp:Image ID="PaypalStatusImage" runat="server" /></td>
                <td><asp:LinkButton ID="BtnTogglePaypal" OnClick="BtnTogglePaypal_Click" Text="" runat="server" /></td>
            </tr>
        </table>
        </telerik:RadAjaxPanel>
    </div>

</asp:Content>
