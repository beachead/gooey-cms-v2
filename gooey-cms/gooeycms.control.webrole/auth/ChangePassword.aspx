<%@ Page Title="" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.ChangePassword" %>
<%@ MasterType VirtualPath="~/PopupWindow.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="LblStatus" runat="server" />
    <br />

    <div>
        Current Password <br />
        <asp:TextBox ID="TxtCurrentPassword" TextMode="Password" runat="server" />
    </div>

    <div>
        New Password <br />
        <asp:TextBox ID="TxtPassword1" TextMode="Password" runat="server" />
        <asp:RequiredFieldValidator ID="Password1Required" ControlToValidate="TxtPassword1" ErrorMessage="*" runat="server" />
    </div>

    <div>
        Confirm Password <br />
        <asp:TextBox ID="TxtPassword2" TextMode="Password" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TxtPassword2" ErrorMessage="*" runat="server" />
        <asp:CompareValidator ID="CompareValidator1" ControlToValidate="TxtPassword2" ControlToCompare="TxtPassword1" runat="server" />
    </div>

    <div>
        <asp:Button ID="BtnUpdate" Text="Change Password" OnClick="BtnUpdate_Click" runat="server" />&nbsp;
    </div>
</asp:Content>
