<%@ Page Title="User Management" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Subscriptions.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .rgCommandRow
    {
      display :none;    
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="UserGridView">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="UserGridView" LoadingPanelID="RadAjaxLoadingPanel1" />
                <telerik:AjaxUpdatedControl ControlID="RolesUpdatePanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                <telerik:AjaxUpdatedControl ControlID="LblStatus" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="LnkAddUser">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="UserGridView" LoadingPanelID="RadAjaxLoadingPanel1" />                
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

<div style="padding-left:5px;padding-top:5px;">
    <b>Existing Users</b>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Skin="Default" runat="server" />

    <asp:Label ID="LblStatus" runat="server" />
    <telerik:RadGrid ID="UserGridView" Width="98%"
                        AllowAutomaticInserts="True" AllowAutomaticDeletes="True" AllowAutomaticUpdates="True" 
                        AllowPaging="True" AutoGenerateColumns="False"
                        DataSourceID="UserDataSource" runat="server" 
                        GridLines="None" 
                        OnInsertCommand="UserGridView_InsertItemCommand"
                        OnDeleteCommand="UserGridView_DeleteItemCommand">
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
    </telerik:RadGrid>
    <telerik:GridTextBoxColumnEditor ID="GridEmailEditor" TextBoxStyle-Width="200px" runat="server" />
    <telerik:GridTextBoxColumnEditor ID="GridTextEditor" TextBoxStyle-Width="150px" runat="server" />   
    <telerik:GridTextBoxColumnEditor ID="GridPasswordEditor" TextBoxMode="Password" runat="server" />
    <asp:ObjectDataSource ID="UserDataSource" runat="server" SelectMethod="GetUsers" 
        TypeName="Gooeycms.Business.Membership.MembershipDataSource" 
        DataObjectTypeName="Gooeycms.Data.Model.Subscription.UserInfo" 
        OldValuesParameterFormatString="original_{0}" UpdateMethod="UpdateUser">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="g" Type="String" Name="siteGuid" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <table>
        <tr>
            <td><asp:LinkButton ID="LnkAddUser" OnClick="LnkAddUser_Click" Text="Add New User" runat="server" /></td>
        </tr>
    </table>

    <telerik:RadWindowManager ID="Singleton" Skin="Default" Modal="true" Width="500" Height="525" ShowContentDuringLoad="false" DestroyOnClose="true" Opacity="100" VisibleStatusbar="false" Behaviors="Move,Close,Resize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    
    <script type="text/javascript" language="javascript">
        function display_roles(userguid) {
            window.radopen('./edit-roles.aspx?g=' + userguid, null);
        }
    </script> 
</div>
</asp:Content>
