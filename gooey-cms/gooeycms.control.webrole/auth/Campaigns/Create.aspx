<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Create" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="./Default.aspx">List Campaigns</a></li>              
            <li class="on">Modify Campaign</li>      
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
    Specify the basic campaign information below. 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
<asp:Label ID="Status" ForeColor="Green" runat="server" /><br />
<table style="width:80%;">
    <tr>
        <td>Campaign Name:</td>
        <td><asp:TextBox ID="Name" dojoType="dijit.form.ValidationTextBox" required="true" promptMessage="Input the display name for this campaign." regExp="[a-zA-Z_0-9\s]+" invalidMessage="The campaign name may not contain any special characters" runat="server" /></td>
        <td>Start Date:</td>
        <td><bdp:BDPLite ID="StartDate" runat="server" /></td>
    </tr>
    <tr>
        <td>Tracking Code:</td>
        <td><asp:TextBox ID="Tracking" dojoType="dijit.form.ValidationTextBox" required="true" promptMessage="Input the tracking code you would like to associate with this campaign." regExp="[a-zA-Z_]+" invalidMessage="The tracking code may not contain any spaces or numbers." runat="server" /></td>
        <td>End Date:</td>
        <td><bdp:BDPLite ID="EndDate" runat="server" /></td>        
    </tr>
    <tr>
        <td colspan="4"><br /><asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" /></td>
    </tr>
</table>
</asp:Content>
