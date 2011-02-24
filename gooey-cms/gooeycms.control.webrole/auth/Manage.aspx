<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Manage" %>
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
<telerik:RadFormDecorator ID="FormDecorator" Skin="Default" EnableRoundedCorners="true" DecoratedControls="Fieldset,CheckBoxes,Textbox,Label" runat="server" />
<h1>MANAGE SITE: <asp:Label ID="LblDomain" runat="server" /></h1>

<p>This page allows you to upgrade your account and user settings and update the domain settings for your website.</p>
<br />
<asp:Panel ID="ManageEnablePanel" runat="server">
    <div style="padding:10px;">
        <b style="color:red;">This subscription is currently disabled.</b><br /><br />
        <asp:MultiView ID="EnableStatusView" runat="server">
            <asp:View ID="SuspendedView" runat="server">
                Your paypal billing agreement is currently inactive and can be reactivated simply by clicking the enable button below.
            </asp:View>
            <asp:View ID="CancelledView" runat="server">
                Your paypal billing agreement has been cancelled. Clicking enable will redirect you to Paypal to re-establish your monthly billing agreement
            </asp:View>
            <asp:View ID="FreeView" runat="server">
                You may immediately reactivate your free subscription by clicking the enable button below.
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:Button ID="BtnEnableSubscription" OnClick="BtnEnableSubscription_Click" Text="Re-Enable Subscription" runat="server" />
    </div>
</asp:Panel>

