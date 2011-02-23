<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageBrowser.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Themes.ImageBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <link rel="Stylesheet" type="text/css" href="../../css/dialog.css" />
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

    <telerik:RadScriptManager ID="ScriptManager" runat="server" />
    <telerik:RadAjaxManager ID="AjaxScriptManager" runat="server">
    </telerik:RadAjaxManager>

    <div id="dialogErrorMessage" dojotype="dijit.Dialog" title="Error" style="display: none;
        width: 200px; height: 100px; overflow: auto;">
        <span id="lblErrorMessage"></span>
    </div>

    <h2>Upload</h2>

    <asp:FileUpload ID="FileUpload" runat="server" />
    <asp:Button ID="BtnUpload" OnClick="BtnUpload_Click" Text="Upload" runat="server" />
    <asp:Label ID="LblUploadStatus" AutoUpdateAfterCallBack="true" runat="server" />
    <asp:Label ID="LblUploadedFiles" runat="server" />

    <hr />

    <h2>Image Library</h2>
    <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
    
    <telerik:RadAjaxPanel ID="AjaxPanel" LoadingPanelID="LoadingPanel" runat="server">

        <div class="columns">
            <div class="column" style="width: 400px;">
                <asp:ImageButton ID="BtnRefreshImages" ImageUrl="~/images/Refresh.gif" OnClick="BtnRefreshImages_Click" runat="server" /> Refresh Images

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
            </div>

            <div class="column last" style="width: 300px;">
                <fieldset style="width: 230px; height: 280px">
                    <legend>Preview</legend>
                    <telerik:RadBinaryImage ID="ImagePreview" ResizeMode="Fit" AutoAdjustImageControlSize="true" Width="230" runat="server" />
                </fieldset>                    
                <asp:ObjectDataSource ID="ImageDataSource" runat="server" 
                    EnablePaging="true"
                    TypeName="Gooeycms.Business.Images.ImageDataSource"
                    SelectMethod="GetThemeImages"
                    SelectCountMethod="GetThemeImageCount"
                    >
                </asp:ObjectDataSource>

            </div>
        </div>

    </telerik:RadAjaxPanel>

    </form>
</body>
</html>
