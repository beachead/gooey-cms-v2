<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Debug.Logs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    Log Files:
    <br />
    <telerik:RadGrid ID="LogGrid" DataSourceID="SqlDataSource" runat="server" 
        GridLines="None" Skin="Default">
        <MasterTableView Width="100%" CommandItemDisplay="Top">
            <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />
        </MasterTableView>
    </telerik:RadGrid>
    
    <asp:SqlDataSource ID="SqlDataSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:gooeycmsConnectionString %>" 
        SelectCommand="SELECT * FROM [Logging] ORDER BY [inserted] DESC">
    </asp:SqlDataSource>
&nbsp;


</asp:Content>
