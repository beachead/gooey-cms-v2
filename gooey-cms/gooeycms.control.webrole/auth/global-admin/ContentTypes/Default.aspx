<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.ContentTypes.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <beachead:Subnav ID="Subnav" runat="server" NavSection="global_admin_contenttypes" NavItem="default" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    This page will allow you to manage the global content types available to all users.
    <br /><br />

    Existing Content Types:
    <asp:GridView ID="ExistingContentTypes" runat="server" AutoGenerateColumns="False" 
            ForeColor="#333333" 
            DataSourceID="ContentTypeDataSource"
            GridLines="None" OnRowCommand="OnRowCommand" CssClass="data">
        <EmptyDataTemplate>
            There are no global content types defined. You can create a new 
            custom content type below.
        </EmptyDataTemplate>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="DisplayName" HeaderText="Display Name" SortExpression="Name">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:CheckBoxField DataField="IsFileType" HeaderText="File" 
                SortExpression="IsFile">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HiddenField ID="ContentTypeId" Value='<%# Eval("Guid") %>' runat="server" />
                    <asp:LinkButton ID="LnkEditType" CommandName="editid" Text="Edit" runat="server" />&nbsp;&nbsp;
                    <asp:HyperLink ID="LinkFields" NavigateUrl='<%# Eval("Guid","./ContentTypeFields.aspx?tid={0}") %>' Text="Manage Fields" runat="server" />&nbsp;&nbsp;&nbsp;
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <asp:ObjectDataSource ID="ContentTypeDataSource" runat="server" 
        SelectMethod="GetGlobalContentTypes" 
        TypeName="Gooeycms.Business.Content.ContentManagerDataAdapter"></asp:ObjectDataSource>

    <asp:Label ID="Status" runat="server" /><br />
    <asp:LinkButton ID="LnkAddNewType" OnClick="LnkAddNewType_Click" Text="Add New Content Type" runat="server" />

    <asp:HiddenField ID="GlobalTypeToken" runat="server" />
    <asp:HiddenField ID="ExistingContentTypeGuid" runat="server" />
    <table class="form">
        <tr>
            <td class="label">Display Name:</td>
            <td><asp:TextBox ID="ContentDispayName" runat="server" /></td>
        </tr>
        <tr>
            <td class="label">Name:</td>
            <td><asp:TextBox ID="ContentSystemName" runat="server" /></td>
        </tr>
        <tr>
            <td class="label">Description:</td>
            <td><asp:TextBox ID="ContentDescription" runat="server" /></td>
        </tr>
        <tr>
            <td class="label">Supports File Uploads?</td>
            <td>
                <asp:RadioButton ID="ContentFileYes" GroupName="Files" Text="Yes" runat="server" />
                <asp:RadioButton ID="ContentFileNo" GroupName="Files" Checked="true" Text="No" runat="server" />                    
            </td>
        </tr> 
        <tr>
            <td class="label">Show Content Editor?</td>
            <td>
                <asp:RadioButton ID="ContentEditorYes" GroupName="Editor" Checked="true" Text="Yes" runat="server" />
                <asp:RadioButton ID="ContentEditorNo" GroupName="Editor" Text="No" runat="server" />                    
            </td>
        </tr>             
        <tr class="controls">
            <td colspan="2">
                <asp:Button ID="BtnAddContentType" OnClick="AddContentType_Click" Text="Add" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
