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
<div id="head-line" class="main">MANAGE SUBSCRIPTION (<asp:Label ID="LblDomain" runat="server" />)</div>

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
        <table>
            <tr>
                <td colspan="2">
                    <label>Username</label><br />
                    <asp:Label ID="LblUsername" runat="server" />
                </td>
            </tr>
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
            <tr>
                <td colspan="2">
                    <label>Company</label><br />
                    <asp:TextBox ID="TxtCompany" Width="300px" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="BtnUpdateUserInfo" OnClick="BtnUpdateUserInfo_Click" Text="Update Information" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>

    <div style="padding-top:10px;">
    <fieldset>
        <legend>Subscription Information</legend>
        <table>
            <tr>
                <td colspan="2" style="padding-right:10px;">Current Gooey Plan: <asp:Label ID="LblCurrentPlan" runat="server" /> (<asp:Label ID="LblPlanCost" runat="server" />)</td>
            </tr>
            <tr>
                <td>
                    <asp:MultiView ID="UpgradeOptions" runat="server">
                        <asp:View ID="UpgradeAvailable" runat="server">
                            <asp:LinkButton ID="BtnUpgradePlan" OnClick="BtnUpgradePlan_Click" Text="Upgrade to Business Plan" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="BtnCancelPlan" OnClick="BtnCancelPlan_Click" Text="Cancel Account" OnClientClick="return confirm('Are you sure you want to cancel this subscription?');" runat="server" />                            
                        </asp:View>
                        <asp:View ID="DowngradeAvailable" runat="server">
                            <asp:LinkButton ID="BtnDowngradePlan" OnClick="BtnDowngradePlan_Click" Text="Downgrade to Free Plan" OnClientClick="return confirm('Are you sure you want to immediately downgrade your subscription?');" runat="server" />                        
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="BtnCancelPlan2" OnClick="BtnCancelPlan_Click" Text="Cancel Account" OnClientClick="return confirm('Are you sure you want to cancel this subscription?');" runat="server" />                            
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>

        <asp:Panel ID="PanelBusinessPlan" runat="server">
        <div style="padding-top:10px;">
        <table style="width:100%;">
            <tr>
                <td>Paypal Billing Id:&nbsp;<asp:Label ID="LblPaypalBillingId" runat="server" /></td>
                <td>Paypal Status:&nbsp;<asp:Label ID="LblPaypalStatus" runat="server" /></td>
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

        <div style="padding-top:10px;">
            <label>Subscription Options:</label><br />
            <asp:CheckBox ID="ChkCampaigns" Text="Campaign Integration" runat="server" /><asp:Label ID="LblCampaignCost" runat="server" /><br />
            <asp:CheckBox ID="ChkSalesforce" Text="Salesforce Integration" runat="server" /><asp:Label ID="LblSalesforceCost" runat="server" /><br />
            <asp:LinkButton ID="BtnUpdateOptions" OnClick="BtnUpdateOptions_Click" Text="Update Options" runat="server" />
        </div>
        </asp:Panel>
    </fieldset>
    </div>

    <div style="padding-top:10px;">
    <fieldset>
        <legend>Domain Settings</legend>

        <div>
            <label>Site Name</label><br />
            <asp:Label ID="LblSiteName" runat="server" />
        </div>
        
        <div style="padding-top:10px;">
        <label>Production Domain</label> <br />
        <asp:TextBox ID="TxtProductionDomain" Width="300px" runat="server" />
        </div>

        <div style="padding-top:10px;">
        <label>Staging Domain</label><br />
        <asp:TextBox ID="TxtCustomStagingDomain" Width="300px" runat="server" />
        </div>

        <div>
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
</asp:Content>
