<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="./Create.aspx">NEW CAMPAIGN</a></li>    
            <li><a href="./Create.aspx">LEAD REPORT</a></li>    
            <li class="last"><a href="./Settings.aspx">CAMPAIGN SETTINGS</a></li>  
        </ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>Create Campaign</h1>
    <p>This page allows you to manage and generate links for campaigns that are currently running on the site.</p>

    <a href="./create.aspx">Manage Campaigns</a><br /><br />
    <asp:GridView ID="CampaignTable" runat="server" 
        DataSourceID="CampaignDataSource"
        AutoGenerateColumns="False" CssClass="data"
        ForeColor="#333333" GridLines="None"
        OnRowCommand="OnRowCommand">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="TrackingCode" HeaderText="Tracking Code" 
                SortExpression="TrackingCode" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" 
                SortExpression="StartDate" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="EndDate" HeaderText="End Date" 
                SortExpression="EndDate" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HiddenField ID="CampaignId" Value='<%# Eval("Guid") %>' runat="server" />
                    <a href="./Create.aspx?id=<%# Eval("Guid") %>">Edit</a>&nbsp;
                    <a href="./Elements.aspx?id=<%# Eval("Guid") %>">Elements</a>&nbsp;                    
                    <a href="#" onclick="window.open('./Links.aspx?id=<%# Eval("Guid") %>','','top=15,left=10,width=600,height=150'); return false;">Build Links</a>&nbsp;
                    <asp:LinkButton ID="DeleteItem" CommandName="deleteid" OnClientClick="return confirm('Are you sure you want to delete this campaign?');" Text="Delete" runat="server" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EmptyDataTemplate>
            There are currently no campaigns created.
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:ObjectDataSource ID="CampaignDataSource" runat="server" 
        SelectMethod="GetCampaigns" 
        TypeName="Gooeycms.Business.Campaigns.CampaignManager">
    </asp:ObjectDataSource>   
</asp:Content>
