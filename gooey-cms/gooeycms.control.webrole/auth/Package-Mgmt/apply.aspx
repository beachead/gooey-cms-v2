<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="apply.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Package_Mgmt.apply" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="ScriptManager" AsyncPostBackTimeout="600" runat="server" />
        <telerik:RadAjaxManager id="RadAjaxManager1" runat="server" OnAjaxRequest="AjaxManager_OnRequest" ClientEvents-OnResponseEnd="closeWindow();">
          <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ProgressArea" />
                </UpdatedControls>
             </telerik:AjaxSetting>
           </AjaxSettings>
        </telerik:RadAjaxManager>

        <script language="javascript" type="text/javascript">
            function closeWindow() {
                alert('Successfully applied the site.');
                var win = window.frameElement.radWindow;
                win.close();
            }
        </script>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
        
        </telerik:RadAjaxPanel>
        <telerik:RadProgressManager ID="ProgressManager" runat="server" />
        <telerik:RadProgressArea ID="ProgressArea" runat="server" Skin="Windows7" />        
        
        <telerik:RadCodeBlock ID="cb1" runat="server">
        <script language="javascript" type="text/javascript">
            var isRunOnce = false;
            function pageLoad() {
                if (!isRunOnce) {
                    var manager = $find("<%= RadAjaxManager1.ClientID %>");
                    if (manager)
                        manager.ajaxRequest('<%=Request.QueryString["s"] %>|<%=Request.QueryString["g"] %>');
                    isRunOnce = true;
                }
            }
        </script>
        </telerik:RadCodeBlock>
    </div>
    </form>
</body>
</html>
