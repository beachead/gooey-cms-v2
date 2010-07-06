<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.Auth.Themes.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li class="on">Manage Themes:</li>
            <li class=""><a href="~/auth/Themes/Add.aspx" runat="server">Add Theme</a></li>
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
    Modify which themes are available on your site, upload new themes or edit existing themes. 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <div style="padding-top:15px;">
        <asp:Label ID="StatusLabel" Visible="false" runat="server" />
        <asp:GridView ID="GridView1" runat="server" Width="90%" AutoGenerateColumns="False" 
            CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="ThemesDataSource">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Id">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:HiddenField ID="HiddenId" Value='<%# Eval("Theme.ThemeGuid") %>' runat="server" />
                        <asp:Label ID="Id" Text='<%# Eval("Theme.Id") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                     <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Name" Text='<%# Eval("Theme.Name") %>' runat="server" />
                    </ItemTemplate>               
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Description" Text='<%# Eval("Theme.Description") %>' runat="server" />
                    </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:RadioButton ID="Enabled" Checked='<%# Eval("Theme.IsEnabled") %>' AutoPostBack="true" OnCheckedChanged="OnEnableClick_Change" runat="server" />
                    </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:HyperLink ID="EditTemplates" Text="Templates" runat="server" 
                            NavigateUrl='<%# Eval("Theme.ThemeGuid","Templates.aspx?tid={0}") %>' />&nbsp;
                        <asp:HyperLink ID="EditHeaderFooter" Text="Header/Footer" runat="server"
                            NavigateUrl='' />&nbsp;
                        <asp:HyperLink ID="EditCss" Text="CSS" runat="server"
                            NavigateUrl='' />&nbsp;
                        <asp:HyperLink ID="EditJavascript" Text="Javascript" runat="server"
                            NavigateUrl='' />&nbsp;
                        <asp:LinkButton ID="DeleteTemplate" Text="[delete]" OnClientClick="return confirm('Are you sure you want to delete this theme?\r\n\r\nWARNING: This will also delete ALL javascript, css and image files associated with this theme.')" OnClick="OnDelete_Click" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>    
        <asp:ObjectDataSource ID="ThemesDataSource" runat="server" 
            SelectMethod="GetThemesBySite" 
            TypeName="Gooeycms.Webrole.Control.App_Code.Adapters.ThemeAdapter" 
            onselecting="ThemesDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="guid" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>  
        <asp:Button ID="BtnSaveThemes" OnClick="OnSaveThemes_Click" Text="Save" runat="server" />              
    </div>
</asp:Content>
