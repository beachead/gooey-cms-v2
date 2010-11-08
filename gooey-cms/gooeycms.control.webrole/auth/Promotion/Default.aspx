<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Promotion.Default" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="promotion" NavItem="promotion" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">

    <script type="text/javascript">
        dojo.addOnLoad(function () { dijit.byId('mainTabContainer').selectChild('<% Response.Write(SelectedPanel); %>'); });
    </script>  
    <div id="mainTabContainer" dojoType="dijit.layout.TabContainer" style="height:500px;overflow:auto;">
        <div id="pagepanel" dojoType="dijit.layout.ContentPane" title="Promote Pages" style="display:none;">
            <asp:Label ID="LblPageStatus" runat="server" />
            <div id="page_promotion_container">
                <asp:GridView ID="PageListing" AutoUpdateAfterCallBack="True" 
                        DataSourceID="PageListDataSource" AutoGenerateColumns="False"
                        ForeColor="#333333" GridLines="None" runat="server" 
                        UpdateAfterCallBack="True" CssClass="data"
                        OnRowDataBound="OnItemDataBound">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="approve" runat="server"/>
                                <asp:HiddenField ID="pageId" runat="server" Value='<%# Eval("Page.Guid") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Pages">
                            <ItemTemplate>
                                <asp:Label ID="Path" Text='<%# Eval("Page.Url") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Author">
                            <ItemTemplate>
                                <a href="#" onclick="window.open('../security/edituser.aspx?user=<%# Eval("Page.Author") %>','','')"><%# Eval("Page.Author") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modified">
                            <ItemTemplate>
                                <asp:Label ID="LblDateModified" runat="server" Text='<%# Eval("Page.DateSaved") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>   
                    </Columns>
                    <EmptyDataTemplate>
                        No pages found matching search criteria.<br />
                        <a href="./Edit.aspx">Create New Page</a>
                    </EmptyDataTemplate>
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                </asp:GridView>
                <asp:ObjectDataSource ID="PageListDataSource" runat="server" 
                    SelectMethod="GetUnapprovedPages" 
                    TypeName="Gooeycms.Business.Adapters.PageAdapter">
                </asp:ObjectDataSource>
                
                <div style="text-align:right;padding-top:5px;">
                <asp:Button ID="Promote" Text="Promote" OnClick="PromotePages_Click" runat="server" />&nbsp;&nbsp;
                <asp:Button ID="Delete" Text="Delete" OnClick="DeletePages_Click" OnClientClick="return confirm('Are you sure you want to delete the selected page versions?')" runat="server" />
                </div>
            </div>
        </div>

        <div id="contentpanel" dojoType="dijit.layout.ContentPane" title="Promote Content" style="display:none;">
            <div id="content_promotion_container">
                <asp:Label ID="LblContentStatus" runat="server" />
                <asp:GridView ID="ContentListing" AutoUpdateAfterCallBack="True" 
                        DataSourceID="ContentListDataSource" AutoGenerateColumns="False"
                        ForeColor="#333333" GridLines="None" runat="server" 
                        UpdateAfterCallBack="True" CssClass="data"
                        OnRowDataBound="OnItemDataBound">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="approve" runat="server"/>
                                <asp:HiddenField ID="contentId" runat="server" Value='<%# Eval("Guid") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="Type" Text='<%# Eval("ContentType.Name") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>                         
                        <asp:TemplateField HeaderText="Title">
                            <ItemTemplate>
                                <asp:Label ID="Title" Text='<%# Eval("Title") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Author">
                            <ItemTemplate>
                                <a href="#" onclick="window.open('../security/edituser.aspx?user=<%# Eval("Author") %>','','')"><%# Eval("Author") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modified">
                            <ItemTemplate>
                                <asp:Label ID="LblDateModified" runat="server" Text='<%# Eval("LastSaved") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>   
                    </Columns>
                    <EmptyDataTemplate>
                        No pages found matching search criteria.<br />
                        <a href="./Edit.aspx">Create New Page</a>
                    </EmptyDataTemplate>
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                </asp:GridView>
                <asp:ObjectDataSource ID="ContentListDataSource" runat="server" 
                    SelectMethod="GetUnapprovedContent" 
                    TypeName="Gooeycms.Business.Content.ContentManagerDataAdapter">
                </asp:ObjectDataSource>
                
                <div style="text-align:right;padding-top:5px;">
                <asp:Button ID="Button1" Text="Promote" OnClick="PromoteContent_Click" runat="server" />&nbsp;&nbsp;
                </div>
            </div>
        </div>
    </div>

    <script language="javascript" type="text/javascript">
        var isMouseDown = false;
        var clicked = false;

        function onMouseEnter(obj) {
            if (isMouseDown)
                obj.checked = true;
        }
        function onInitialDown(obj) {
            if ((!isMouseDown) && (obj.checked == false)) {
                obj.checked = true;
                clicked = true;

            }
            else if (!isMouseDown) {
                obj.checked = false;
                clicked = true;
            }
        }
        function onMouseDown(event) {
            isMouseDown = true;
        }
        function onMouseUp(event) {
            isMouseDown = false;
        }
        function onMouseClick(obj) {
            if (clicked)
                obj.checked = !obj.checked;
        }

        $('page_promotion_container').addEvent('mousedown', onMouseDown);
        $('page_promotion_container').addEvent('mouseup', onMouseUp);
        $('content_promotion_container').addEvent('mousedown', onMouseDown);
        $('content_promotion_container').addEvent('mouseup', onMouseUp);
        </script>  
</asp:Content>
