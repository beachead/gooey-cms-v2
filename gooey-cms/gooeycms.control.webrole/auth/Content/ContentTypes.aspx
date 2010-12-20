<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="ContentTypes.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Content.ContentTypes" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="content" NavItem="contenttypes" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <h1>Manage Content Types</h1>
    
<div dojoType="dijit.TitlePane" title="Existing Custom Content Types">
    <asp:GridView ID="ExistingContentTypes" runat="server" AutoGenerateColumns="False" 
            ForeColor="#333333" 
            DataSourceID="ContentTypeDataSource"
            GridLines="None" OnRowCommand="OnRowCommand" CssClass="data">
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
            <asp:BoundField DataField="DisplayName" HeaderText="Display Name" SortExpression="Name">
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="System Name" SortExpression="Name">
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
    
    <asp:LinkButton ID="LnkAddNewType" OnClick="LnkAddNewType_Click" Text="Add New Content Type" runat="server" />
    <table>
        <tr>
            <td>
                <asp:HiddenField ID="ExistingContentTypeGuid" runat="server" />
                <table class="form">
                    <tr>
                        <td class="label">Display Name:</td>
                        <td><asp:TextBox ID="ContentDispayName" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="label">System Name (no spaces allowed):</td>
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
                            <asp:Button ID="BtnAddContent" OnClick="AddContentType_Click" Text="Add" runat="server" />
                        </td>
                    </tr>
                </table>            
            </td>
            <td style="vertical-align:top;">or duplicate and customize a global type:</td>
            <td style="vertical-align:top;">
                <asp:DropDownList ID="LstGlobalTypes" runat="server" />&nbsp;
                <asp:LinkButton ID="BtnDuplicate" OnClick="BtnDuplicate_Click" Text="Duplicate" runat="server" />  
            </td>
        </tr>
    </table>
</div>
</asp:Content>
