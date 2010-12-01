<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="AllSubscriptions.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Subscriptions.AllSubscriptions" %>
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager"  runat="server"></telerik:RadScriptManager>
    All Subscriptions <br />
    <telerik:RadGrid ID="AllSubscriptionsTable" 
        DataSourceID="AllSubscriptionsDataSource" runat="server" 
                     AutoGenerateColumns="False" GridLines="None" 
                     OnItemDataBound="AllSubscriptionsTable_ItemDataBound">
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
                <telerik:GridTemplateColumn ItemStyle-Width="125">
                    <ItemTemplate>
                        <asp:LinkButton ID="LnkEdit" Text="Modify Subscription" OnClientClick='<%# DataBinder.Eval(Container.DataItem, "Guid", "display_modify(\"{0}\"); return false;") %>' runat="server" />
                        <asp:LinkButton ID="LnkToggleSubscription" Text="" CommandName="EnableDisable" runat="server" />&nbsp;&nbsp;
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this subscription?\r\nWARNING:This will delete ALL of the data associated with this subscription." ConfirmDialogType="Classic"
                    ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete"
                    UniqueName="DeleteUser">
                    <ItemStyle HorizontalAlign="Center" />
                </telerik:GridButtonColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>    
    <asp:ObjectDataSource ID="AllSubscriptionsDataSource" runat="server" 
        SelectMethod="GetAllSubscriptions" 
        TypeName="Gooeycms.Business.Subscription.SubscriptionAdapter" >
    </asp:ObjectDataSource>

    <telerik:RadWindowManager ID="Singleton" Skin="Default" Modal="true" Width="800" Height="370" ShowContentDuringLoad="false" DestroyOnClose="true" Opacity="100" VisibleStatusbar="false" Behaviors="Move,Close,Resize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    
    <script type="text/javascript" language="javascript">
        function display_modify(guid) {
            window.radopen('./Modify.aspx?g=' + guid, null);
        }
    </script> 
</asp:Content>
