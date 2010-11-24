<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageBrowser.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.ImageBrowser" %>

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
        dojo.require("dojox.image.Lightbox");
    </script>
</head>
<body class="claro">
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

         <div dojoType="dijit.layout.TabContainer" style="width:675px;height:395px;overflow:auto;">
            <div dojoType="dijit.layout.ContentPane" title="Image Upload">
                <anthem:Label ID="LblUploadStatus" AutoUpdateAfterCallBack="true" runat="server" />
                <table style="width:100%;">
                    <tr>
                        <td colspan="2"><asp:FileUpload ID="FileUpload" runat="server" /><br /><hr /></td>
                    </tr>
                    <tr>
                        <td><anthem:Button ID="BtnUpload" OnClick="BtnUpload_Click" Text="Upload" runat="server" /></td>
                    </tr>
                </table>
                <anthem:Label ID="LblUploadedFiles" AutoUpdateAfterCallBack="true" runat="server" />
            </div>

             <div dojoType="dijit.layout.ContentPane" title="Image Library">
                <table style="width:100%;">
                    <tr>
                        <td colspan="2"><b>Or Choose Existing Image:</b></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <anthem:Panel ID="PanelImageList" AutoUpdateAfterCallBack="true" Visible="true"  runat="server">
                                <asp:DataList ID="AvailableImageList" RepeatDirection="Horizontal" BackColor="#F7F6F3" 
                                    RepeatColumns="4" runat="server" CellPadding="6" CellSpacing="6">
                                    <ItemTemplate>
                                        <div style="width:125px;padding-bottom:5px;text-align:center;">
                                        <br />
                                        <span style="font-size:10px;"><%# DataBinder.Eval(Container,"DataItem.Filename") %></span>
                                        <br />
                                        <a href="#" onclick="javascript:_imageclick('<%# DataBinder.Eval(Container,"DataItem.Filename") %>'); return false;">Select</a>
                                        <a href="#" onclick="javascript:_imagedelete('<%# DataBinder.Eval(Container,"DataItem.Filename") %>'); return false;">Delete</a>
                                        </div>
                                    </ItemTemplate>
                                    <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </asp:DataList>
                            </anthem:Panel>
                        </td>
                    </tr>

                    <script language="javascript" type="text/javascript">
                        function _imageclick(name) {
                            window.opener.onimage_selected(name);
                        }
                        function _imagedelete(name) {
                            if (confirm('Are you sure you want to delete this image?')) {
                                alert('TODO: Implement delete functionality. Deleting ' + name);
                            }
                        }
                    </script>
                </table>
             </div>
         </div>
    </form>
</body>
</html>
