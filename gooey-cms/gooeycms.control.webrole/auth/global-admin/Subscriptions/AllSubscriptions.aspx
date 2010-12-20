<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="AllSubscriptions.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Subscriptions.AllSubscriptions" %>

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
    <beachead:Subnav ID="Subnav" runat="server" NavSection="global_admin_subscriptions" NavItem="all" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager" runat="server" OnAjaxRequest="RadAjaxManager_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AllSubscriptionsTable" LoadingPanelID="LoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="AllSubscriptionsTable">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="AllSubscriptionsTable" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    All Subscriptions <br />

    <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
    <telerik:RadGrid ID="AllSubscriptionsTable" 
        DataSourceID="AllSubscriptionsDataSource" runat="server" 
                     AutoGenerateColumns="False" GridLines="None" 
                     OnItemDataBound="AllSubscriptionsTable_ItemDataBound"
                     OnItemCommand="AllSubscriptionsTable_ItemCommand">
        <MasterTableView Width="100%" CommandItemDisplay="TopAndBottom" EditMode="InPlace">
            <CommandItemSettings ShowAddNewRecordButton="false" ExportToPdfText="Export to Pdf"></CommandItemSettings>
            <Columns>
                <telerik:GridBoundColumn DataField="Guid" HeaderText="Subscription ID" SortExpression="Guid" UniqueName="Guid" />
                <telerik:GridBoundColumn DataField="SubscriptionPlanSku" HeaderText="Plan" SortExpression="SubscriptionPlanSku" UniqueName="SubscriptionPlanSku" />
                <telerik:GridBoundColumn DataField="PrimaryUser.Username" HeaderText="Username" SortExpression="PrimaryUser.Username" UniqueName="PrimaryUser.Username" />
                <telerik:GridBoundColumn DataField="Domain" HeaderText="Production Domain" SortExpression="Domain" UniqueName="Domain" />
                <telerik:GridBoundColumn DataField="StagingDomain" HeaderText="Staging Domain" SortExpression="StagingDomain" UniqueName="StagingDomain" />
                <telerik:GridBoundColumn DataField="Created" HeaderText="Created" SortExpression="Created" UniqueName="Created" />
                <telerik:GridBoundColumn DataField="PaypalProfileId" HeaderText="Paypal Customer ID" SortExpression="PaypalProfileId" UniqueName="PaypalProfileId" />
                <telerik:GridTemplateColumn ItemStyle-Width="250">
                    <ItemTemplate>
                        <asp:LinkButton ID="LnkEdit" Text="Modify Subscription" OnClientClick='<%# DataBinder.Eval(Container.DataItem, "Guid", "display_modify(\"{0}\"); return false;") %>' runat="server" />
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="LinkButton1" Text="Manage Users" OnClientClick='<%# DataBinder.Eval(Container.DataItem, "Guid", "display_users(\"{0}\"); return false;") %>' runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>    
    <asp:ObjectDataSource ID="AllSubscriptionsDataSource" runat="server" 
        SelectMethod="GetAllSubscriptions" 
        TypeName="Gooeycms.Business.Subscription.SubscriptionAdapter" >
    </asp:ObjectDataSource>
     
    <telerik:RadWindowManager ID="Singleton" Skin="Default" Modal="true" Width="975" Height="600" OnClientClose="window_close" ShowContentDuringLoad="false" Opacity="100" VisibleStatusbar="false" Behaviors="Move,Close,Resize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    
    <telerik:RadScriptBlock ID="RadScriptBlock" runat="server">
        <script type="text/javascript" language="javascript">
            function display_modify(guid) {
                window.radopen('./Modify.aspx?g=' + guid, null);
            }

            function display_users(guid) {
                window.radopen('./Users.aspx?g=' + guid, null);
            }

            function window_close(sender, eventArgs) {
                if (sender) {
                    var manager = $find("<%= RadAjaxManager.ClientID %>");
                    if (manager)
                        manager.ajaxRequest();
                }
            }
        </script> 
    </telerik:RadScriptBlock>
</asp:Content>
