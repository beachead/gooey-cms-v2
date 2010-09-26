<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Purchase.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.store.Purchase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
	<!-- START: content -->
	<div id="content">
        <div style="padding-left:25px;">
            Before we can process your order you must first re-sign in to your account. You will be able to review and confirm your purchase after signing in.
            If you are not currently a GooeyCMS subscriber you can sign-up for your <asp:LinkButton ID="LnkNewUser2" OnClick="LnkNewUser_Click" runat="server">free account</asp:LinkButton> now!
            <br />
            <table width="100%;">
                <tr>
                    <td style="padding-top:15px;padding-bottom:15px;padding-right:5px;vertical-align:top;width:50%;font-size:11px;">
                        <table style="border:1px solid #c0c0c0;width:100%;height:100%;">
                            <tr>
                                <td colspan="2" style="padding-left:5px;"><b>Please Sign In To Process Your Purchase:</b><br /><br /></td>
                            </tr>
                            <tr>
                                <td style="padding-left:5px;">
                            <asp:Login ID="LoginControl"  OnLoggedIn="LoginControl_LoggedIn" runat="server" 
                                CssClass="content-item" style="margin-left: 21px" Width="320px">
                                <LayoutTemplate>
                                    <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                                        <tr>
                                            <td class="style3">
                                                <table cellpadding="0">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" 
                                                                Font-Bold="True">User Name: </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="UserName" runat="server" Height="25px" Width="150px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                                                ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                                                ToolTip="User Name is required." ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="style1">
                                                        </td>
                                                        <td class="style1">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" 
                                                                Font-Bold="True">Password:   </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Password" runat="server" Height="25px" TextMode="Password" 
                                                                Width="150px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                                                ControlToValidate="Password" ErrorMessage="Password is required." 
                                                                ToolTip="Password is required." ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2" style="color:Red;">
                                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <asp:LinkButton ID="LnkNewUser" OnClick="LnkNewUser_Click" runat="server">New User?</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="LoginButton" runat="server" 
                                                                CommandName="Login" Text="Log In" ValidationGroup="LoginControl" 
                                                                Width="58px" CssClass="UserManagerListing" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <TextBoxStyle Width="150px" />
                            </asp:Login>                                
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding-right:10px;">
                        <ul id="themes-panel">
                        <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" runat="server">
                            <ItemTemplate>
   			                    <li class="theme">  
				                    <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c}") %></div>
				                    <div class="logo"><img src="../images/___placeholder_logo.png" width="93" height="27" alt="" /></div>
				                    <ul class="thumbs">
                                            <asp:Repeater ID="ThumbnailImages" runat="server">
                                                <ItemTemplate>
                                                    <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
				                    </ul>
				                    <ul class="thumb-nav"></ul>
				                    <ul class="features">
                                            <asp:Repeater ID="FeatureList" runat="server">
                                                <ItemTemplate>
                                                    <li><%# Container.DataItem %></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
				                    </ul>
			                    </li>                                      
                                </ItemTemplate>
                         </asp:Repeater>
                         </ul>
                    </td>
                </tr>
             </table>
        </div>
    </div>
</asp:Content>
