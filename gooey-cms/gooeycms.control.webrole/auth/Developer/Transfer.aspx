<%@ Page Title="Transfer Site" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Transfer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style="padding:10px;">
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
    <telerik:RadAjaxPanel ID="AjaxPanel" LoadingPanelID="LoadingPanel" runat="server">
        <asp:Label ID="LblStatus" runat="server" /><br />

        Input the email address of the user you'd like to transfer this site to: <br />
        <asp:TextBox ID="TxtEmailAddress" Width="300px" runat="server" />&nbsp;<asp:Button ID="BtnSearch" OnClick="BtnSearch_Click" Text="Search" runat="server" />

        <asp:Panel ID="ConfirmationPanel" Visible="false" runat="server">
            <br />
            <fieldset>
                <legend>Confirm Transfer</legend>
                Confirm you would like to copy this site to <asp:Label ID="LblEmailAddress" runat="server" />. <br /><br />
                This will place this site package in the user's dashboard area and allow them to apply it to their site.
                <br /><br />
                <asp:Button ID="BtnConfirm" OnClick="BtnConfirm_Click" Text="Transfer" runat="server" />&nbsp;
                <asp:Button ID="BtnCancel" Text="Cancel" OnClientClick="window.frameElement.radWindow.Close();" runat="server" />
            </fieldset>
        </asp:Panel>
    </telerik:RadAjaxPanel>
</div>
</asp:Content>
