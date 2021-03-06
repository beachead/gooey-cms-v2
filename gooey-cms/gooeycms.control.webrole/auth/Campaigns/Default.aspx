﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Default" %>
<%@ MasterType VirtualPath="~/Secure.master" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="campaigns" NavItem="listing" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>Create Campaign</h1>
    <p>This page allows you to manage and generate links for campaigns that are currently running on the site.</p>

    <asp:Panel ID="PanelEnablePhoneIntegration" runat="server">
    <b>Enable phone integration for your campaigns!</b><br />
    To enable phone integration, simply go to the <a href="Settings.aspx?pid=phone-panel">settings</a> page.
    </asp:Panel>

    <asp:Label ID="Status" ForeColor="Green" runat="server" /><br />
    <a href="./create.aspx">Create New Campaign</a><br /><br />
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
            <asp:BoundField DataField="StartDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Start Date" 
                SortExpression="StartDate" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="EndDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="End Date" 
                SortExpression="EndDate" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" 
                SortExpression="PhoneNumber" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:HiddenField ID="CampaignId" Value='<%# Eval("Guid") %>' runat="server" />
                    <a href="./Create.aspx?id=<%# Eval("Guid") %>">Edit</a>&nbsp;
                    <!--<a href="./Elements.aspx?id=<%# Eval("Guid") %>">Elements</a>&nbsp;  -->
                    <a href="#" onclick="window.radopen('./Links.aspx?id=<%# Eval("Guid") %>');">Build Links</a>&nbsp;
                    <a href="#" onclick="open_window('./Elements.aspx?id=<%# Eval("Guid") %>',570,900); return false;">Elements</a>&nbsp;
                    <a href="#" onclick="window.radopen('./ActivatePhone.aspx?id=<%# Eval("Guid") %>'); return false;">Activate Phone</a>&nbsp;
                    <asp:LinkButton ID="DeleteItem" CommandName="deleteid" OnClientClick="return confirm('Are you sure you want to delete this campaign?\r\n\r\nNOTE:This will also release any phone number associated with this campaign.');" Text="Delete" runat="server" />
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
        <EmptyDataRowStyle CssClass="margin-right:200px;"/>
        <EmptyDataTemplate>
            There are currently no campaigns created.
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:ObjectDataSource ID="CampaignDataSource" runat="server" 
        SelectMethod="GetCampaigns" 
        TypeName="Gooeycms.Business.Campaigns.CampaignManager">
    </asp:ObjectDataSource>   

    <telerik:RadWindowManager ID="Singleton" Skin="Default" Modal="true" Height="350" Width="700" ShowContentDuringLoad="false"  VisibleStatusbar="false" Behaviors="Close,Move,Resize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <script language="javascript" type="text/javascript">
        function open_window(path, height, width) {
            var wnd = window.radopen(path);
            wnd.set_height(height);
            wnd.set_width(width);
            wnd.moveTo(150, 10);
        }
    </script>
</asp:Content>
