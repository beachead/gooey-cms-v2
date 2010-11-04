<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Links.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Links" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Campaign Link Builder</title>
    <link id="Link1" href="~/css/admin-theme.css" media="all" rel="Stylesheet" type="text/css" runat="server" />
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <script src="../../scripts/functions.js" type="text/javascript" language="javascript"></script> 
    <script src="../../scripts/mootools-core.js" type="text/javascript" language="javascript"></script>    
    <script src="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dojo/dojo.xd.js" djConfig="parseOnLoad: true" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        dojo.require("dojo.parser");
        dojo.require("dijit.TitlePane");
        dojo.require("dijit.dijit");
        dojo.require("dijit.Dialog");
        dojo.require("dijit.layout.TabContainer");
        dojo.require("dijit.layout.ContentPane");
        dojo.require("dijit.form.ValidationTextBox");
        dojo.require("dijit.Tooltip");
    </script>
</head>
<body class="claro">
    <form id="form1" runat="server">
    <div>
    <div style="padding-top:15px;padding-left:15px;">
        <asp:Panel ID="InfoPanel" runat="server">
        Fill in the below information to generate an appropriate campaign tracking link.
        <br /><br />
        <table>
            <tr>
                <td colspan="2" style="padding-bottom:5px;">
                    Choose a link type: <br />
                    <anthem:RadioButton ID="RdoLandingPage" Text="Landing Page" AutoPostBack="true" GroupName="LinkType" Checked="true" OnCheckedChanged="RdoLinkType_Change" runat="server" />
                    <anthem:RadioButton ID="RdoFilePage" Text="File Download" AutoPostBack="true" GroupName="LinkType" OnCheckedChanged="RdoLinkType_Change" runat="server" />
                </td>
            </tr>
            <anthem:Panel ID="PnlLinkTypeFile" Visible="false" AutoUpdateAfterCallBack="true" runat="server">
            <tr>
                <td>File Download:</td>
                <td>
                    <asp:DropDownList ID="LstFileDownloads" runat="server" />
                </td>
            </tr>
            </anthem:Panel>
            <anthem:Panel ID="PnlLinkTypeLandingPage" Visible="true" AutoUpdateAfterCallBack="true" runat="server">
            <tr>
                <td>Landing Page:</td>
                <td>
                    <asp:DropDownList ID="LandingPage" runat="server" /> or custom&nbsp;
                    <asp:TextBox ID="CustomLandingPage" runat="server" />
                </td>
            </tr>
            </anthem:Panel>
            <tr>
                <td>Campaign Source:</td>
                <td>
                <asp:TextBox ID="Source" dojoType="dijit.form.ValidationTextBox" promptMessage="Input the source for this campaign, such as email" required="true" runat="server" /> (internal, google, etc.) 
                <asp:RequiredFieldValidator ID="RequiredSource" ControlToValidate="Source" Display="None" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Link Type:</td>
                <td>
                    <asp:DropDownList ID="Type" runat="server">
                        <asp:ListItem Value="Internal" Text="Internal Link" />
                        <asp:ListItem Value="Internal" Text="Email" />
                        <asp:ListItem Value="Internal" Text="Cost-Per-Click" />
                        <asp:ListItem Value="Internal" Text="Banner Ad" />                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="General" Text="Generate" OnClick="Generate_Click" runat="server" /></td>
            </tr>                        
        </table>
        </asp:Panel>
        
        <asp:Panel ID="ResultPanel" Visible="false" runat="server">
            External Tracking Link:<br />
            <asp:TextBox ID="Result" Width="350px" runat="server" />
            <br /><br />
            Internal Tracking Link:<br />
            <asp:TextBox ID="InternalLink" Width="350px" runat="server" />            
        </asp:Panel>
    </div>    
    </div>
    </form>
</body>
</html>
