<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <beachead:Subnav ID="Subnav" runat="server" NavSection="GlobalAdmin" NavItem="default" />
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
            <tr>
                <td>Paypal PDT Token:</td>
                <td><asp:Image ID="PaypalPdtTokenImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="PaypalPdtTokenLink" runat="server" />
                    <telerik:RadToolTip ID="PaypalPdtTokenTooltip" Skin="Default" Width="250" TargetControlID="PaypalPdtTokenLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Paypal API Username:</td>
                <td><asp:Image ID="PaypalApiUsernameImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="PaypalApiUsernameLink" runat="server" />
                    <telerik:RadToolTip ID="PaypalApiUsernameTooltip" Skin="Default" Width="250" TargetControlID="PaypalApiUsernameLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Paypal API Password:</td>
                <td><asp:Image ID="PaypalApiPasswordImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="PaypalApiPasswordLink" runat="server" />
                    <telerik:RadToolTip ID="PaypalApiPasswordTooltip" Skin="Default" Width="250" TargetControlID="PaypalApiPasswordLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Paypal API Signature:</td>
                <td><asp:Image ID="PaypalApiSignatureImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="PaypalApiSignatureLink" runat="server" />
                    <telerik:RadToolTip ID="PaypalApiSignatureTooltip" Skin="Default" Width="250" TargetControlID="PaypalApiSignatureLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Default Template:</td>
                <td><asp:Image ID="DefaultTemplateImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="LnkDefaultTemplate" runat="server" />
                    <telerik:RadToolTip ID="TooltipDefaultTemplate" Skin="Default" Width="250" TargetControlID="LnkDefaultTemplate" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Default Template Name:</td>
                <td><asp:Image ID="DefaultTemplateNameImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="DefaultTemplateNameLink" runat="server" />
                    <telerik:RadToolTip ID="DefaultTemplateNameTooltip" Skin="Default" Width="250" TargetControlID="DefaultTemplateNameLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Default Theme Name:</td>
                <td><asp:Image ID="DefaultThemeNameImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="DefaultThemeNameLink" runat="server" />
                    <telerik:RadToolTip ID="DefaultThemeNameTooltip" Skin="Default" Width="250" TargetControlID="DefaultThemeNameLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Default Theme Description:</td>
                <td><asp:Image ID="DefaultThemeDescriptionImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="DefaultThemeDescriptionLink" runat="server" />
                    <telerik:RadToolTip ID="DefaultThemeDescriptionTooltip" Skin="Default" Width="250" TargetControlID="DefaultThemeDescriptionLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Default Homepage:</td>
                <td><asp:Image ID="DefaultHomepageImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="LnkDefaultHomepage" runat="server" />
                    <telerik:RadToolTip ID="TooltipDefaultHomepage" Skin="Default" Width="250" TargetControlID="LnkDefaultHomepage" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Default CMS Domain:</td>
                <td><asp:Image ID="DefaultCmsDomainImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="DefaultCmsDomainLink" runat="server" />
                    <telerik:RadToolTip ID="DefaultCmsDomainTooltip" Skin="Default" Width="250" TargetControlID="DefaultCmsDomainLink" runat="server">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>Admin Site Domain:</td>
                <td><asp:Image ID="AdminSiteHostImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="AdminSiteHostLink" runat="server" />
                    <telerik:RadToolTip ID="AdminSiteHostTooltip" Skin="Default" Width="250" TargetControlID="AdminSiteHostLink" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Default Page Name:</td>
                <td><asp:Image ID="DefaultPageNameImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="DefaultPageNameLink" runat="server" />
                    <telerik:RadToolTip ID="DefaultPageNameTooltip" Skin="Default" Width="250" TargetControlID="DefaultPageNameLink" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Default Staging Prefix:</td>
                <td><asp:Image ID="DefaultStaingPrefixImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="DefaultStaingPrefixLink" runat="server" />
                    <telerik:RadToolTip ID="DefaultStaingPrefixTooltip" Skin="Default" Width="250" TargetControlID="DefaultStaingPrefixLink" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Default Async Timeout:</td>
                <td><asp:Image ID="DefaultAsyncTimeoutImage" runat="server" /></td>
                <td>
                    <asp:Hyperlink ID="DefaultAsyncTimeoutLink" runat="server" />
                    <telerik:RadToolTip ID="DefaultAsyncTimeoutTooltip" Skin="Default" Width="250" TargetControlID="DefaultAsyncTimeoutLink" runat="server" />
                </td>
            </tr>
        </table>
        </telerik:RadAjaxPanel>
    </div>

    <telerik:RadWindowManager ID="Singleton" Skin="Default" Modal="true" Height="500" Width="600" ShowContentDuringLoad="false" AutoSize="false" VisibleStatusbar="false" Behaviors="Close,Move,Resize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
</asp:Content>
