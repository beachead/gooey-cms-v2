<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
This page will allow you to package your site or theme for sale in the Gooey Store.
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
<a href="./site.aspx">Package Site</a>&nbsp;<a href="./theme.aspx">Package Theme</a>

<div>
    Themes or sites awaiting approval to be listed:<br />
    <asp:DropDownList ID="DropDownList2" runat="server" />
</div>

<div>
    Sites currently listed in the Gooey Store: <br />
    <asp:DropDownList ID="ExistingSites" runat="server" />
</div>

<div>
    Themes currently listed in the Gooey Store: <br />
    <asp:DropDownList ID="DropDownList1" runat="server" />
</div>
</asp:Content>
