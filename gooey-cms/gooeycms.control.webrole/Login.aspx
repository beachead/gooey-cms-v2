<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Gooeycms.Webrole.Control.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Administer | {0}</title>
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="header-region">
        </div>
        <div id="main">
            <table class="content" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="center-column">
                        <div id="subnav">
                            <div id="subnav-inner" style="padding-top:9px;padding-left:5px;">
                                <b>Gooey-CMS Management Console</b>
                            </div>
                        </div>
                        <div id="instructions-area">

                        </div>
                        <div id="editor-area">
                            <asp:Login ID="LoginControl" OnLoggedIn="LoginControl_LoggedIn" runat="server">
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
