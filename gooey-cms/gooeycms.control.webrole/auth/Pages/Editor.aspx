<%@ Page Title="" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Editor.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Editor" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding-left:5px;">
        <p><asp:Label ID="Status" runat="server" /></p>

        <div class="page-group">
            <strong>Template:</strong><br />
            <asp:DropDownList ID="PageTemplate" runat="server" />
        </div>
        <beachead:Editor ID="PageMarkupText" TabIndex="5" runat="server" />
    
        <div style="padding-top:7px;">
        <asp:Button ID="BtnSave" OnClick="BtnSave_Click" Text="Save" runat="server" />   
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function showFormEditor() {
            alert('Show the form editor');
        }
    </script>
</asp:Content>
