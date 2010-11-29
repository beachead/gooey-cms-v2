<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Signup-Review.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.SignupReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div id="content">
    <ol id="signup-steps">
	    <li>
		    <h2><img src="../images/h2_create_gooey_acct.png" width="435" height="35" alt="create your gooey cms account" /> [NEED IMAGE]</h2>
            <table cellspacing="0" class="form">		    
                <tr>
                    <td class="label"><label for="first-name">First Name</label></td>
                    <td><asp:Label ID="Firstname" runat="server" /></td>
                </tr>
                <tr>
                    <td class="label"><label for="last-name">Last Name</label></td>
                    <td><asp:Label ID="Lastname" runat="server" /></td>
                </tr>
                <tr>
                    <td class="label"><label for="email">Email / Username</label></td>
                    <td><asp:Label ID="Email" runat="server" /></td>
                </tr>                                
                <tr>
                    <td class="label"><label for="company">Company</label></td>
                    <td><asp:Label ID="Company" runat="server" /></td>
                </tr>                  
                <tr>
                    <td class="label"><label for="email">Subdomain</label></td>
                    <td><asp:Label ID="Subdomain" runat="server" /></td>
                </tr>       
                <tr>
                    <td class="label"><label for="subscription-plan">Subscription Plan</label></td>
                    <td><asp:Label ID="SubscriptionPlan" runat="server" /></td>
                </tr>                                  
                <tr>
                    <td colspan="2" style="padding-top:10px;">
                        <asp:ImageButton ID="BtnSubscribe" ImageUrl="https://www.sandbox.paypal.com/en_US/i/btn/btn_subscribeCC_LG.gif" OnClick="BtnSubscribe_Click" runat="server" />                    
                    </td>
                </tr>
            </table>
        </li>
    </ol>
</div>
</asp:Content>
