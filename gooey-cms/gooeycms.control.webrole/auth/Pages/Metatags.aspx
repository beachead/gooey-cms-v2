<%@ Page Title="" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="Metatags.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Metatags" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p><asp:Label ID="Status" runat="server" /></p>

    <table style="width:100%;">
            <tr>
                <td class="label" style="width:175px;">Page Title:</td>
                <td>
                    <asp:TextBox ID="PageTitle" TabIndex="2" Width="300px" runat="server" />
                    <asp:RequiredFieldValidator ID="PageTitleRequired" Text="You must enter a title for this page" ControlToValidate="PageTitle" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="label"">Page Description:</td>
                <td>
                    <asp:TextBox ID="PageDescription" Width="300px" TabIndex="3" TextMode="MultiLine" Rows="5" runat="server" /><br />
                    <asp:RequiredFieldValidator ID="PageDescriptionRequired" Enabled="false" Text="You must provide a description for this page" ControlToValidate="PageDescription" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="label">Page Keywords:</td>
                <td>
                    <asp:TextBox ID="PageKeywords" Width="300px" TabIndex="4" runat="server" /><br />
                    <asp:RequiredFieldValidator ID="PageKeywordsRequired" Enabled="false" Text="You must enter keywords for this page" ControlToValidate="PageKeywords" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="label">Body Load Options:</td>
                <td>
                    <asp:TextBox ID="BodyLoadOptions" Width="300px" TabIndex="4" runat="server" /><br />
                </td>
            </tr>    
            <tr>
                <td><asp:Button ID="BtnSave" OnClick="BtnSave_Click" Text="Save" runat="server" /></td>
            </tr>
    </table>
    </div>
</asp:Content>
