<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Stylesheet.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.Stylesheet" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a id="A2" href="~/auth/Themes/Default.aspx" runat="server">Manage Themes</a>:</li>
            <li><a id="A1" href="~/auth/Themes/Add.aspx" runat="server">Add Theme</a></li>
            <li>Manage Stylesheets</li>
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
This page allows you to associate css files from your global library to your theme
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <script type="text/javascript">
        dojo.addOnLoad(function () { dijit.byId('mainTabContainer').selectChild('<% Response.Write(OutsideSelectedPanel); %>'); });
        dojo.addOnLoad(function () { dijit.byId('stylesheetTabContainer').selectChild('<% Response.Write(SelectedPanel); %>'); });
    </script>    

    <beachead:StatusPanel ID="ErrorPanel" runat="server" />
    <div id="mainTabContainer" dojoType="dijit.layout.TabContainer" style="width:90em;height:700px;overflow:auto;">
        <div id="modifypanel" dojoType="dijit.layout.ContentPane" title="Enable/Disable Stylesheets">
            From this page you can manage which stylesheets are enabled for this theme. <br /><br />
            To enable a style, choose it from the <i>disabled</i> list and click <i>enable</i>.<br />
            To disable a style, choose it from the <i>enabled</i> list and click <i>disable</i>.
            <br /><br />
            <table>
                <tr>
                    <td>
                        Disabled Stylesheets<br />
                        <asp:ListBox ID="LstDisabledFiles" SelectionMode="Multiple" Rows="10" Width="150px" runat="server" />
                    </td>
                    <td style="width:15px;">&nbsp;</td>
                    <td>
                        Enabled Stylesheets<br />
                        <asp:ListBox ID="LstEnabledFiles" SelectionMode="Multiple" Rows="10" Width="150px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="BtnEnableScripts" OnClick="BtnEnableScripts_Click" Text="Enable" runat="server" /></td>
                    <td><asp:Button ID="BtnDisableScripts" OnClick="BtnDisableScripts_Click" Text="Disable" runat="server" /></td>
                </tr>
            </table>
        </div>
        <div id="mylibrarypanel" dojoType="dijit.layout.ContentPane" title="My Styles Library">
            <div id="stylesheetTabContainer" dojoType="dijit.layout.TabContainer">
                <div id="uploadpanel" dojoType="dijit.layout.ContentPane" title="Upload Stylesheet">
                    From this page you can upload and add a new custom css file to your styles library.
                    <br /><br />

                    Upload:&nbsp;<asp:FileUpload ID="FileUpload" runat="server" />&nbsp;
                    <asp:Button ID="BtnUpload" Text="Upload" CausesValidation="false" OnClick="BtnUpload_Click" runat="server" />
                    <br /><br />
                    <b>or</b>
                    Create New:&nbsp;<asp:TextBox ID="TxtNewFileName" ValidationGroup="FilName" ToolTip="Input a name for this javascript file" runat="server" />&nbsp;
                    <asp:Button ID="BtnCreateNew" Text="Create" ValidationGroup="FileName" OnClick="BtnUpload_Click" runat="server" />

                    <asp:ValidationSummary ID="ValidationSummary" ValidationGroup="FileName" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredName" ControlToValidate="TxtNewFileName" Display="None" ErrorMessage="You must specify a name" ValidationGroup="FileName" runat="server" />
                    <asp:RegularExpressionValidator ID="NewPatternValidator" ControlToValidate="TxtNewFileName" Display="None" ValidationExpression="[\w\d]+\.css" ValidationGroup="FileName" ErrorMessage="The file name may not contain spaces or special characters and must end in .css" runat="server" />
                </div>
                <div id="editpanel" dojoType="dijit.layout.ContentPane" title="Edit Scripts">
                    From this page you can edit or delete any of the css files in your styles library.
                    <br /><br />

                    Choose Stylesheet:&nbsp;<anthem:DropDownList ID="LstExistingFile" AutoUpdateAfterCallBack="true" runat="server" />&nbsp;
                    <asp:Button ID="BtnEdit" OnClick="BtnEdit_Click" Text="Edit" runat="server" />   
                    <asp:Button ID="BtnDelete" OnClick="BtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this javascript file?');" Text="Delete" runat="server" />

                    <br /><hr />
                    Edit:<br />
                    <beachead:Editor ID="Editor" ShowToolbar="false" runat="server" />

                    <br />
                    <asp:Button ID="BtnSaveEdit" Text="Save" OnClick="BtnSaveEdit_Click" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
