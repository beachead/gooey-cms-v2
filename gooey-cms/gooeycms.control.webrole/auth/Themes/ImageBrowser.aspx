<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageBrowser.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Themes.ImageBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <script src="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dojo/dojo.xd.js" djconfig="parseOnLoad: true"
        type="text/javascript"></script>
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

    <telerik:RadAjaxManager ID="AjaxScriptManager" runat="server">
    </telerik:RadAjaxManager>

    <div id="dialogErrorMessage" dojotype="dijit.Dialog" title="Error" style="display: none;
        width: 200px; height: 100px; overflow: auto;">
        <span id="lblErrorMessage"></span>
    </div>
    <div dojotype="dijit.layout.TabContainer" style="width: 675px; height: 395px; overflow: auto;">
        <div dojotype="dijit.layout.ContentPane" title="Image Upload">
            <asp:Label ID="LblUploadStatus" AutoUpdateAfterCallBack="true" runat="server" />
            <table style="width: 100%;">
                <tr>
                    <td colspan="2">
                        <asp:FileUpload ID="FileUpload" runat="server" /><br />
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="BtnUpload" OnClick="BtnUpload_Click" Text="Upload" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="LblUploadedFiles" runat="server" />
        </div>
        <div dojotype="dijit.layout.ContentPane" title="Image Library">
            <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
            <telerik:RadAjaxPanel ID="AjaxPanel" LoadingPanelID="LoadingPanel" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td colspan="2">
                        <asp:ImageButton ID="BtnRefreshImages" ImageUrl="~/images/Refresh.gif" OnClick="BtnRefreshImages_Click" runat="server" />&nbsp;Refresh
                        Images
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align:top;padding-top:7px;width:75%;">
                        <telerik:RadGrid ID="GridExistingImages" runat="server" AllowPaging="true" 
                            Skin="Default" DataSourceID="ImageDataSource" 
                            onitemcommand="GridExistingImages_ItemCommand">
                            <MasterTableView AutoGenerateColumns="false" AllowPaging="true" DataKeyNames="Guid" Width="100%" Height="100%" 
                            PageSize="10" DataSourceID="ImageDataSource">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Filename" ReadOnly="true" HeaderText="Filename" />
                                    <telerik:GridBoundColumn DataField="Length" ReadOnly="true" HeaderText="Length" />
                                    <telerik:GridTemplateColumn HeaderText="Actions">
                                        <ItemTemplate>
                                        <a href="#" onclick="<%# DataBinder.Eval(Container.DataItem, "Filename", "_imageclick(\'{0}\'); return false;") %>">select</a>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true">
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>
                            <PagerStyle Mode="Slider" />
                        </telerik:RadGrid>
                    </td>
                    <td style="vertical-align:top;">
                        <fieldset style="width: 230px; height: 220px">
                            <legend>Preview</legend>
                            <telerik:RadBinaryImage ID="ImagePreview" ResizeMode="Fit" AutoAdjustImageControlSize="true" Width="230" runat="server" />
                        </fieldset>                    
                    </td>
                </tr>
                <asp:ObjectDataSource ID="ImageDataSource" runat="server" 
                    EnablePaging="true"
                    TypeName="Gooeycms.Business.Images.ImageDataSource"
                    SelectMethod="GetThemeImages"
                    SelectCountMethod="GetThemeImageCount"
                    >
                </asp:ObjectDataSource>

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
            </telerik:RadAjaxPanel>
        </div>
    </div>
    </form>
</body>
</html>
