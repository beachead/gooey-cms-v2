<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Purchase.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.store.Purchase" %>

<asp:Content ContentPlaceHolderID="localCSS" runat="server" ID="localCSS">
    <link rel="Stylesheet" type="text/css" href="/css/store.css" />
    <style type="text/css">
    div.columns .left-column {
        width: 400px;
    }
    
    div.columns .right-column {
        width: 300px;
        padding-top: 50px;
    }    
    </style>
</asp:Content>

<asp:Content id="localJS" ContentPlaceHolderID="localJS" runat="server">
    <script type="text/javascript" src="/js/store.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
	
    <!-- START: content -->
	<div id="content">
        <h2>Sign In</h2>
        <p>Before we can process your order you must first re-sign in to your account. You will be able to review and confirm your purchase after signing in.</p>
        <p>If you are not currently a GooeyCMS subscriber you can sign-up for your <asp:LinkButton ID="LnkNewUser2" OnClick="LnkNewUser_Click" runat="server">free account</asp:LinkButton> now!</p>
        <br />

        <!-- START: columns -->
        <div class="columns">

            <!-- START: left-column -->
            <div class="left-column">

                <ul id="themes-panel" class="collapse-margin">
                <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" runat="server">
                    <ItemTemplate>
   			            <li class="theme">  
				            <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c}") %></div>
				            <div class="logo"><img src="../images/___placeholder_logo.png" width="93" height="27" alt="" /></div>
				                <!-- start: preview -->
                            <div class="preview">
                                <ul class="thumbs">
                                        <asp:Repeater ID="ThumbnailImages" runat="server">
                                            <ItemTemplate>
                                                <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                            </ItemTemplate>
                                        </asp:Repeater>
				                </ul>

				                <ul class="thumb-nav"></ul>
				        
				                <div class="features">
                                    <a href="#" class="showFeatures">Features</a>
				                    <ul>
                                        <asp:Repeater ID="FeatureList" runat="server">
                                            <ItemTemplate>
                                                <li><%# Container.DataItem %></li>
                                            </ItemTemplate>
                                        </asp:Repeater>
				                    </ul>
                                </div>
                            </div>
                            <!-- end: preview -->
			            </li>                                      
                    </ItemTemplate>
                </asp:Repeater>
                </ul>                
                
            </div>
            <!-- END: left-column -->


            <!-- START: right-column -->
            <div class="right-column">

                <p><strong>Please Sign In To Process Your Purchase</strong></p>
                    
                    <asp:Login ID="LoginControl"  OnLoggedIn="LoginControl_LoggedIn" runat="server">
                    <LayoutTemplate>
                        <table cellspacing="0" class="form">
                        <tr>
                        <td class="label">
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" >User Name: </asp:Label>
                        </td>
                            
                        <td>
                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                ToolTip="User Name is required." ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
                        </td>
                        </tr>
                            
                        <tr>
                        <td class="label">
                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" >Password:</asp:Label>
                        </td>
                            
                        <td>
                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                ControlToValidate="Password" ErrorMessage="Password is required." 
                                ToolTip="Password is required." ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
                        </td>
                        </tr>
                            
                        <tr>
                        <td class="error" colspan="2">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        </td>
                        </tr>
                            
                        <tr>
                        <td class="controls" colspan="2">
                            <asp:LinkButton ID="LnkNewUser" OnClick="LnkNewUser_Click" runat="server">New User?</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="LoginButton" runat="server" 
                                CommandName="Login" Text="Log In" ValidationGroup="LoginControl" CssClass="UserManagerListing" />
                        </td>
                        </tr>
                        </table>
                    </LayoutTemplate>
                    <TextBoxStyle Width="150px" />
                </asp:Login>                                

            </div>
            <!-- END: right-column -->

        </div>
        <!-- END: columns -->
  
    </div>
    <!-- END: content -->

</asp:Content>
