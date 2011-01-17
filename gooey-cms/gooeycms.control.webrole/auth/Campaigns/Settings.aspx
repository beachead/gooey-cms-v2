<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Settings" %>
<%@ MasterType VirtualPath="~/Secure.master" %>
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
    <h1>CAMPAIGN SETTINGS</h1>
    <p>Configure your settings for Google Analytics and Salesforce below.</hp>

<div id="mainTabContainer" dojoType="dijit.layout.TabContainer" style="height:600px;width:800px;">
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
        <div dojoType="dijit.TitlePane" title="Salesforce Fields">
            <table>
                <tr>
                    <td>Successfully Authenticated:</td>
                    <td><asp:Label ID="LblSalesforceAuthenticated" runat="server" /></td>
                </tr>
                <tr>
                    <td style="vertical-align:top;">Available Lead Fields</td>
                    <td>
                        <anthem:ListBox ID="LstSalesforceAvailableFields" onchange="settext(this);" SelectionMode="Single" Rows="10" runat="server" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="4" style="padding-top:12px;"><b>Custom Mappings:</b> <i>(Choose field above)</i></td>
                </tr>
                <tr>
                    <td>SalesForce Field:</td>
                    <td><asp:TextBox ID="TxtSalesforceField" Width="225px" runat="server" /></td>
                    <td>Custom Name:</td>
                    <td>
                        <asp:TextBox ID="TxtSalesforceFriendly" runat="server" />&nbsp;
                        <asp:Button ID="BtnAddCustomMapping" OnClick="BtnAddCustomMapping_Click" Text="Add Mapping" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top;">Existing Mappings:</td>
                    <td>
                        <asp:ListBox ID="LstCustomMappings" Width="350px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="BtnRemoveMapping" Width="350px" OnClick="BtnRemoveCustomMapping_Click" Text="Remove Mapping" runat="server" /></td>
                </tr>

                <script language="javascript" type="text/javascript">
                    function settext(list) {
                        var item = list.options[list.selectedIndex].value;
                        if (item)
                            document.getElementById('<%= TxtSalesforceField.ClientID %>').value = item;
                    }
                </script>
            </table>
        </div>
    </div>

    <div id="phone-panel" dojoType="dijit.layout.ContentPane" title="Phone Settings" style="display:none;">
        <table>
            <tr>
                <td colspan="2">
                    <b>Phone Integration</b><br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:Label ID="LblPhoneStatus" runat="server" /></td>
            </tr>
            <tr>
                <td style="vertical-align:top;padding-top:4px;">Would you prefer a local or toll-free number?</td>
                <td>
                    <asp:RadioButton ID="RdoLocalType" GroupName="NumberType" Text="Local Number" runat="server" />&nbsp; 
                    <asp:RadioButton ID="RdoTollFreeType" GroupName="NumberType" Text="Toll-Free Number" runat="server" />
                </td>

            </tr>
            <tr>
                <td>Default Area Code (<i>applies to local numbers only</i>)</td>
                <td>
                    <asp:TextBox ID="TxtAreaCode" Width="75px" MaxLength="3" dojoType="dijit.form.ValidationTextBox" ValidationGroup="AreaCode" promptMessage="Input the area code used for your local campaign phone numbers." regExp="\d{3}" invalidMessage="Area codes must contain exactly three digits" runat="server" />
                    <asp:RegularExpressionValidator ControlToValidate="TxtAreaCode" ValidationGroup="AreaCode" ValidationExpression="\d{3}" Display="None" runat="server" />
            </tr>
            <tr>
                <td>Default Forwarding Number</td>
                <td>
                    <asp:TextBox ID="TxtForwardNumber" Width="150px" MaxLength="12" dojoType="dijit.form.ValidationTextBox" ValidationGroup="AreaCode" promptMessage="Input the default forwarding number for your company." regExp="\d{3}-\d{3}-\d{4}" invalidMessage="Forwarding numbers must be in the following format: 555-555-5555" runat="server" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TxtForwardNumber" ValidationGroup="AreaCode" ValidationExpression="\d{3}-\d{3}-\d{4}" Display="None" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator" ControlToValidate="TxtForwardNumber" ValidationGroup="AreaCode" Text="*" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Site Phone Number Format
                    <div style="font-size:9px;padding-right:5px;">
                        Use the following tags to specify the fields in your phone number<br />
                        {0} - Area code <br />
                        {1} - Prefix <br />
                        {2} - Line Number <br />
                        (example: ({0} {1}-{2}) products (555) 555-5555)                    
                    </div>
                </td>
                <td style="vertical-align:top;">
                    <asp:TextBox ID="TxtPhoneFormat" Width="150px" dojoType="dijit.form.ValidationTextBox" ValidationGroup="AreaCode" promptMessage="Input the format to use when displaying phone numbers of your webiste." regExp=".*" invalidMessage="Input the format to use when displaying phone numbers on your website" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="TxtForwardNumber" ValidationGroup="AreaCode" Text="*" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right;">
                    <asp:Button ID="BtnPhoneSettings" OnClick="BtnPhoneSettings_Click" Text="Save" ValidationGroup="AreaCode" runat="server" />
                </td>
            </tr>
        </table>

        <fieldset>
            <legend>Phone Account Status</legend>
            <b>Remaining Phone Numbers:</b>&nbsp;<asp:Label ID="LblRemainingPhoneNumbers" runat="server" />
            <br /><br />

            <b>All Assigned Phone Numbers:</b>
            <table>
                <asp:Repeater ID="AssignedPhoneNumbers" runat="server">
                    <HeaderTemplate>
                        <tr visible='<%# ((ICollection)AssignedPhoneNumbers.DataSource).Count == 0 ? true : false %>' runat="server">
                            <td>
                                There currently are no phone numbers assigned to this subscription.<br />
                                You may assign a phone number to individual campaigns from the <a href="Default.aspx">Manage Campaigns</a> page.
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem,"FriendlyPhoneNumber") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>

        </fieldset>
    </div>
</div>
</asp:Content>
