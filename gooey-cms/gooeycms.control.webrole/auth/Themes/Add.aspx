<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Gooeycms.Webrole.Control.Auth.Themes.Add" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li class=""><a href="./">Manage Themes</a></li>
            <li class="on"><%=PageAction %> Theme</li>
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
This page allows you to add a new theme to the site or modify an existing theme.
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <asp:Label ID="Callout" Text="Add New Theme" runat="server" />
    <br /><br />
    
    <beachead:StatusPanel ID="Status" ShowStatus="false" runat="server" />

    <b class="small">1. General Info</b>
    <table>
        <tr>
            <td>Theme Name:</td>    
            <td><asp:TextBox ID="ThemeName" Width="250px" runat="server" /></td>
        </tr>   
        <tr>
            <td>Theme Description (one-line):</td>    
            <td><asp:TextBox ID="ThemeDescription" Width="250px" runat="server" /></td>
        </tr>         
        <tr>
            <td colspan="2"><asp:Button ID="Save" Text="Save" OnClick="SaveTheme_Click" runat="server" /></td>
        </tr>        
    </table>
</asp:Content>
