<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Signup-Review.aspx.cs" Inherits="gooeycms.webrole.website.SignupReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div id="content">
    <ol id="signup-steps">
	    <li>
		    <h2><img src="images/h2_create_gooey_acct.png" width="435" height="35" alt="create your gooey cms account" /> [NEED IMAGE]</h2>
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
                        <asp:Panel ID="PaypayPanel" Visible="true" runat="server">
                            <input type="hidden" name="cmd" value="_xclick-subscriptions">
                            <input type="hidden" name="business" value="seller_1272749312_biz@gmail.com" />
                            <input type="hidden" name="item_name" value="<%= PaypalDescription %>" />
                            <input type="hidden" name="a1" value="0" />
                            <input type="hidden" name="p1" value="1" />
                            <input type="hidden" name="t1" value="M" />
                            <input type="hidden" name="a3" value="<%= PaypalCost %>" />
                            <input type="hidden" name="p3" value="1" />
                            <input type="hidden" name="t3" value="M" />
                            <input type="hidden" name="src" value="1" />
                            <input type="hidden" name="sra" value="1" />
                            <input type="hidden" name="custom" value="<%= Guid %>" />
                            <input type="hidden" name="return" value="<%=ReturnUrl %>" />
                            <asp:ImageButton ID="ImageButton1" ImageUrl="https://www.sandbox.paypal.com/en_US/i/btn/btn_subscribeCC_LG.gif" PostBackUrl="https://www.sandbox.paypal.com/cgi-bin/webscr" runat="server" />                    
                        </asp:Panel>                        
                        <asp:Panel ID="FreePanel" Visible="false" runat="server">
                            <asp:ImageButton ID="BtnSubscribe" ImageUrl="images/btn_create_acct.png" OnClick="BtnSubscribe_Click" runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <asp:Panel ID="DebugPanel" Visible="false" runat="server">
                    <asp:HyperLink ID="SkipPaypal" Text="Create Account - Skip Paypal" runat="server" />
                </asp:Panel>
            </table>
        </li>
    </ol>
</div>
</asp:Content>
