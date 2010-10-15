<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gooeycms.Webrole.Control.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" profile="http://control.gooeycms.net" runat="server">
    <title>Gooey CMS Management | Login</title>
    <link href="http://control.gooeycms.net/images/gooey-icon.ico" rel="icon" />
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="header-region">
        </div>
        <div id="main">
           <table class="content" cellpadding="5" cellspacing="5" border="0">
            
                <tr>
                    <td class="center-column">
                       <div id="subnav">
                            <div id="subnav-inner" style="padding-top:9px;padding-left:5px;">
                                <b> Gooey CMS Management</b>
                            </div>
                        </div>
                        
                        <div id="instructions-area">
                        <img src="images/logo_gooey-login.png" 
                                style="padding-left: 42px; padding-top: 20px;" />
                        </div>
                        <div id="editor-area" style="padding-left: 104px">
                            <asp:Login ID="LoginControl" OnLoggedIn="LoginControl_LoggedIn" runat="server" 
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
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="RememberMe" runat="server" CssClass="remember" 
                                                                Text="Remember me" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2" style="color:Red;">
                                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
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
                        </div>
                    </td>
                </tr>
            </table>
 



        </div>                    
    </div>
    </form>
</body>
</html>    
