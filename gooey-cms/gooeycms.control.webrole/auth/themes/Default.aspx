<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.Auth.Themes.Default" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="themes" navItem="theme" />
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <h1>Manage Themes</h1>
    <p>Modify which themes are available on your site, <a href="http://corp.gooeycms.net/store">purchase new themes</a> or edit existing themes.</p>
    
        <asp:Label ID="StatusLabel" Visible="false" runat="server" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                      CssClass="data" ForeColor="#333333" GridLines="None"
                      OnRowCommand="OnRowCommand_Click"
                      OnRowDataBound="OnRowDataBound"
                      DataSourceID="ThemesDataSource">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
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
                <asp:TemplateField HeaderText="Active">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:RadioButton ID="Enabled" Checked='<%# Eval("Theme.IsEnabled") %>' AutoPostBack="true" OnCheckedChanged="OnEnableClick_Change" runat="server" />
                    </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:HyperLink ID="EditTemplates" Text="Templates" runat="server" 
                            NavigateUrl='<%# Eval("Theme.ThemeGuid","Templates.aspx?tid={0}") %>' />&nbsp;|&nbsp;
                        <asp:HyperLink ID="EditHeaderFooter" Text="Header" runat="server"
                            NavigateUrl='<%# Eval("Theme.ThemeGuid","Header.aspx?tid={0}") %>' />&nbsp;
                        <asp:HyperLink ID="HyperLink1" Text="Footer" runat="server"
                            NavigateUrl='<%# Eval("Theme.ThemeGuid","Footer.aspx?tid={0}") %>' />&nbsp;|&nbsp;
                        <asp:HyperLink ID="EditCss" Text="CSS" runat="server"
                            NavigateUrl='<%# Eval("Theme.ThemeGuid","Stylesheet.aspx?tid={0}") %>'  />&nbsp;
                        <% if (Gooeycms.Business.Util.CurrentSite.Restrictions.IsJavascriptAllowed) { %>
                        <asp:HyperLink ID="EditJavascript" Text="Javascript" runat="server"
                            NavigateUrl='<%# Eval("Theme.ThemeGuid","Javascript.aspx?tid={0}") %>' />
                        <% } else { %>
                        <asp:HyperLink ID="UpgradeJavascript" Text="Javascript (requires upgrade)" runat="server"
                            NavigateUrl='http://store.gooeycms.net/signup/upgrade.aspx' />
                        <% } %>
                        &nbsp;|&nbsp;
                        <a href="#" onclick="window.open('<%# Eval("Theme.ThemeGuid","ImageBrowser.aspx?tid={0}") %>','','width=600,height=500,left=150');">Images</a>&nbsp;|&nbsp;
                        <asp:LinkButton ID="DeleteTheme" Text="[delete]" CommandArgument='<%# Eval("Theme.ThemeGuid") %>' CommandName='deletetheme' OnClientClick="return confirm('Are you sure you want to delete this theme?\r\n\r\nWARNING: This will also delete ALL javascript, css and image files associated with this theme.')" runat="server" />
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
            TypeName="Gooeycms.Business.Adapters.ThemeAdapter" 
            onselecting="ThemesDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="guid" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>  
   
    <div class="controls">
        <asp:Button ID="BtnSaveThemes" OnClick="OnSaveThemes_Click" Text="Save" runat="server" />              
   </div>
</asp:Content>
