<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Default" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li class="on">MANAGE PAGES</li>
            <li><a href="./Edit.aspx">ADD NEW PAGE</a></li>
            <li><a href="./Promotion.aspx">PROMOTION</a></li> 
            <li class="last"><a href="./Redirects.aspx">MANAGE REDIRECTS</a></li>       
        </ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <h1>Manage Pages</h1>
    <p>Pages on your site can be added, edited and managed from this area. Use the below filter to quickly find your existing pages.</p>

    <a href="./Edit.aspx">Add New Page</a><br /><br />
    Filter: <input id="filter_searchbox" type="text" value="*" onkeypress="return performFilter(this);" title="Use * for wildcard character"/>&nbsp;
    <a href="#" onclick="filter_clear(); return false;">clear</a>&nbsp;
    <a href="#" onclick="filter_search('*'); return false;">all</a>
 
    <asp:Label ID="Status" runat="server" /><br />
    <anthem:HiddenField ID="Filter" runat="server" />        
    <span id="refreshing"><br /></span>
    <anthem:GridView ID="PageListing" AutoUpdateAfterCallBack="True" 
            DataSourceID="PageListDataSource" AutoGenerateColumns="False" PageSize="25" 
            ForeColor="#333333" GridLines="None" runat="server" 
            UpdateAfterCallBack="True" CssClass="data">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField HeaderText="Pages">
                <ItemTemplate>
                    <asp:Label ID="Path" Text='<%# Eval("Page.Url") %>' runat="server" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <a href="./Edit.aspx?a=edit&pid=<%# Server.UrlEncode(Eval("Page.Url").ToString()) %>">Edit</a>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>        
        </Columns>
        <EmptyDataTemplate>
            No pages found matching search criteria.<br />
            <a href="./Edit.aspx">Create New Page</a>
        </EmptyDataTemplate>
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    </anthem:GridView>
    <asp:ObjectDataSource ID="PageListDataSource" runat="server" 
        SelectMethod="GetFilteredPages" 
        TypeName="Gooeycms.Business.Adapters.PageAdapter">
        <SelectParameters>
            <asp:ControlParameter ControlID="Filter" DefaultValue="*" Name="filter" 
                PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script language="javascript" type="text/javascript">
        function DoLoadData() {
            Anthem_InvokePageMethod('LoadPageData', [document.getElementById('<%=Filter.ClientID %>').value],
                function (result) {
                    var lbl = document.getElementById("refreshing");
                    lbl.innerHTML = "<br />";
                }
            );
        }

        function filter_clear() {
            document.getElementById('filter_searchbox').value = '';
        }

        function filter_search(val) {
            var obj = document.getElementById('filter_searchbox');
            obj.value = val;
            performFilter(obj);
        }

        function keypressHandler(obj) {
            if (window.mytimeout) window.clearTimeout(window.mytimeout);
            var lbl = document.getElementById("refreshing");
            lbl.innerHTML = "Refreshing...";
            document.getElementById('<%=Filter.ClientID %>').value = obj.value;
            DoLoadData();
        }

        function performFilter(obj) {
            if (window.mytimeout) window.clearTimeout(window.mytimeout);
            window.mytimeout = window.setTimeout(function () { keypressHandler(obj) }, 500);
            return true;
        }
    </script>
</asp:Content>
