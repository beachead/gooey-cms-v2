<%@ Page Title="" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Email_Templates.Template" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding:10px;">
    <div>
        Editing <b><asp:Label ID="LblTemplateName" runat="server" /></b> template:
    </div>
    <br />

    <table style="width:99%;">
        <tr>
            <td>
                <div overflow:auto;">

                </div>            
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">
                <asp:LinkButton ID="BtnSaveTemplate" Text="Save" runat="server" />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
