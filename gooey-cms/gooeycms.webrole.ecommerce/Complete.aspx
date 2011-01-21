<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Complete.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.store.Complete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="content">
        <div style="padding-left:25px;">
        <span style="font-size:130%;font-weight:bold;">Thank you for your purchase!</span><br /><br />
        The <asp:Label ID="LblPurchaseType" runat="server" /> you have purchased has been added to your account and is immediately available. <br /><br />

        A receipt for this purchase has also been emailed to you. 
        <br />
        Login to your <a href="http://control.gooeycms.com/auth/dashboard.aspx">Dashboard</a> now to apply this <%= LblPurchaseType.Text %> to your subscription.
        </div>
    </div>
</asp:Content>
