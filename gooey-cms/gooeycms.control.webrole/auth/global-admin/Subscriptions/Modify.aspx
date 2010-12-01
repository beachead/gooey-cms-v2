<%@ Page Title="Subscription Management"  Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Subscriptions.Modify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        table.stats
        {text-align: center;
        font-family: Verdana, Geneva, Arial, Helvetica, sans-serif ;
        font-weight: normal;
        font-size: 11px;
        color: #fff;
        width: 100%;
        background-color: #666;
        border: 0px;
        border-collapse: collapse;
        border-spacing: 0px;}

        table.stats td
        {background-color: #CCC;
        color: #000;
        padding: 4px;
        text-align: left;
        border: 1px #fff solid;}

        table.stats td.hed
        {background-color: #666;
        color: #fff;
        padding: 4px;
        text-align: left;
        border-bottom: 2px #fff solid;
        font-size: 12px;
        font-weight: bold;}     
    </style>

    <asp:Panel ID="EditPanel" runat="server">
    <table class="stats">
        <tr>
            <td class="hed" colspan="3">Subscription General</td>
            <td class="hed" style="text-align:right;padding-right:5px;">Status: <b><asp:Label ID="LblProfileStatus" runat="server" /></b></td>
        </tr>
        <tr>
            <td>Subscription Id</td>
            <td><asp:Label ID="LblGuid" runat="server" /></td>
            <td>Paypal Profile</td>
            <td><asp:TextBox ID="TxtPaypalProfile" runat="server" />&nbsp;<asp:Button ID="BtnUpdatePayalProfile" Text="Update Profile" OnClick="BtnUpdatePaypalProfile_Click" OnClientClick="return confirm('Are you sure want to update the paypal profile id?\r\n\r\nWARNING: Incorrectly modifying this value can cause the subscription to be no longer associated with the paypal recurring payment profile');" runat="server" /></td>
        </tr>
        <tr>
            <td>Subscription Plan</td>
            <td>
                <asp:DropDownList ID="LstSubscriptionPlans" runat="server" />&nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="ChkSalesforceOption" Text="Salesforce" runat="server" />&nbsp;
                <asp:CheckBox ID="ChkCampaigns" Text="Campaigns" runat="server" />&nbsp;
                <asp:Button ID="BtnUpdateSubscriptionPlan" Text="Update Plan" OnClick="BtnUpdateSubscriptionPlan_Click" OnClientClick="return confirm('Are you sure you want to update the subscription plan and options?\r\nNOTE: This will not change the monthly subscription price.');" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Production Domain</td>
            <td>    
                <asp:Label ID="LblProductionDomain" runat="server" />
            </td>
            <td>Staging Domain</td>
            <td><asp:Label ID="LblStagingDomain" runat="server" /></td>
        </tr>
        <tr>
            <td>Date Created</td>
            <td colspan="3"><asp:Label ID="LblCreated" runat="server" /></td>
        </tr>
        <tr>
            <td>Trial Period Active</td>
            <td colspan="3">
                <asp:Label ID="LblTrialPeriodRemaining" runat="server" />
                &nbsp;&nbsp;
                <asp:Button ID="BtnExtendTrialPeriod" Text="Extend Trial Period" Visible="false" OnClick="BtnExtendTrialPeriod_Click" OnClientClick="return confirm('Are you sure you want to extend this trial period by one month?');" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Last Billing</td>
            <td><asp:Label ID="LblLastBilling" runat="server" /></td>
            <td>Amount</td>
            <td><asp:Label ID="LblLastBillingAmount" runat="server" /></td>
        </tr>
        <tr>
            <td>Next Billing</td>
            <td><asp:Label ID="LblNextBilling" runat="server" /></td>
            <td>Amount</td>
            <td><asp:Label ID="LblNextBillingAmount" runat="server" /> (<asp:Label ID="LblNormalAmt" runat="server" /> recurring amount)</td>
        </tr>
        <tr>
            <td class="hed" colspan="4">Subscription Actions</td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button ID="BtnEnableDisable" Text="" OnClick="BtnEnableDisable_Click" runat="server" />

                <div style="float:right;">
                <asp:Button ID="BtnDelete" Text="Delete" OnClick="BtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this subscription?\r\n\r\nWARNING: This will ERASE all of the data and users associated with this subscription!');" runat="server" />
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <asp:Label ID="LblStatus" runat="server" />
</asp:Content>
