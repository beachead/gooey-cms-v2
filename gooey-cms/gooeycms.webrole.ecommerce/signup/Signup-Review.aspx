<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Signup-Review.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.SignupReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div id="content">
    <ol id="signup-steps">
		    <h2><img src="../images/order_review.png" width="272" height="39" alt="Order Review" /></h2>
            <table cellspacing="0" class="form">		    
                <tr>
                    <td class="label"></td>
                    <td><b><asp:Label ID="Firstname" runat="server" />&nbsp;<asp:Label ID="Lastname" runat="server" /></b></td>
                </tr>
               <tr>
                    <td class="label"><label for="email">Username:</label></td>
                    <td><asp:Label ID="Email" runat="server" /></td>
                </tr>                                
                <tr>
                    <td class="label"><label for="company">Company:</label></td>
                    <td><asp:Label ID="Company" runat="server" /></td>
                </tr>                  
                <tr>
                    <td class="label"><label for="email">Website:</label></td>
                    <td><asp:Label ID="Subdomain" runat="server" /></td>
                </tr>       
                <tr>
                    <td class="label"></td>
                    <td></td>
                </tr>                                  
                 <tr>
                    <td class="label"></td>
                    <td><b>Subscription Details:</b></td>
                </tr>       
                <tr>
                    <td class="label"></td>
                    <td><asp:Label ID="SubscriptionPlan" runat="server" /></td>
                </tr>                                  
                   <tr>
                    <td class="label"></td>
                    <td></td>
                </tr>   
                <tr>
                     <td class="label"></td>
                     <td>
                        <asp:ImageButton ID="BtnSubscribe" ImageUrl="https://www.sandbox.paypal.com/en_US/i/btn/btn_subscribeCC_LG.gif" OnClick="BtnSubscribe_Click" runat="server" />                    
                    </td>
                </tr>
            </table>
    </ol>
</div>
</asp:Content>
