<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Subscriptions.Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
    <style type="text/css">
    .rgCommandRow
    {
      display :none;    
    }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <beachead:Subnav ID="Subnav" runat="server" NavSection="global_admin_subscriptions" NavItem="pending" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager"  runat="server"></telerik:RadScriptManager>

    <div>
        Upcoming Renewals (next <asp:Label ID="LblTimeframe" Text="14" runat="server" /> days) <br />
        <telerik:RadGrid ID="PendingSubscriptions" DataSourceID="PendingSubscriptionsDataSource" runat="server"  AutoGenerateColumns="False" GridLines="None">
            <MasterTableView Width="100%" CommandItemDisplay="TopAndBottom" EditMode="InPlace">
                <CommandItemSettings ShowAddNewRecordButton="false" ExportToPdfText="Export to Pdf"></CommandItemSettings>
                <Columns>
                    <telerik:GridBoundColumn DataField="Guid" HeaderText="Subscription ID" SortExpression="Guid" UniqueName="Guid" />
                    <telerik:GridBoundColumn DataField="Domain" HeaderText="Custom Domain" SortExpression="Domain" UniqueName="Domain" />
                    <telerik:GridBoundColumn DataField="Created" HeaderText="Created" SortExpression="Created" UniqueName="Created" />
                    <telerik:GridBoundColumn DataField="PaypalProfileId" HeaderText="Paypal Customer ID" SortExpression="PaypalProfileId" UniqueName="PaypalProfileId" />
                </Columns>
            </MasterTableView>

        </telerik:RadGrid>    
        <asp:ObjectDataSource ID="PendingSubscriptionsDataSource" runat="server" 
            SelectMethod="GetUpcomingRenewals" 
            TypeName="Gooeycms.Business.Subscription.SubscriptionAdapter" >
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="14" Name="timeframeInDays" 
                    QueryStringField="timeframe" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
