<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Content.Edit" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="content" NavItem="" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <script language="javascript" type="text/javascript">
        dojo.query("a").onclick(function (e) {
            if (!confirm('Are you sure you want to navigate away from this page')) {
                dojo.stopEvent(e);
            }
        });
    </script>     
    <div style="padding-bottom:10px;">
        <asp:Table ID="ControlTable" runat="server" />
    </div>

    <div style="padding-bottom:10px;">
    <beachead:Editor ID="TxtEditor" ShowPreviewWindow="false" ShowToolbar="false" Visible="false" runat="server" />
    </div>
        

    <div style="padding-bottom:10px;">
    <b>Tags:</b> (separate multiple tags with a comma)<br />
    <asp:TextBox ID="TxtTags" dojoType="dijit.form.ValidationTextBox" required="false" Width="350px" runat="server" />
    </div>  

    <div dojoType="dijit.TitlePane" style="width:450px;height:400px;"  title="Advanced Options" open="false"> 
        <div style="padding-bottom:10px;">
        <b>Require Registration:</b><br />
        <anthem:DropDownList ID="RequireRegistration" AutoCallBack="true" OnSelectedIndexChanged="RequireRegistration_Change" runat="server">
            <asp:ListItem Text="No" Value="False" />
            <asp:ListItem Text="Yes" Value="True" />
        </anthem:DropDownList>
        &nbsp;&nbsp;
        <anthem:DropDownList ID="RegistrationPage" AutoUpdateAfterCallBack="true" Visible="false" runat="server"></anthem:DropDownList>
        </div>

        <div style="padding-bottom:10px;">
            <b>Publish On:</b>(leave blank for immediately)<br />
            <bdp:BDPLite ID="PublishDate" runat="server" /> at <bdp:TimePicker ID="PublishTime" runat="server" />
            <br /><br />
            
            <b>Expire On:</b>(leave blank for no expiration)<br />
            <bdp:BDPLite ID="ExpireDate" runat="server" /> at <bdp:TimePicker ID="ExpireTime" runat="server" /><br />
        </div>      
    </div>

    <div style="padding-top:7px;">
    <asp:Button ID="BtnSaveContent" Text="Save" OnClick="BtnSaveContent_Click" runat="server" />
    </div>
</asp:Content>
