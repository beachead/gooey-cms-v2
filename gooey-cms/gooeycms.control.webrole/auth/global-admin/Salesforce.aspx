<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="Salesforce.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Salesforce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <beachead:Subnav ID="Subnav" runat="server" NavSection="GlobalAdmin" NavItem="salesforce" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <telerik:RadFormDecorator ID="FormDecorator" DecoratedControls="Label,Fieldset,Textbox" runat="server" />

    <fieldset style="width:500px;">
        <legend>Salesforce Configuration</legend>

        <label>Username</label><br />
        <asp:TextBox ID="TxtUsername" runat="server" />
        <br />

        <label>Password</label><br />
        <asp:TextBox ID="TxtPassword" TextMode="Password" runat="server" />
        <br />

        <label>API Token</label><br />
        <asp:TextBox ID="TxtToken" Width="350px" runat="server" />
        <br />

        <asp:Button ID="BtnUpdate" Text="Update" OnClick="BtnUpdate_Click" runat="server" />
    </fieldset>
</asp:Content>
