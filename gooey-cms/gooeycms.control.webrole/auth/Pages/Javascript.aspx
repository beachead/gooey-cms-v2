<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Javascript.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Pages.Javascript" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <link rel="stylesheet" href="../../css/reorder.css" />
    <link rel="stylesheet" href="../../css/dialog.css" />
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
        <script language="javascript" type="text/javascript">
            function showErrorMessage(text) {
                dojo.byId('lblErrorMessage').innerHTML = text;
                dijit.byId('dialogErrorMessage').show();
            }
        </script>
        <div id="dialogErrorMessage" dojoType="dijit.Dialog" title="Error" style="display:none;width:200px;height:100px;overflow:auto;">
            <span id="lblErrorMessage"></span>
        </div>

        <fieldset id="addScript" runat="server">
            <legend>Add Script</legend>
            <div class="columns">
                <div class="column" style="width: 350px;">
                    <h3>Upload File</h3>
                    <asp:FileUpload ID="FileUpload" runat="server" />&nbsp;
                    <asp:Button ID="BtnUpload" OnClick="BtnUpload_Click" Text="Upload" runat="server" />                            
                </div>

                <div class="column last" style="width: 350px;">
                    <h3>Create File</h3>
                    <asp:TextBox ID="TxtFileName" ValidationGroup="FilName" Width="250px" ToolTip="Input a name for this JavaScript file" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredField1" ValidationGroup="Filename" ControlToValidate="TxtFileName" Text="*" runat="server" />
                    <asp:Button ID="BtnCreateNew" OnClick="BtnCreate_Click" ValidationGroup="Filename" Text="Create" runat="server" />
                </div>

            </div>
        </fieldset>

        <fieldset id="manageScripts" runat="server">
            <legend>Manage Existing Scripts</legend>
            
            <div class="columns">
                <div class="column" style="width:250px;">
                    <h3>Enable</h3>
                    <asp:ListBox ID="LstDisabledFiles" SelectionMode="Multiple" Rows="10" Width="150px" runat="server" />
                    <br />
                    <asp:Button ID="BtnEnableScripts" OnClick="BtnEnableScripts_Click" Text="Enable" runat="server" />
                </div>

                <div class="column" style="width:250px;">
                    <h3>Re-order</h3>
                    <p>click-and-drag to reorder</p>
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
                            <div style="padding-right:5px;cursor:move;"><img src="../../images/drag_arrow.png" alt="drag" /></div>
                        </DragHandleTemplate>
                        <ReorderTemplate>
                            <div style="height :20px; border: dotted 2px black;"></div>
                        </ReorderTemplate>
                    </ajaxToolkit:ReorderList>
                </div>

                <div class="column last" style="width: 250px;">
                    <h3>Edit an existing script</h3>
                    <asp:DropDownList ID="LstExistingFile" runat="server" />
                    <asp:Button ID="BtnEdit" OnClick="BtnEdit_Click" Text="Edit" runat="server" />
                </div>
            </div>

        </fieldset>

        <fieldset id="editScriptContent" runat="server" visible="false">
            <legend>Edit Script</legend>
            <beachead:Editor ID="Editor" ShowToolbar="false" UseStandardImageTags="true" ShowPreviewWindow="false" runat="server" />
            <asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
            <asp:Button ID="BtnCancel" Text="Cancel" OnClick="BtnCancel_Click" runat="server" />
        </fieldset>


    </form>
</body>
</html>
