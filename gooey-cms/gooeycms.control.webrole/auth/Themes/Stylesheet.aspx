<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Stylesheet.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.Stylesheet" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<%@ MasterType VirtualPath="~/Secure.Master" %>

<asp:Content ID="ContentStylesheets" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" href="../../css/reorder.css" />
    <link rel="Stylesheet" href="../../css/dialog.css" />
</asp:Content>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="templates" navItem="stylesheets" />
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>Template CSS</h1>

    <beachead:StatusPanel ID="ErrorPanel" runat="server" />
    <asp:Label ID="LblStatus" runat="server" />

    <fieldset id="addStylesheet" runat="server">
        <legend>Add Stylesheet</legend>
        <p>You can add a new stylesheet by uploading an existing copy of by creating a new one.</p>
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
                <asp:RegularExpressionValidator ID="NewPatternValidator" ControlToValidate="TxtNewFileName" Display="None" ValidationExpression="[\w\d]+\.css" ValidationGroup="FileName" ErrorMessage="The file name may not contain spaces or special characters and must end in .css" runat="server" />
            </div>
        </div>
    </fieldset>
        

    <fieldset id="manageStylesheets" runat="server">
        <legend>Manage Existing Stylesheets</legend>
        <p>
            Edit existing stylesheets, manage which stylesheets are enabled for this theme, and define their order of occurrence.
        </p>

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
                    <em style="font-size:.8em;">click-and-drag to reorder</em>
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
                <h3>Edit an existing stylesheet</h3>
                <anthem:DropDownList ID="LstExistingFile" AutoUpdateAfterCallBack="true" runat="server" />
                <asp:Button ID="BtnEdit" OnClick="BtnEdit_Click" Text="Edit" runat="server" />   
                <asp:Button ID="BtnDelete" OnClick="BtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this javascript file?');" Text="Delete" runat="server" />
            </div>
        </div>

    </fieldset>

    <fieldset id="editStylesheetContent" runat="server" visible="false">
        <legend>Edit Stylesheet</legend>
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Default" runat="server" />
        <telerik:RadAjaxPanel ID="AjaxPanel" LoadingPanelID="LoadingPanel" runat="server">
            <asp:Label ID="SaveStatus" runat="server" /> <br />
            <beachead:Editor ID="Editor" ShowToolbar="false" runat="server" />
            <asp:Button ID="BtnSaveEdit" Text="Save" OnClick="BtnSaveEdit_Click" runat="server" />
            <asp:Button ID="BtnCancel" Text="Cancel" OnClick="BtnCancel_Click" runat="server" />
        </telerik:RadAjaxPanel>
    </fieldset>
</asp:Content>
