<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Developer.SendEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gooey CMS Management | {0}</title>
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
    <link rel="shortcut icon" href="/images/favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <script src="<%# ResolveUrl("~/scripts/functions.js") %>" type="text/javascript" language="javascript"></script> 
    <script src="<%# ResolveUrl("~/scripts/mootools-core.js") %>" type="text/javascript" language="javascript"></script>    
    <script src="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dojo/dojo.xd.js" djConfig="parseOnLoad: true" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        dojo.require("dojo.parser");
        dojo.require("dojo.window");
        dojo.require("dojo._base.html");
        dojo.require("dijit.TitlePane");
        dojo.require("dijit.dijit");
        dojo.require("dijit.Dialog");
        dojo.require("dijit.layout.TabContainer");
        dojo.require("dijit.layout.ContentPane");
        dojo.require("dijit.form.ValidationTextBox");
        dojo.require("dijit.Tooltip");
    </script>
</head>
<body>
    <script language="javascript" type="text/javascript">
        function closeWindow() {
            var win = window.frameElement.radWindow;   
            win.close();
        }
    </script>

    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>TO:</td>
                <td><asp:TextBox ID="ToAddress" Width="320px" runat="server" /></td>
            </tr>
            <tr>
                <td>FROM:</td>
                <td><asp:TextBox ID="FromAddress" Width="320px" Text="package-admin@gooeycms.com" runat="server" /></td>
            </tr>
            <tr>
                <td>SUBJECT:</td>
                <td><asp:TextBox ID="Subject" Width="320px" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="2">Message</td>
            </tr>
            <tr>
                <td colspan="2">
                <asp:TextBox ID="Body" Width="400px" Rows="10" TextMode="MultiLine" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="text-align:right;"><asp:Button ID="BtnSend" OnClick="BtnSend_Click" Text="Send" runat="server" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
