<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="AccountInfo.aspx.cs" Inherits="gooeycms.webrole.website.signup.AccountInfo" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceholder" runat="server">
    <div id="progress">
        <em>1. CREATE ACCOUNT</em>&nbsp;&gt;&nbsp; 
        <Strong>2. ACCOUNT INFO</Strong>&nbsp;&gt;&nbsp;
        <em>3. NAME CMS SITE</em>&nbsp;&gt;&nbsp;
        <em>4. CHOOSE TEMPLATE</em>
    </div>
    <div id="main">
        <table>
            <tr>
                <th class="title">
                    First name<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Firstname" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>
            <tr>
                <th class="title">
                    Last name<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Lastname" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>     
            <tr>
                <th class="title">
                    Company<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Company" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>     
            <tr>
                <th class="title">
                    Address 1<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Address1" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>
            <tr>
                <th class="title">
                    Address 2<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Address2" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>     
            <tr>
                <th class="title">
                    Cityh<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="City" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>  
            <tr>
                <th class="title">
                    State<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="State" runat="server" />
                    </div>
                </td>
                <td class="help">
                </td>
            </tr>
            <tr>
                <th class="title">
                    Zipcode<br />
                </th>
                <td>
                    <div class="errorbox-good">
                        <asp:TextBox ID="Zipcode" runat="server" />
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
