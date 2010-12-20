<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoPackage.aspx.cs" EnableSessionState="False" Inherits="Gooeycms.Webrole.Control.auth.Developer.DoPackage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gooey CMS | Packaging Site into Cloud</title>
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
    
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server" />
        <telerik:RadAjaxManager id="RadAjaxManager1" runat="server" RequestQueueSize="100" OnAjaxRequest="AjaxManager_OnRequest" ClientEvents-OnResponseEnd="closeWindow();">
        </telerik:RadAjaxManager>


        <script language="javascript" type="text/javascript">
            function closeWindow() {
                var win = window.frameElement.radWindow;
                win.BrowserWindow.redirect_to_default();
            }
        </script>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
        
        </telerik:RadAjaxPanel>
        <telerik:RadProgressManager ID="ProgressManager" EnableMemoryOptimization="true" runat="server" />
        <telerik:RadProgressArea ID="ProgressArea"  runat="server" Skin="Windows7" />        
        
        <telerik:RadCodeBlock ID="cb1" runat="server">
        <script language="javascript" type="text/javascript">
            var isRunOnce = false;
            function pageLoad() {
                if (!isRunOnce) {
                    var manager = $find("<%= RadAjaxManager1.ClientID %>");
                    if (manager)
                        manager.ajaxRequest('<%=Request.QueryString["g"] %>');
                    isRunOnce = true;
                }
            }
        </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
