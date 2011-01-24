<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Javascript.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.Javascript" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<asp:Content ID="ContentStylesheets" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" href="../../css/reorder.css" />
    <link rel="stylesheet" href="../../css/dialog.css" />
</asp:Content>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="templates" navItem="javascript" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>Template JavaScript</h1>
    <p>Theme JavaScripts are available to all pages within your theme.  Use these tools to edit and delete existing scripts and to create or upload new scripts.</p>

    <beachead:StatusPanel ID="ErrorPanel" runat="server" />


    <fieldset id="addScript" runat="server">
        <legend>Add Script</legend>
        <div class="columns">
            <div class="column" style="width: 350px;">
                <h3>Upload File</h3>
                <asp:FileUpload ID="FileUpload" runat="server" />&nbsp;
                <asp:Button ID="BtnUpload" Text="Upload" CausesValidation="false" OnClick="BtnUpload_Click" runat="server" />
            </div>
            <div class="column last" style="width: 350px;">
                <h3>Create File</h3>

                <asp:TextBox ID="TxtNewFileName" ValidationGroup="FilName" ToolTip="Input a name for this javascript file" runat="server" />&nbsp;
                <asp:Button ID="BtnCreateNew" Text="Create" ValidationGroup="FileName" OnClick="BtnUpload_Click" runat="server" />

                <asp:ValidationSummary ID="ValidationSummary" ValidationGroup="FileName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredName" ControlToValidate="TxtNewFileName" Display="None" ErrorMessage="You must specify a name" ValidationGroup="FileName" runat="server" />
                <asp:RegularExpressionValidator ID="NewPatternValidator" ControlToValidate="TxtNewFileName" Display="None" ValidationExpression="[\w\d]+\.js" ValidationGroup="FileName" ErrorMessage="The file name may not contain spaces or special characters and must end in .js" runat="server" />
            </div>

        </div>
    </fieldset>


    <fieldset id="manageScripts" runat="server">
        <legend>Manage Existing Scripts</legend>

        <div class="columns">
            <div class="column" style="width:250px;">
                <h3>Enable</h3>
                <asp:ListBox ID="LstDisabledFiles" SelectionMode="Multiple" Rows="5" Width="200px" runat="server" />
                <br />
                <asp:Button ID="BtnEnableScripts" OnClick="BtnEnableScripts_Click" Text="Enable" runat="server" />
            </div>

            <div class="column" style="width:250px;">
                <h3>
                    Re-order<br />
                    <em style="font-size:.8em; font-weight: normal;">click-and-drag to reorder</em>
                </h3>

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
                <anthem:DropDownList ID="LstExistingFile" AutoUpdateAfterCallBack="true" runat="server" />
                <asp:Button ID="BtnEdit" OnClick="BtnEdit_Click" Text="Edit" runat="server" />   
                <asp:Button ID="BtnDelete" OnClick="BtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this javascript file?');" Text="Delete" runat="server" />
            </div>
        </div>

    </fieldset>

    <fieldset id="editScriptContent" runat="server" visible="false">
        <legend>Edit Script</legend>
        <beachead:Editor ID="Editor" ShowToolbar="false" UseStandardImageTags="true" runat="server" />
        <asp:Button ID="BtnSaveEdit" Text="Save" OnClick="BtnSaveEdit_Click" runat="server" />
    </fieldset>

</asp:Content>
