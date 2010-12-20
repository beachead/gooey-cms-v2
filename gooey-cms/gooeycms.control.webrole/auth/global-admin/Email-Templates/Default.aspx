<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Email_Templates.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
    Manage the Email Templates Below.
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <div>
        <asp:LinkButton ID="LnkModifyEmail" OnCommand="LnkModifyTemplate_Click" CommandArgument="signup" Text="Modify Sign-Up Email Template" runat="server" />
    </div>

    <div>
    <asp:LinkButton ID="LnkCancelEmail" OnCommand="LnkModifyTemplate_Click" CommandArgument="cancel" Text="Modify Cancellation Email Template" runat="server" />
    </div>

    <div>
    <asp:LinkButton ID="LnkPurchaseEmail" OnCommand="LnkModifyTemplate_Click" CommandArgument="purchase" Text="Modify Purchase Email Template" runat="server" />
    </div>

    <div>
        <beachead:Editor ID="TxtTemplate" Visible="false" ShowToolbar="false" ShowPreviewWindow="false" runat="server" />
    </div>

    <div>
        <asp:HiddenField ID="TemplateName" runat="server" />
        <asp:LinkButton ID="BtnSave" OnClick="BtnSave_Click" Text="Save Template" runat="server" />
    </div>

    <telerik:RadWindowManager ID="Singleton" Skin="Default" DestroyOnClose="true" Height="600" Modal="false" KeepInScreenBounds="true" ShowContentDuringLoad="false" AutoSize="false" VisibleStatusbar="false" Behaviors="Close,Move,Resize,Minimize,Maximize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
</asp:Content>
