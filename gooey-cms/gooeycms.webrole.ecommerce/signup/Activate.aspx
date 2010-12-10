<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Activate.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.signup.Activate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div id="content">
    <asp:MultiView ID="ActivateViews" runat="server">
        <asp:View ID="SuccessPaypalView" runat="server">
                Congratulations, Your subscription has been successfully activated!
                <br /><br />

                For your reference your paypal profile id is: <asp:Label ID="PaypalProfileId" runat="server" />
                <br /><br />

                To start managing your site, <a href="http://control.gooeycms.net">login</a> to your management console now.        
        </asp:View>
        <asp:View ID="SuccessFreeView" runat="server">
                Congratulations, Your subscription has been successfully activated!
                <br /><br />

                To start managing your site, <a href="http://control.gooeycms.net">login</a> to your management console now.        
        </asp:View>
        <asp:View ID="FailureView" runat="server">
                There was a problem activating your subscription.
                <br />
                Reason: <asp:Label ID="LblErrorReason" runat="server" />
                <br /><br />
                Please contact customer support for further help.
        </asp:View>
    </asp:MultiView>
</div>
</asp:Content>
