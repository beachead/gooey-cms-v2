﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Header.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.HeaderFooter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="Default.aspx">MANAGE THEMES</a></li>
            <li class="last"><a href="Add.aspx">ADD THEME</a></li>
        </ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <h1>Template Header</h1>
    <asp:Label ID="LblStatus" runat="server" />
    <br />
    <asp:Button ID="BtnSave2" Text="Save" OnClick="BtnSave_Click" runat="server" />
    <div dojoType="dijit.TitlePane" title="Header Template">
        <beachead:Editor ID="TxtHeader" ShowPreviewWindow="false" runat="server" />
    </div>
    <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
</asp:Content>
