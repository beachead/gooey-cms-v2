<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoPackage.aspx.cs" EnableSessionState="False" Inherits="Gooeycms.Webrole.Control.auth.Developer.DoPackage" %>
<%@ Register Assembly="MattBerseth.WebControls.AJAX" Namespace="MattBerseth.WebControls.AJAX.Progress" TagPrefix="mb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gooey Package Tool</title>
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <script src="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dojo/dojo.xd.js" djConfig="parseOnLoad: true" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        dojo.require("dojox.timing._base");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" EnablePageMethods="true" />
    <asp:HiddenField ID="ExistingGuid" runat="server" />
    <div style="padding:5px;">
        <div>
        <b>Currently Packaging Site: <asp:Label ID="LblSiteTitle" runat="server" /></b><br /><br />
        Activity:&nbsp;<span id="lblevent" style="font-weight:bold;font-style:italic;"></span><br />
        <table>
            <tr>
                <td style="vertical-align:middle;padding-top:15px;padding-right:5px;"><mb:ProgressControl ID="ProgressControl" CssClass="green" runat="server" Width="300px" />&nbsp;</td>
                <td><div id="spinner"><img id="Img2" alt="Spinner" src="~/images/spinner.gif" runat="server" /></div></td>
            </tr>
        </table>
        </div>

        <div style="position:absolute;bottom:10px;">
            <b>Do not close this window until the process completes</b>        
        </div>
    </div>

        <script language="javascript" type="text/javascript">
            function updateStatus() {
                PageMethods.DoUpdateStatus('<%= ExistingGuid.Value %>', onUpdateSuccess, onUpdateFailure);
            }

            function createPackage() {
                //Invoke the ajax call to create the package
                PageMethods.DoPackageSite('<%= ExistingGuid.Value %>', onPackageSuccess, onPackageFailure);
            }

            var isDone = false;
            function onUpdateSuccess(result, userContext, methodName) {
                var arr = result.split(',');
                $get('lblevent').innerHTML = (arr[1]);
                $find('ProgressControl').set_percentage(arr[0]);

                if (isDone) {
                    timer.stop();
                    $get('lblevent').innerHTML = 'Package Completed Successfully!';
                    $get('spinner').style.display = 'none';
                }
            }

            function onUpdateFailure(error, userContext, methodName) {
                alert(error.get_message());
            }

            function onPackageSuccess(result, userContext, methodName) {
                isDone = true;
            }

            function onPackageFailure(error, userContext, methodName) {
                timer.stop();
                $get('lblevent').innerHTML = 'There was an error creating the package!';
                $get('lblevent').style.color = 'red';
                $get('spinner').style.display = 'none';
                alert('There was a problem creating your package:\n\n' + error.get_message());
            }

            var timer = new dojox.timing.Timer();
            timer.setInterval(1800);
            timer.onTick = updateStatus;
            timer.start();

            createPackage();
        </script>
    </form>
</body>
</html>
