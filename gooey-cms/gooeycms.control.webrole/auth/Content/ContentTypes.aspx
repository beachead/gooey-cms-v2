<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="ContentTypes.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Content.ContentTypes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="./Default.aspx">MANAGE CONTENT</a></li>
            <li><a href="">PROMOTION</a></li> 
            <li class="on">MANAGE CONTENT TYPES</li>       
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

<div dojoType="dijit.TitlePane" title="Existing Custom Content Types">
    <asp:GridView ID="ExistingContentTypes" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" ForeColor="#333333" 
            DataSourceID="ContentTypeDataSource"
            GridLines="None" OnRowCommand="OnRowCommand" Width="90%">
        <EmptyDataTemplate>
            There are no custom content types defined. You can create a new 
            custom content type below.
        </EmptyDataTemplate>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id">
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
                    <asp:HyperLink ID="LinkFields" NavigateUrl='<%# Eval("Guid","./ContentTypeFields.aspx?tid={0}") %>' Text="Manage Fields" runat="server" />
                    <asp:LinkButton ID="DeleteType" Text="Delete" CommandName="deleteid" OnClientClick="return confirm('Are you sure you want to delete this content type?');" runat="server" />                
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
        SelectMethod="GetContentTypes" 
        TypeName="Gooeycms.Business.Content.ContentManagerDataAdapter"></asp:ObjectDataSource>
</div>
<br />
<div dojoType="dijit.TitlePane" title="Define Custom Content Type">
    <asp:Label ID="Status" runat="server" />
    <asp:HiddenField ID="GlobalTypeToken" runat="server" />
    <table>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="ContentName" runat="server" /></td>
        </tr>
        <tr>
            <td>Description:</td>
            <td><asp:TextBox ID="ContentDescription" runat="server" /></td>
        </tr>
        <tr>
            <td>Supports File Uploads?</td>
            <td>
                <asp:RadioButton ID="ContentFileYes" GroupName="Files" Text="Yes" runat="server" />
                <asp:RadioButton ID="ContentFileNo" GroupName="Files" Checked="true" Text="No" runat="server" />                    
            </td>
        </tr> 
        <tr>
            <td>Show Content Editor?</td>
            <td>
                <asp:RadioButton ID="ContentEditorYes" GroupName="Editor" Checked="true" Text="Yes" runat="server" />
                <asp:RadioButton ID="ContentEditorNo" GroupName="Editor" Text="No" runat="server" />                    
            </td>
        </tr>             
        <tr>
            <td colspan="2">
                <asp:Button ID="AddContent" OnClick="AddContentType_Click" Text="Add" runat="server" />
            </td>
        </tr>
    </table>
</div>
</asp:Content>
