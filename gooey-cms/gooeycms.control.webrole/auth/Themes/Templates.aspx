﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Templates.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Themes.Templates" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li class=""><a href="./">Manage Themes:</a></li>
            <li class="on">Manage Templates</li> 
            <li class=""><a href="./AddNewTheme.aspx">Add Theme</a></li>           
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <asp:HiddenField ID="ThemeGuid" runat="server" />    
    Managing Templates for: <b><asp:Label ID="ThemeName" runat="server" /></b><br />
    <b class="small">Existing Templates</b><br />    
    [EXISTING TEMPLATES WILL BE LISTED HERE]
    
    <br /><br />
    <table>
        <tr>
            <td>Template</td>
            <td>
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
                    <beachead:Editor ID="TemplateContent" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Button ID="BtnSave" OnClick="OnSave_Click" Text="Save" runat="server" /><br />      
    </div>
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
