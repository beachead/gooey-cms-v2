<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Templates.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Themes.Templates" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="templates" navItem="templates" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <div class="page-controls">
        <ul>
            <li><asp:Button ID="BtnAddNewTemplate" OnClick="BtnAddTemplate_Click" Text="Add New Template" runat="server" /></li>
        </ul>
        </div>

    <asp:HiddenField ID="ThemeGuid" runat="server" />    
    <h1>Manage Templates: <asp:Label ID="ThemeName" runat="server" /></h1> 
    
    
    <asp:DropDownList ID="LstExistingTemplates" runat="server" />&nbsp;
    <asp:Button ID="BtnEditTemplate" OnClick="BtnEditTemplate_Click" Text="Edit" runat="server" />&nbsp;
    <asp:Button ID="BtnDeleteTemplate" OnClick="BtnDeleteTemplate_Click" OnClientClick="return confirm('Are you sure you want to delete this template? This will also delete all pages associated with this template.\n\nThis action can NOT be reversed!');" Text="Delete" runat="server" />    
    
    <br /><hr />
    <asp:Panel ID="ManageTemplatePanel" Visible="false" runat="server">
    <asp:HiddenField ID="ExistingTemplateId" runat="server" />
    <br />
    <table style="width:100%;">
        <tr>
            <td colspan="2" style="padding-bottom:10px;"><asp:Label ID="LblStatus" runat="server" /></td>
        </tr>
        <tr>
            <td style="width:70px;">Template</td>
            <td>
                <asp:Label ID="ExistingTemplateName" Visible="false" Font-Bold="true" runat="server" />
                <asp:DropDownList ID="TemplateType" runat="server" />
                <asp:LinkButton ID="AddCustomType" Text=" or add custom template" OnClientClick="return confirm('Are you sure you want to add a custom template? Custom templates cannot be transferred between purchased themes or sites.');" OnClick="AddCustomType_Click" runat="server" />
                
                <asp:Label ID="CustomTemplateTypeLabel" Text="Name:" Visible="false" runat="server" />&nbsp;<asp:TextBox ID="CustomTemplateType" Visible="false" runat="server" />
                <asp:LinkButton ID="UseGlobalType" Text=" or use global template" OnClick="AddCustomType_Click" Visible="false" runat="server" />
            </td>
        </tr>
    </table>
    <div id="template-editor">
        <table style="width:800px;">
            <tr>
                <td colspan="2">
                    <beachead:Editor ID="TemplateContent" ShowToolbar="false" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Button ID="BtnSave" OnClick="OnSave_Click" Text="Save" runat="server" /><br />      
    </div>
    </asp:Panel>    
    
    <script language="javascript" type="text/javascript">
        function FCKeditor_OnComplete(instance) {
            window.mytimeout = window.setTimeout(function() {
                instance.SwitchEditMode();
                var fixit = instance.GetHTML(false);
                if (fixit.substr(0, 6) == "<br />") {
                    fixit = fixit.substring(6);
                    instance.SetHTML(fixit);
                }
            }, 500);
        }
                
    </script>    
</asp:Content>
