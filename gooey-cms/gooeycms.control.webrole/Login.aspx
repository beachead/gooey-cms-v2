<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gooeycms.Webrole.Control.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Gooey CMS Management | Login</title>
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
    <link rel="shortcut icon" href="/images/favicon.ico" type="image/x-icon" />
    <style type="text/css">
    td.left-column {
        width: 100%;
        vertical-align: top;
        background-color: #fff;
    }
    
    table.form {
        margin-left: 100px;
    }
    
    #logo {
        margin: 20px 60px;
        float: none;
    }
    </style>
</head>
<body>
<form id="form1" runat="server">

<!-- START: main -->
<div id="main">
    <div id="header-region"></div>

    
    
    <!-- START: content -->
    <table class="content">
    <tr>
    <td class="left-column">
    
        <div id="logo"><img src="images/logo_gooey-login.png" /></div>

        <asp:Login ID="LoginControl" OnLoggedIn="LoginControl_LoggedIn" runat="server">
        
            <LayoutTemplate>
                <table cellpadding="0" class="form">
                <tr>
                <td class="label">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email:</asp:Label>
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
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                        ControlToValidate="Password" ErrorMessage="Password is required." 
                        ToolTip="Password is required." ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>
                </td>
                </tr>
            
                <tr>
                <td></td>
                <td>
                    <asp:CheckBox ID="RememberMe" runat="server"  Text="Remember me" />
                </td>
                </tr>
            
                <tr>
                <td align="center" colspan="2" style="color:Red;">
                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                </td>
                </tr>
            
                <tr class="controls">
                <td colspan="2">
                    <asp:Button ID="LoginButton" runat="server" 
                    CommandName="Login" Text="Log In" ValidationGroup="LoginControl" 
                    CssClass="UserManagerListing" />
                </td>
                </tr>
                </table>
<script type="text/javascript" charset="utf-8">
    var is_ssl = ("https:" == document.location.protocol);
    var asset_host = is_ssl ? "https://s3.amazonaws.com/getsatisfaction.com/" : "http://s3.amazonaws.com/getsatisfaction.com/";
    document.write(unescape("%3Cscript src='" + asset_host + "javascripts/feedback-v2.js' type='text/javascript'%3E%3C/script%3E"));
</script>

<script type="text/javascript" charset="utf-8">
    var feedback_widget_options = {};

    feedback_widget_options.display = "overlay";
    feedback_widget_options.company = "gooey_cms";
    feedback_widget_options.placement = "left";
    feedback_widget_options.color = "#4395f1";
    feedback_widget_options.style = "question";








    var feedback_widget = new GSFN.feedback_widget(feedback_widget_options);
</script>
            </LayoutTemplate>

       </asp:Login>

    </td> 
    </tr>
    </table>
    <!-- END: content -->

</div>
<!-- END: main -->

</form>
</body>
</html>    