<asp:Panel ID="ManagePanel" runat="server">
<div style="width:50%;">
    <asp:Label ID="LblStatus" runat="server" />

    <fieldset>
        <legend>User Information</legend>
         <div style="padding-top:10px; padding-bottom:10px; padding-left:3px;">
                <label>Username: </label> <asp:Label ID="LblUsername" runat="server" />
             </div>
              <table>
                 <tr>
                 <td>
                    <label>First Name</label><br />
                    <asp:TextBox ID="TxtFirstname" runat="server" />
                </td>
                <td>
                    <label>Last Name</label><br />
                    <asp:TextBox ID="TxtLastname" runat="server" />
                </td>
            </tr>
            </table>
                    <div style="padding-top:10px; padding-bottom:10px; padding-left:3px;">
                    <label>Company</label><br />
                    <asp:TextBox ID="TxtCompany" Width="300px" runat="server" />
                    </div>
          <div style="padding-top:10px; padding-bottom:10px; padding-left:3px;">
                    <asp:LinkButton ID="BtnUpdateUserInfo" OnClick="BtnUpdateUserInfo_Click" Text="Update Information" runat="server" />
              </div>
    </fieldset>

    <div style="padding-top:20px;">
    <fieldset>
        <legend>Subscription Information</legend>
       <div style="padding-top:10px; padding-bottom:10px; padding-left:3px;">
       <table>
            <tr>
                <td colspan="2">Current Gooey Plan: <asp:Label ID="LblCurrentPlan" runat="server" /> (<asp:Label ID="LblPlanCost" runat="server" />)</td>
            </tr>
        </table>
        <div style="padding-top:10px; padding-left:3px;">
                <asp:MultiView ID="UpgradeOptions" runat="server">
                        <asp:View ID="UpgradeAvailable" runat="server">
                            <asp:LinkButton ID="BtnUpgradePlan" OnClientClick="show_options(); return false;" Text="Upgrade to Business Plan" runat="server" />
                            <telerik:RadWindow ID="UpgradeOptionsWindow" Skin="Default" Modal="true" Title="Select Options" VisibleTitleBar="false" VisibleStatusbar="false"
                                              Width="550px" Height="300px"  runat="server">
                                <ContentTemplate>
                                    <telerik:RadAjaxLoadingPanel ID="UpgradeLoadingPanel" Skin="Default" runat="server" />
                                    <telerik:RadAjaxPanel ID="AjaxUpgradePanel" LoadingPanelID="UpgradeLoadingPanel" runat="server">
                                    <div style="padding:5px;">
                                    <fieldset style="width:500px;">
                                        <legend><span style="color:#4395F1;">Subscription Cost</span></legend>
                                        You will be charged <b><asp:Label ID="LblSubscriptionPrice" runat="server" /> per month</b> starting on <b><asp:Label ID="LblBillingStartDate" runat="server" /></b> <br />
                                        <label>(plus any options added below)</label>
                                    </fieldset>
                                    <br />
                                    <fieldset style="width:450px;">
                                        <legend><span style="color:#4395F1;">Subscription Options</span></legend>
                                        <table>
                                            <tr>
                                                <td><asp:CheckBox ID="ChkUpgradeCampaignOption" AutoPostBack="true" OnCheckedChanged="RecalculateCost_Click" runat="server" /></td>
                                                <td>Campaign Integration - <asp:Label ID="LblCampaignPrice" runat="server" /></td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align:top;"><asp:CheckBox ID="ChkUpgradeSalesforceOption" AutoPostBack="true" OnCheckedChanged="RecalculateCost_Click" runat="server" /></td>
                                                <td>
                                                    Salesforce Integration - <asp:Label ID="LblSalesforcePrice" runat="server" />
                                                    <br />
                                                    <label>(requires a Salesforce Pro + API, Enterprise Account or Unlimited Account)</label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <telerik:RadButton ID="BtnUpgradeAccount" ButtonType="LinkButton" OnClick="BtnUpgradePlan_Click" Text="Upgrade Account" runat="server" />&nbsp;&nbsp;
                                    <asp:LinkButton ID="BtnCloseWindow" OnClientClick="close_window(); return false;" Text="Cancel" runat="server" />
                                    <br /><br />
                                    <label><b>Your account will be billed <asp:Label ID="LblTotalAmount" runat="server" /> per month.</b><br /></label>
                                    </div>
                                    </telerik:RadAjaxPanel>
                                </ContentTemplate>
                            </telerik:RadWindow>
                            <script type="text/javascript" language="javascript">
                                function show_options() {
                                    var wnd = $find("<%=UpgradeOptionsWindow.ClientID %>");
                                    wnd.show();
                                }

                                function close_window() {
                                    var wnd = $find("<%=UpgradeOptionsWindow.ClientID %>");
                                    wnd.close();
                                }
                            </script>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="BtnCancelPlan" OnClick="BtnCancelPlan_Click" Text="Cancel Account" OnClientClick="return confirm('Are you sure you want to cancel this subscription?\r\n\r\nWARNING: This will also release any phone numbers that you may have associated with your campaigns.');" runat="server" />                            
                        </asp:View>
                        <asp:View ID="DowngradeAvailable" runat="server">
                            <asp:LinkButton ID="BtnDowngradePlan" OnClick="BtnDowngradePlan_Click" Text="Downgrade to Free Plan" OnClientClick="return confirm('Are you sure you want to immediately downgrade your subscription?');" runat="server" />                        
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="BtnCancelPlan2" OnClick="BtnCancelPlan_Click" Text="Cancel Account" OnClientClick="return confirm('Are you sure you want to cancel this subscription?');" runat="server" />                            
                        </asp:View>
                    </asp:MultiView>
          </div>       
            </div>
       

        <asp:Panel ID="PanelBusinessPlan" runat="server">
        <div style="padding-top:10px; padding-left:3px;">
        <table style="width:100%;">
            <tr>
                <td>Paypal Billing Id:&nbsp;<asp:Label ID="LblPaypalBillingId" runat="server" /></td>
                <td>Paypal Subscription:&nbsp;<asp:Label ID="LblPaypalStatus" runat="server" /></td>
            </tr>
            <tr>
            <asp:MultiView ID="PaypalTrialView" runat="server">
                <asp:View ID="InTrialPeriodView" runat="server">
                    <td colspan="2">
                        <label>Currently In Trial Period: </label>
                        <asp:Label ID="LblTrialRemaining" runat="server" /> days remaining
                    </td>
                </asp:View>

                <asp:View ID="OutOfTrialPeriodView" runat="server">
                    <td colspan="2">
                        <label>Next Billing Date:</label>
                        <asp:Label ID="LblNextBillingDate" runat="server" />                
                    </td>                
                </asp:View>
            </asp:MultiView>
            </tr>
        </table>
        </div>

        <div style="padding-top:10px; padding-left:3px;">
            <label>Subscription Options:</label>
        </div>
        <div style="padding-top:5px; padding-bottom:5px; padding-left:15px;">
            <asp:CheckBox ID="ChkCampaigns" Text="Campaign Integration" runat="server" /><asp:Label ID="LblCampaignCost" runat="server" /><br />
            <asp:CheckBox ID="ChkSalesforce" Text="Salesforce Integration" runat="server" /><asp:Label ID="LblSalesforceCost" runat="server" /><br />
        </div>

        <div style="padding-top:10px; padding-bottom:10px; padding-left:3px;">
            <asp:LinkButton ID="BtnUpdateOptions" OnClick="BtnUpdateOptions_Click" Text="Update Options" runat="server" />
        </div>
        </asp:Panel>
    </fieldset>
    </div>

    <div style="padding-top:20px; padding-bottom:10px;">
    <fieldset>
        <legend>Domain Settings</legend>

        <div style="padding-top:10px; padding-bottom:10px; padding-left:3px;">
            <label>Site Name: </label><asp:Label ID="LblSiteName" runat="server" />.gooeycms.net
        </div>

        <div>
            <label>Site Language</label><br />
            <asp:DropDownList ID="LstSiteCulture" runat="server" />
        </div>
        
        <div style="padding-top:10px; padding-left:3px;">
        <label>Production Domain</label> <br />
        <asp:TextBox ID="TxtProductionDomain" Width="300px" runat="server" />
        </div>

        <div style="padding-top:10px; padding-left:3px;">
        <label>Staging Domain</label><br />
        <asp:TextBox ID="TxtCustomStagingDomain" Width="300px" runat="server" />
        </div>

        <div style="padding-top:10px; padding-left:3px;">
        <label>Allow remote support</label><asp:RadioButton ID="RdoRemoteSupportYes" GroupName="RemoteSupport" Text="Yes" runat="server" />&nbsp;<asp:RadioButton ID="RdoRemoteSupportNo" GroupName="RemoteSupport" Text="No" runat="server" />
        </div>
       <div style="padding-top:10px; padding-bottom:10px; padding-left:3px;">
        <asp:LinkButton ID="LnkUpdateDomain" OnClick="LnkUpdateDomain_Click" Text="Update Domain Settings" runat="server" />
        </div>
    </fieldset>
    </div>

    <telerik:RadScriptBlock ID="ScriptBlock" runat="server">
    <script language="javascript" type="text/javascript">
        function set_default(name, txt) {
            var obj = document.getElementById(txt);
            obj.value = name;
        }
    </script>
    </telerik:RadScriptBlock>
</div>
</asp:Panel>
<br /><br /><br /><br />
</asp:Content>