<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="CmsInfo.aspx.cs" Inherits="gooeycms.webrole.website.signup.CmsInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceholder" runat="server">
    <div id="progress">
        <em>1. CREATE ACCOUNT</em>&nbsp;&gt;&nbsp; <em>2. ACCOUNT INFO</em>&nbsp;&gt;&nbsp;
        <strong>3. NAME CMS SITE</strong>&nbsp;&gt;&nbsp; <em>4. CHOOSE TEMPLATE</em>
    </div>
    <div id="main">
        <table>
            <tr>
                <th class="title">
                    Site Name<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Sitename" runat="server" />
                    </div>
                </td>
                <td class="help">
                Please input your site name or short description.
                </td>
            </tr>
            <tr>
                <th class="title">
                    Domain<br />
                    (optional)
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Domain" runat="server" />
                    </div>
                </td>
                <td class="help">
                Input your domain name for this site (if you have one). This can be set later from
                your control panel. We will also assign you a subdomain on the gooeycms site which
                can be used to access your cms.
                </td>
            </tr>
            <tr>
                <th class="title">
                    Staging Domain<br />
                    (optional)
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Staging" runat="server" />
                    </div>
                </td>
                <td class="help">
                Input your staging domain name. This can be used to access your 
                staging server. We will also assign you a staging server subdomain 
                on the gooeycms site.
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
