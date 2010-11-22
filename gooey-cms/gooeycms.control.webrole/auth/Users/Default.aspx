<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Users.Default" %>
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
    <beachead:Subnav ID="Subnav" runat="server" navSection="users" navItem="default" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
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
        <telerik:AjaxSetting AjaxControlID="UserGridView">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="UserGridView" LoadingPanelID="RadAjaxLoadingPanel1" />
                <telerik:AjaxUpdatedControl ControlID="RolesUpdatePanel" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="LnkAddUser">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="UserGridView" LoadingPanelID="RadAjaxLoadingPanel1" />                
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    Select a user to edit or add a new user. Make sure to apply the appropriate roles to each user.
    <br /><br />

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Skin="Default" runat="server" />
    <table style="width:100%;">
        <tr>
            <td style="width:65%;">
               
                <telerik:RadGrid ID="UserGridView" Width="90%"
                                 AllowAutomaticInserts="True" AllowAutomaticDeletes="True" AllowAutomaticUpdates="True" 
                                 AllowPaging="True" AutoGenerateColumns="False"
                                 DataSourceID="UserDataSource" runat="server" GridLines="None">
                    <MasterTableView Width="100%" CommandItemDisplay="TopAndBottom" DataKeyNames="Guid" AutoGenerateColumns="false" EditMode="InPlace">
                        <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                        <Columns>
                            <telerik:GridBoundColumn DataField="Email" HeaderText="Username" SortExpression="Email" UniqueName="Email" ColumnEditorID="GridEmailEditor" />
                            <telerik:GridBoundColumn DataField="Password" HeaderText="Password" UniqueName="Password" EmptyDataText="****" ItemStyle-Font-Italic="true" ColumnEditorID="GridPasswordEditor" />
                            <telerik:GridBoundColumn DataField="Firstname" HeaderText="Firstname" UniqueName="Firstname" ColumnEditorID="GridTextEditor" />
                            <telerik:GridBoundColumn DataField="Lastname" HeaderText="Lastname" UniqueName="Lastname" ColumnEditorID="GridTextEditor"  />
                            <telerik:GridTemplateColumn ItemStyle-Width="125">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkEditUser" Text="Edit" CommandName="Edit" runat="server" />&nbsp;&nbsp;
                                    <asp:LinkButton ID="LnkEditRoles" Text="Manage Roles" OnClientClick='<%# DataBinder.Eval(Container.DataItem, "Guid", "display_roles(\"{0}\"); return false;") %>' runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LnkSaveEdit" Text="Save" CommandName="Update" runat="server" />&nbsp;
                                    <asp:LinkButton ID="LnkCancelEdit" Text="Cancel" CommandName="Cancel" runat="server" />                                
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <asp:LinkButton ID="LnkSaveEdit" Text="Add User" CommandName="PerformInsert" runat="server" />&nbsp;&nbsp;
                                    <asp:LinkButton ID="LnkCancelEdit" Text="Cancel" CommandName="Cancel" runat="server" />                                
                                </InsertItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this user?" ConfirmDialogType="Classic"
                                ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete"
                                UniqueName="DeleteUser">
                                <ItemStyle HorizontalAlign="Center" />
                            </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <ClientEvents OnRowDblClick="RowDblClick" />
                    </ClientSettings>
                </telerik:RadGrid>
                <telerik:GridTextBoxColumnEditor ID="GridEmailEditor" TextBoxStyle-Width="200px" runat="server" />
                <telerik:GridTextBoxColumnEditor ID="GridTextEditor" TextBoxStyle-Width="150px" runat="server" />   
                <telerik:GridTextBoxColumnEditor ID="GridPasswordEditor" TextBoxMode="Password" runat="server" />         
            </td>
        </tr>    
    </table>
    <br />
     <asp:LinkButton ID="LnkAddUser" OnClick="LnkAddUser_Click" Text="Add New User" runat="server" />

    <telerik:RadWindowManager ID="Singleton" Skin="Default" Modal="true" Width="500" Height="570" ShowContentDuringLoad="false" DestroyOnClose="true" Opacity="100" VisibleStatusbar="false" Behaviors="Move,Close,Resize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    
    <script type="text/javascript" language="javascript">
        function display_roles(userguid) {
            window.radopen('./edit-roles.aspx?g=' + userguid,null);
        }
    </script>    
    <asp:ObjectDataSource ID="UserDataSource" runat="server" 
        DataObjectTypeName="Gooeycms.Data.Model.Subscription.UserInfo" 
        DeleteMethod="DeleteUser" InsertMethod="InsertUser" SelectMethod="GetUsers" 
        TypeName="Gooeycms.Business.Membership.MembershipDataSource" 
        UpdateMethod="UpdateUser">
        <SelectParameters>
            <asp:CookieParameter CookieName="selected-site" Name="encryptedSiteGuid" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
