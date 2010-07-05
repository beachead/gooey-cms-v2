<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <asp:DropDownList ID="AvailableSites" runat="server" />&nbsp;
    <asp:Button ID="BtnManageSite" Text="Manage Site" OnClick="BtnManageSite_Click" runat="server" />
</asp:Content>
