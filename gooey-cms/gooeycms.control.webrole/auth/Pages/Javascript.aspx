<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Javascript.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Javascript" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <script src="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dojo/dojo.xd.js" djConfig="parseOnLoad: true" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        dojo.require("dojo.parser");
        dojo.require("dijit.dijit");
        dojo.require("dijit.Dialog");
        dojo.require("dijit.layout.TabContainer");
        dojo.require("dijit.layout.ContentPane");
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <script language="javascript" type="text/javascript">
            function showErrorMessage(text) {
                dojo.byId('lblErrorMessage').innerHTML = text;
                dijit.byId('dialogErrorMessage').show();
            }
        </script>
        <div id="dialogErrorMessage" dojoType="dijit.Dialog" title="Error" style="display:none;width:200px;height:100px;overflow:auto;">
            <span id="lblErrorMessage"></span>
        </div>
        <div>
            <table style="width:100%;">
                <tr>
                    <td colspan="2"><asp:FileUpload ID="FileUpload" runat="server" /><br /><hr /></td>
                </tr>
                <tr>
                    <td><anthem:Button ID="BtnUpload" OnClick="BtnUpload_Click" Text="Upload" runat="server" /></td>
                </tr>
            </table>                
        </div>
    </form>
</body>
</html>
