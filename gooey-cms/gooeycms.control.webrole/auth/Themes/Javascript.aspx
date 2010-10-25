<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Javascript.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.Javascript" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<asp:Content ID="ContentStylesheets" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" href="../../css/reorder.css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="localJS" ID="localJS" runat="server">
    <script type="text/javascript">
        dojo.addOnLoad(function () { dijit.byId('mainTabContainer').selectChild('<% Response.Write(OutsideSelectedPanel); %>'); });
        dojo.addOnLoad(function () { dijit.byId('javascriptTabContainer').selectChild('<% Response.Write(SelectedPanel); %>'); });
    </script>    
</asp:Content>


<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="manageTemplates" navItem="javascript" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <h1>Template JavaScript</h1>
    <p>This page allows you to associate javascript files from your global library to your theme</p>


    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager" EnableCdn="true" EnablePageMethods="true" runat="server" />

    <beachead:StatusPanel ID="ErrorPanel" runat="server" />
    <div id="mainTabContainer" dojoType="dijit.layout.TabContainer" style="width:90em;height:700px;overflow:auto;">
        <div id="modifypanel" dojoType="dijit.layout.ContentPane" title="Enable/Disable Scripts">
            From this page you can manage which scripts are enabled for this theme. <br /><br />
            To enable a script, choose it from the <i>disabled</i> list and click <i>enable</i>.<br />
            To disable a script, choose it from the <i>enabled</i> list and click <i>disable</i>.
            <br /><br />
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
        <div id="providedpanel" dojoType="dijit.layout.ContentPane" title="Provided Javascript Libraries">
            From this page you can link some of the most popular open-source javascript libraries available today.
            <br />THIS TAB IS NOT FUNCTIONAL YET -- WILL BE DEVELOPED IF TIME
            <br /><br />

            Choose which open-source javascript packages you would like to use with this theme:<br />
            <asp:CheckBoxList ID="ChkProviders" runat="server">
                <asp:ListItem Text="Dojo" Value="Dojo" />
                <asp:ListItem Text="Mootools" Value="Mootools" />
            </asp:CheckBoxList>
        </div>

        <div id="mylibrarypanel" dojoType="dijit.layout.ContentPane" title="My Javascript Library">
            <div id="javascriptTabContainer" dojoType="dijit.layout.TabContainer">
                <div id="uploadpanel" dojoType="dijit.layout.ContentPane" title="Upload Javascript">
                    From this page you can upload and add a new custom javascript files to your script library.
                    <br /><br />

                    Upload:&nbsp;<asp:FileUpload ID="FileUpload" runat="server" />&nbsp;
                    <asp:Button ID="BtnUpload" Text="Upload" CausesValidation="false" OnClick="BtnUpload_Click" runat="server" />
                    <br /><br />
                    <b>or</b>
                    Create New:&nbsp;<asp:TextBox ID="TxtNewFileName" ValidationGroup="FilName" ToolTip="Input a name for this javascript file" runat="server" />&nbsp;
                    <asp:Button ID="BtnCreateNew" Text="Create" ValidationGroup="FileName" OnClick="BtnUpload_Click" runat="server" />

                    <asp:ValidationSummary ID="ValidationSummary" ValidationGroup="FileName" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredName" ControlToValidate="TxtNewFileName" Display="None" ErrorMessage="You must specify a name" ValidationGroup="FileName" runat="server" />
                    <asp:RegularExpressionValidator ID="NewPatternValidator" ControlToValidate="TxtNewFileName" Display="None" ValidationExpression="[\w\d]+\.js" ValidationGroup="FileName" ErrorMessage="The file name may not contain spaces or special characters and must end in .js" runat="server" />
                </div>
                <div id="editpanel" dojoType="dijit.layout.ContentPane" title="Edit Scripts">
                    From this page you can edit or delete any of the javascript files in your script library.
                    <br /><br />

                    Choose Script:&nbsp;<anthem:DropDownList ID="LstExistingFile" AutoUpdateAfterCallBack="true" runat="server" />&nbsp;
                    <asp:Button ID="BtnEdit" OnClick="BtnEdit_Click" Text="Edit" runat="server" />   
                    <asp:Button ID="BtnDelete" OnClick="BtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this javascript file?');" Text="Delete" runat="server" />

                    <br /><hr />
                    Edit:<br />
                    <beachead:Editor ID="Editor" ShowToolbar="false" UseStandardImageTags="true" runat="server" />

                    <br />
                    <asp:Button ID="BtnSaveEdit" Text="Save" OnClick="BtnSaveEdit_Click" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
