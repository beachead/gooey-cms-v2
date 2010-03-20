<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="ChooseTemplate.aspx.cs" Inherits="gooeycms.webrole.website.signup.ChooseTemplate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceholder" runat="server">
    <div id="progress">
        <em>1. CREATE ACCOUNT</em>&nbsp;&gt;&nbsp; <em>2. ACCOUNT INFO</em>&nbsp;&gt;&nbsp;
        <em>3. NAME CMS SITE</em>&nbsp;&gt;&nbsp; <strong>4. CHOOSE TEMPLATE</strong>
    </div>
    <div id="main">
        <table>
            <tr>
                <th class="title">
                    Default Template<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        Default Template
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>  
            <tr>
                <td colspan="3">
                    <asp:HiddenField ID="AccountGuid" runat="server" />
                    <asp:Button ID="BtnContinue" Text="PAYMENT" runat="server" 
                        onclick="BtnContinue_Click" />
                </td>
            </tr>                               
        </table>
    </div>
</asp:Content>
