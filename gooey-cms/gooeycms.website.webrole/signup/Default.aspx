<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="gooeycms.webrole.website.signup.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceholder" runat="server">
    <div id="progress">
        <strong>1. CREATE ACCOUNT</strong>&nbsp;&gt;&nbsp; 
        <em>2. ACCOUNT INFO</em>&nbsp;&gt;&nbsp;
        <em>3. NAME CMS SITE</em>&nbsp;&gt;&nbsp;
        <em>4. CHOOSE TEMPLATE</em>
    </div>
    <div id="main">
        <table>
            <tr>
                <th class="title">
                    Email address<br />
                    (must already exist)
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Email" runat="server" />
                    </div>
                </td>
                <td class="help">
                    You'll use this address to log in to manage your CMS site.
                </td>
            </tr>
            <tr>
                <th class="title">
                    Retype email address
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Email2" runat="server" />
                    </div>
                </td>
                <td class="help">
                    Retype your email address to ensure there are no typos.
                </td>
            </tr>
            <tr>
                <th class="title">
                    Enter a password
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Password" TextMode="Password" runat="server" />
                    </div>
                </td>
                <td class="help">
                    Must be at least 8 characters long.
                </td>
            </tr>
            <tr>
                <th class="title">
                    Retype password
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Password2" TextMode="Password" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>       
            <tr>
                <td colspan="3">
                    <asp:HiddenField ID="AccountGuid" runat="server" />
                    <asp:Button ID="BtnContinue" Text="CONTINUE" runat="server" 
                        onclick="BtnContinue_Click" />
                </td>
            </tr>  
        </table>
    </div>
</asp:Content>
