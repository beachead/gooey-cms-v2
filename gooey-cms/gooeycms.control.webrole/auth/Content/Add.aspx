<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Content.Add" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="./Default.aspx">SITE CONTENT</a></li>
            <li><a href="">PROMOTION</a></li> 
            <li><a href="./ContentTypes.aspx">MANAGE CONTENT TYPES</a></li>       
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <script language="javascript" type="text/javascript">
        dojo.query("a").onclick(function (e) {
            if (!confirm('Are you sure you want to navigate away from this page')) {
                dojo.stopEvent(e);
            }
        });
    </script>
    <div dojoType="dijit.TitlePane" title="Choose Content Type">    
        <asp:DropDownList ID="LstContentTypes" runat="server" />&nbsp
        <asp:Button ID="BtnChooseContentType" OnClick="BtnChooseContentType_Click" Text="Create" CausesValidation="false" runat="server" />
    </div>
    
    <br />
    <asp:Panel ID="PanelAddContent" Visible="false" runat="server">
    <div dojoType="dijit.TitlePane" title="Define New Content">    
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

        <div dojoType="dijit.TitlePane" title="Advanced Options" open="false"> 
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
        <asp:Button ID="BtnAddContent" OnClick="BtnAddContent_Click" Text="Add" runat="server" />
        </div>
    </div>
    </asp:Panel>
</asp:Content>
