<%@ Page Title="" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="Redirects.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Redirects" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .rgCommandRow
    {
      display :none;    
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager" OnAsyncPostBackError="RadScriptManager_OnAjaxError" runat="server"></telerik:RadScriptManager>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onEndAjaxRequest);
            function RowDblClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }

            function onEndAjaxRequest(sender, args) {
                if (args.get_error() != undefined && args.get_error().httpStatusCode == '500') {
                    var msg = args.get_error().message;
                    args.set_errorHandled(true);

                    alert('There was a problem processing this request: ' + msg);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="RedirectGridView">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RedirectGridView" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="LnkAddRedirect">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RedirectGridView" LoadingPanelID="RadAjaxLoadingPanel1" />                
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div style="padding:10px;">
        Manage Site Redirects:
        <br />

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Skin="Default" runat="server" />
        <telerik:RadGrid ID="RedirectGridView" Width="90%" AllowAutomaticInserts="True" AllowAutomaticDeletes="True"
            AllowAutomaticUpdates="True" AllowPaging="False" AutoGenerateColumns="False"
            DataSourceID="RedirectDataSource" runat="server" GridLines="None">
            <MasterTableView Width="100%" CommandItemDisplay="TopAndBottom" DataKeyNames="RedirectFrom" AutoGenerateColumns="false" EditMode="InPlace">
                <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                <Columns>
                    <telerik:GridBoundColumn DataField="RedirectFrom" HeaderText="Redirect From" SortExpression="RedirectFrom" UniqueName="RedirectFrom" ColumnEditorID="GridTextEditor" />
                    <telerik:GridBoundColumn DataField="RedirectTo" HeaderText="Redirect To" SortExpression="RedirectTo" UniqueName="RedirectTo" ColumnEditorID="GridTextEditor" />
                    <telerik:GridTemplateColumn ItemStyle-Width="125">
                        <ItemTemplate />
                        <InsertItemTemplate>
                            <asp:LinkButton ID="LnkSaveEdit" Text="Add Redirect" CommandName="PerformInsert" runat="server" />&nbsp;&nbsp;
                            <asp:LinkButton ID="LnkCancelEdit" Text="Cancel" CommandName="Cancel" runat="server" />                                
                        </InsertItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this redirect?" ConfirmDialogType="Classic"
                        ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteRedirect">
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <telerik:GridTextBoxColumnEditor ID="GridTextEditor" TextBoxStyle-Width="200px" runat="server" />

        <asp:LinkButton ID="LnkAddRedirect" OnClick="LnkAddRedirect_Click" Text="Add New Redirect" runat="server" />
        <asp:ObjectDataSource ID="RedirectDataSource" runat="server" 
            DeleteMethod="DeleteRedirect" InsertMethod="InsertRedirect" SelectMethod="GetRedirects" 
            TypeName="Gooeycms.Business.Site.RedirectDataSource" >
        </asp:ObjectDataSource>
    </div>
</asp:Content>
