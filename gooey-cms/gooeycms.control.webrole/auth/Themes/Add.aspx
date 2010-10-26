<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Gooeycms.Webrole.Control.Auth.Themes.Add" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="themes" navItem="add" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <h1><asp:Label ID="Callout" Text="Add New Theme" runat="server" /></h1>
    <p>This page allows you to add a new theme to the site or modify an existing theme.</p>
    
    <beachead:StatusPanel ID="Status" ShowStatus="false" runat="server" />

    <h2>1. General Info</h2>
    
    <table class="form">
        <tr>
            <td>Theme Name:</td>    
            <td>
                <asp:TextBox ID="ThemeName" Width="250px" runat="server" />
                <asp:RegularExpressionValidator ID="NameValidator" ControlToValidate="ThemeName" ValidationExpression="[\w\d]+" ErrorMessage="* The theme name may only contain letters and/or numbers. No spaces or special characters." runat="server" />
            </td>
        </tr>   
        <tr>
            <td>Theme Description (one-line):</td>    
            <td><asp:TextBox ID="ThemeDescription" Width="250px" runat="server" /></td>
        </tr>         
        <tr class="controls">
            <td colspan="2"><asp:Button ID="Save" Text="Save" OnClick="SaveTheme_Click" runat="server" /></td>
        </tr>        
    </table>
</asp:Content>
