<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="./Create.aspx">NEW CAMPAIGN</a></li>    
            <li><a href="./Create.aspx">LEAD REPORT</a></li>    
            <li class="last on">CAMPAIGN SETTINGS</li>  
        </ul>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
Configure your campaign settings below.
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
<table>
    <tr>
        <td style="vertical-align:top;padding-top:4px;">Enable Google Analytics</td>
        <td>
            <asp:RadioButton ID="RdoGoogleEnabledYes" GroupName="GoogleEnabled" Text="Yes" runat="server" />&nbsp; 
            <asp:RadioButton ID="RdoGoogleEnabledNo" GroupName="GoogleEnabled" Text="No" Checked="true" runat="server" />
        </td>
    </tr>
    <tr>
        <td>Google Analytics Account ID</td>
        <td>
            <asp:TextBox ID="TxtGoogleAccountId" Width="175px" dojoType="dijit.form.ValidationTextBox" promptMessage="Input the Google Web Profile ID or UA Number for this site. (e.g. UA-XXXXXXXX-Y)" regExp="UA-\d{4,10}-\d{1,2}" invalidMessage="format: UA-XXXXXXX-YY (where X's are your account number and Y is the profile number)" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align:right;">
            <asp:Button ID="BtnSaveGoogle" OnClick="BtnSaveGoogle_Click" Text="Save" runat="server" />
        </td>
    </tr>
</table>
</asp:Content>
