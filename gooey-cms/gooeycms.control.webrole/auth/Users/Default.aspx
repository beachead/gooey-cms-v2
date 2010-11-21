<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Users.Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <beachead:Subnav ID="Subnav" runat="server" navSection="users" navItem="default" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager" runat="server" />

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RowDblClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
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
        </AjaxSettings>
    </telerik:RadAjaxManager>

    Select a user to edit or add a new user
    <br /><br />

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" Skin="Windows7" runat="server" />

    <table style="width:100%;">
        <tr>
            <td style="width:65%;">
                <telerik:RadGrid ID="UserGridView" Width="100%"
                                 AllowAutomaticInserts="True" AllowAutomaticDeletes="True" AllowAutomaticUpdates="True" 
                                 AllowPaging="True" AutoGenerateColumns="False"  Skin="Windows7"
                                 DataSourceID="UserDataSource" runat="server" GridLines="None">
                    <MasterTableView Width="100%" CommandItemDisplay="TopAndBottom" DataKeyNames="Guid" AutoGenerateColumns="false" EditMode="InPlace">
                        <Columns>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton"  UniqueName="EditColumnCommand">
                            </telerik:GridEditCommandColumn>
                            <telerik:GridBoundColumn DataField="Guid" HeaderText="ID" UniqueName="Guid" ReadOnly="true" ColumnEditorID="Readonly" />
                            <telerik:GridBoundColumn DataField="Email" HeaderText="Username/Email" UniqueName="Email" ColumnEditorID="GridEmailEditor" />
                            <telerik:GridBoundColumn DataField="Password" HeaderText="Password" UniqueName="Password" EmptyDataText="[stored]" ItemStyle-Font-Italic="true" ColumnEditorID="GridPasswordEditor" />
                            <telerik:GridBoundColumn DataField="Firstname" HeaderText="Firstname" UniqueName="Firstname" ColumnEditorID="GridTextEditor" />
                            <telerik:GridBoundColumn DataField="Lastname" HeaderText="Lastname" UniqueName="Lastname" ColumnEditorID="GridTextEditor"  />
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkEditRoles" Text="Edit Roles" OnClientClick='<%# DataBinder.Eval(Container.DataItem, "Guid", "display_roles(\"{0}\"); return false;") %>' runat="server" />
                                </ItemTemplate>
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

    <telerik:RadWindowManager ID="Singleton" Skin="Windows7" Modal="true" Width="463" Height="237" ShowContentDuringLoad="false" DestroyOnClose="true" Opacity="100" VisibleStatusbar="false" Behaviors="Move,Close,Resize" runat="server" EnableShadow="true">
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
