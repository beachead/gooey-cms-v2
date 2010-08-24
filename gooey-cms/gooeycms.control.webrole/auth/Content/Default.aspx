<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Content.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li class="on">MANAGE CONTENT</li>
            <li><a href="">PROMOTION</a></li> 
            <li><a href="./ContentTypes.aspx">MANAGE CONTENT TYPES</a></li>       
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
    Content Types such as news and events can be edited and managed from this area.<br />
    To add new content types, click Manage Content Types above.<br /><br />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
   <a href="./Add.aspx">Add New Content</a>
   <br /><br />

   <b>Filter</b>: Content Type&nbsp;<asp:DropDownList ID="LstContentTypes" runat="server" />&nbsp;<asp:Button ID="BtnFilter" Text="Filter" runat="server" />
   <br />

   <div dojoType="dijit.TitlePane" title="Manage Existing Content">
        <asp:GridView ID="ContentTable" runat="server" Width="90%" AutoGenerateColumns="False" 
            CellPadding="4" DataSourceID="ExistingContent" ForeColor="#333333" 
            GridLines="None">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Title">
                    <ItemTemplate>
                        <asp:Label ID="Title" Text='<%# Eval("Title") %>' runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="LastSaved" HeaderText="Last Saved" 
                    SortExpression="LastSaved" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:CheckBoxField DataField="IsApproved" HeaderText="Approved" ReadOnly="True" 
                    SortExpression="IsApproved" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:CheckBoxField>
                <asp:CheckBoxField DataField="RequiresRegistration" 
                    HeaderText="Registration Required?" ReadOnly="True" 
                    SortExpression="RequiresRegistration" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:CheckBoxField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:HiddenField ID="ContentId" Value='<%# Eval("Guid") %>' runat="server" />
                        <asp:HyperLink ID="EditLink" NavigateUrl='<%# Eval("Guid","./Edit.aspx?tid={0}") %>' Text="edit" runat="server" />
                        <asp:LinkButton ID="DeleteItem" CommandName="deleteid" Text="delete" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>                
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ExistingContent" runat="server" 
            SelectMethod="GetExistingContent" 
            TypeName="Gooeycms.Business.Content.ContentManagerDataAdapter">
            <SelectParameters>
                <asp:ControlParameter ControlID="LstContentTypes" Name="contentTypeFilter" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
   </div>
</asp:Content>
