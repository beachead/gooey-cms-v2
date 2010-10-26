<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Javascript.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Pages.Javascript" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <link rel="stylesheet" href="../../css/reorder.css" />
    <script src="/scripts/functions.js" type="text/javascript" language="javascript"></script> 
    <script src="/scripts/mootools-core.js" type="text/javascript" language="javascript"></script> 
    <script src="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dojo/dojo.xd.js" djConfig="parseOnLoad: true" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        dojo.require("dojo.parser");
        dojo.require("dijit.TitlePane");    
        dojo.require("dijit.dijit");
        dojo.require("dijit.Dialog");
        dojo.require("dijit.layout.TabContainer");
        dojo.require("dijit.layout.ContentPane");
    </script>
</head>
<body class="claro">
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager" EnableCdn="true" EnablePageMethods="true" runat="server" />
        <script language="javascript" type="text/javascript">
            function showErrorMessage(text) {
                dojo.byId('lblErrorMessage').innerHTML = text;
                dijit.byId('dialogErrorMessage').show();
            }
        </script>
        <div id="dialogErrorMessage" dojoType="dijit.Dialog" title="Error" style="display:none;width:200px;height:100px;overflow:auto;">
            <span id="lblErrorMessage"></span>
        </div>

        <script type="text/javascript">
            dojo.addOnLoad(function () { dijit.byId('mainTabContainer').selectChild('<% Response.Write(SelectedPanel); %>'); });
        </script>  
         <div id="mainTabContainer" dojoType="dijit.layout.TabContainer" style="width:880px;height:560px;overflow:auto;">
            <div id="enablepanel" dojoType="dijit.layout.ContentPane" title="Enable/Disable Scripts">
                <table>
                <tr>
                    <asp:Panel ID="DisablePanel" AutoUpdateAfterCallBack="true" Visible="true" runat="server">
                    <td style="vertical-align:top;">
                        Disabled Scripts<br />
                        <asp:ListBox ID="LstDisabledFiles" SelectionMode="Multiple" Rows="10" Width="150px" runat="server" />
                        <br />
                        <asp:Button ID="BtnEnableScripts" OnClick="BtnEnableScripts_Click" Text="Enable" runat="server" />
                    </td>
                    </asp:Panel>
                    <td style="padding-left:7px;vertical-align:top;">
                        Enabled Scripts:(click-and-drag to reorder)<br />
                        <ajaxToolkit:ReorderList ID="LstEnabledFilesOrderable" CssClass="ajaxOrderedList"  PostBackOnReorder="false" 
                                                OnItemReorder="LstEnabledFiles_Reorder" 
                                                OnItemCommand="LstEnabledFiles_ItemCommand"
                                                DragHandleAlignment="Left" AllowReorder="true" runat="server">
                            <ItemTemplate>
                                <div class="<%# ((Container.DisplayIndex % 2) == 0) ? "" : "alt" %>">
                                    <%# Eval("Name") %>&nbsp;
                                    <asp:LinkButton ID="LnkDisableScript" CssClass="normal" CommandName="Disable" CommandArgument='<%# Eval("Name") %>' Text="Disable" runat="server" />
                                </div>
                            </ItemTemplate>
                            <DragHandleTemplate>
                                <div style="padding-right:5px;cursor:move;">
                                <img src="../../images/drag_arrow.png" alt="drag" />
                                </div>
                            </DragHandleTemplate>
                            <ReorderTemplate>
                                <div style="height :20px; border: dotted 2px black;"></div>
                            </ReorderTemplate>
                        </ajaxToolkit:ReorderList>
                        <br /><br />
                    </td>
                </tr>
                </table>                
            </div>
            <div id="managepanel" dojoType="dijit.layout.ContentPane" title="Manage Scripts" style="display:none;">
                <div dojoType="dijit.TitlePane" title="Add Javascript">
                    <table>
                        <tr>
                            <td>
                                Upload File:<br />
                                <asp:FileUpload ID="FileUpload" runat="server" />&nbsp;
                                <asp:Button ID="BtnUpload" OnClick="BtnUpload_Click" Text="Upload" runat="server" />                            
                            </td>
                            <td><br />&nbsp;<b>or</b>&nbsp;</td>
                            <td>
                                Create File:<br />
                                <asp:TextBox ID="TxtFileName" ValidationGroup="FilName" Width="250px" ToolTip="Input a name for this javascript file" runat="server" />&nbsp;
                                <asp:Button ID="BtnCreateNew" OnClick="BtnCreate_Click" Text="Create" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                 <div dojoType="dijit.TitlePane" title="Edit">
                 <table>
                    <tr>
                        <td>
                            Edit Existing 
                            <asp:DropDownList ID="LstExistingFile" runat="server" />&nbsp;<asp:Button ID="BtnEdit" OnClick="BtnEdit_Click" Text="Edit" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contents:<br />
                            <beachead:Editor ID="Editor" ShowToolbar="false" UseStandardImageTags="true" ShowPreviewWindow="false" runat="server" />                        
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />                        
                        </td>
                    </tr>
                 </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
