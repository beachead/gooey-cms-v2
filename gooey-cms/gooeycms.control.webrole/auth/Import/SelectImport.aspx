<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="SelectImport.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Import.SelectImport" %>
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
    <telerik:RadFormDecorator ID="FormDecorator" DecoratedControls="Fieldset" runat="server" />

    <asp:Button ID="BtnImport" Text="Import" OnClientClick="return confirm('Are you sure you want to import these files?\r\nWARNING: Files or pages with the same names will automatically be overwritten.');"  OnClick="BtnImport_Click" runat="server" />

    <fieldset>
        <legend>Import the following pages</legend>
        <div style="height:300px; overflow:auto;">
        <asp:CheckBoxList ID="ImportPages" runat="server" />
        </div>
    </fieldset>

    <fieldset>
        <legend>Import the following css/javascript</legend>
        <div style="height:300px; overflow:auto;">
        <asp:CheckBoxList ID="ImportCssJs" runat="server" />        
        </div>
    </fieldset>

    <fieldset>
        <legend>Import the following images</legend>
        <div style="height:300px; overflow:auto;">
        <asp:CheckBoxList ID="ImportImages" runat="server" />
        </div>
    </fieldset>

    <fieldset>
        <legend>The following files could not automatically be imported</legend>
        <asp:ListBox ID="LstUnknowns" SelectionMode="Multiple" runat="server" />
    </fieldset>
</asp:Content>
