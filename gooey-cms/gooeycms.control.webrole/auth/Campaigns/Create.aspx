<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Create" %>
<%@ MasterType VirtualPath="~/Secure.master" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="campaigns" NavItem="new" />
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
<h1>Create Campaign</h1>

<p>Specify the basic campaign information below. </p>
<asp:Label ID="Status" ForeColor="Green" runat="server" /><br />
<table class="form">
    <tr>
        <td class="label">Campaign Name:</td>
        <td><asp:TextBox ID="Name" dojoType="dijit.form.ValidationTextBox" required="true" promptMessage="Input the display name for this campaign." regExp="[a-zA-Z_0-9\s]+" invalidMessage="The campaign name may not contain any special characters" runat="server" /></td>
        <td class="label">Start Date:</td>
        <td><bdp:BDPLite ID="StartDate" runat="server" /></td>
    </tr>
    <tr>
        <td class="label">Tracking Code:</td>
        <td><asp:TextBox ID="Tracking" dojoType="dijit.form.ValidationTextBox" required="true" promptMessage="Input the tracking code you would like to associate with this campaign." regExp="[a-zA-Z_0-9]+" invalidMessage="The tracking code may not contain any spaces or special characters." runat="server" /></td>
        <td class="label">End Date:</td>
        <td><bdp:BDPLite ID="EndDate" runat="server" /></td>        
    </tr>
    <tr class="controls">
        <td colspan="4"><br /><asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" /></td>
    </tr>
</table>
</asp:Content>
