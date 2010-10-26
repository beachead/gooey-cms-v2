<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Settings" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="campaigns" NavItem="campaignsettings" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">

    <script type="text/javascript">
        dojo.addOnLoad(function () { dijit.byId('mainTabContainer').selectChild('<% Response.Write(SelectedPanel); %>'); });
    </script>    

</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <h1>Campaign Settings</h1>
    <p>Configure your campaign settings below.</hp>

<div id="mainTabContainer" dojoType="dijit.layout.TabContainer" style="height:700px;overflow:auto;">
    <div id="analytics-panel" dojoType="dijit.layout.ContentPane" title="Google Analytics Setup" style="display:none;">
        <table>
            <tr>
                <td colspan="2">
                    <b>Google Analytics Integration</b><br />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top;padding-top:4px;">Enable Google Analytics</td>
                <td>
                    <asp:RadioButton ID="RdoGoogleEnabledYes" GroupName="GoogleEnabled" Text="Enabled" runat="server" />&nbsp; 
                    <asp:RadioButton ID="RdoGoogleEnabledNo" GroupName="GoogleEnabled" Text="Disabled" Checked="true" runat="server" />
                </td>

            </tr>
            <tr>
                <td>Google Analytics Account ID</td>
                <td>
                    <asp:TextBox ID="TxtGoogleAccountId" Width="175px" dojoType="dijit.form.ValidationTextBox" ValidationGroup="Google" promptMessage="Input the Google Web Profile ID or UA Number for this site. (e.g. UA-XXXXXXXX-Y)" regExp="UA-\d{4,10}-\d{1,2}" invalidMessage="format: UA-XXXXXXX-YY (where X's are your account number and Y is the profile number)" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right;">
                    <asp:Button ID="BtnSaveGoogle" OnClick="BtnSaveGoogle_Click" Text="Save" ValidationGroup="Google" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="salesforce-panel" dojoType="dijit.layout.ContentPane" title="SalesForce Setup" style="display:none;">
        <div dojoType="dijit.TitlePane" title="Login Information">
            <table>
            <tr>
                <td colspan="3">
                    <b>Salesforce.com Integration</b><br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding-bottom:10px;">Salesforce.com Integration: 
                <asp:RadioButton ID="RdoSalesforceEnabledYes" GroupName="SalesforceEnabled" runat="server" Text="Enabled" />&nbsp;
                <asp:RadioButton ID="RdoSalesforceEnabledNo" GroupName="SalesforceEnabled" runat="server" Text="Disabled" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding-bottom:5px;">
                    <b>Salesforce.com Login Information</b><br />
                    This account will be used as the default-owner for all leads.
                </td>
            </tr>
            <tr>
                <td>Login Username:</td>
                <td colspan="2">
                    <asp:TextBox ID="TxtSalesforceUsername" Width="200px"  runat="server" />
                    <asp:RequiredFieldValidator ID="Required1" ControlToValidate="TxtSalesforceUsername" Text="* Required" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Login Password:</td>
                <td colspan="2">
                    <asp:TextBox ID="TxtSalesforcePassword" TextMode="Password" Width="200px"  runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TxtSalesforcePassword" Text="* Required"  runat="server" />
                </td>
            </tr>    
            <tr>
                <td>Login Token:</td>
                <td colspan="2">
                    <asp:TextBox ID="TxtSalesforceToken" Width="205px" dojoType="dijit.form.ValidationTextBox" promptMessage="Input your salesforce.com security remote API security token" invalidMessage="You must input your salesforce.com security token" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TxtSalesforceToken" Text="*" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width:205px; text-align:right;"><asp:Button ID="SaveLogin" Text="Save" OnClick="BtnSaveLogin_Click"  runat="server" /></td>
                <td>&nbsp;</td>
            </tr>
            </table>
        </div>
        <div dojoType="dijit.TitlePane" title="Salesforce Information">
            <table>
                <tr>
                    <td>Successfully Authenticated:</td>
                    <td><asp:Label ID="LblSalesforceAuthenticated" runat="server" /></td>
                </tr>
                <tr>
                    <td style="vertical-align:top;">Available Lead Fields</td>
                    <td>
                        <asp:ListBox ID="LstSalesforceAvailableFields" SelectionMode="Multiple" Rows="10" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
</asp:Content>
