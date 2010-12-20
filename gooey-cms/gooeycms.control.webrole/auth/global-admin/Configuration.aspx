<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Configuration" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Modify Configuration</title>
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" language="javascript">
        function close_window() {
            alert('Successfully updated configuration value');
            var win = window.frameElement.radWindow;
            win.BrowserWindow.location.href = 'Default.aspx';
            win.close();            
        }
    </script>

    <telerik:RadAjaxPanel ClientEvents-OnResponseEnd="close_window();" EnableAJAX="true" runat="server">
    <div>
        <table>
            <tr>
                <td>Configuration Name: </td>
                <td><asp:Label ID="LblConfigurationName" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <b>Content:</b><br />
                    <asp:TextBox ID="TxtConfigurationValue" TextMode="MultiLine" Rows="10" Columns="62" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
