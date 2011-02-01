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

    <br />
    <telerik:RadFormDecorator ID="FormDecorator" DecoratedControls="Fieldset,Label,Textbox" runat="server" />
    <fieldset style="width:550px;">
        <legend>Email Settings</legend>
        <label>Smtp Server</label><br />
        <asp:TextBox ID="TxtSmtpServer" Width="300px" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredField1" ControlToValidate="TxtSmtpServer" ErrorMessage="*" runat="server" />
        <br /><br />

        <label>Smtp Port</label><br />
        <asp:TextBox ID="TxtSmtpPort" Width="300px" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TxtSmtpPort" ErrorMessage="*" runat="server" />
        <br /><br />

        <label>Smtp Username</label><br />
        <asp:TextBox ID="TxtSmtpUsername" Width="300px" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TxtSmtpUsername" ErrorMessage="*" runat="server" />
        <br /><br />

        <label>Smtp Password</label><br />
        <asp:TextBox ID="TxtSmtpPassword" TextMode="Password" Width="300px" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="TxtSmtpPassword" ErrorMessage="*" runat="server" />
        <br /><br />
        <asp:Button ID="BtnUpdateSmtp" OnClick="BtnUpdateSmtp_Click" Text="Update" runat="server" />&nbsp;
        <asp:Button ID="BtnTestSettings" OnClick="BtnTestSettings_Click" Text="Test Settings" CausesValidation="false" runat="server" />
        <br />
        <asp:Label ID="LblStatus" runat="server" />
     </fieldset>

    <telerik:RadWindowManager ID="Singleton" Skin="Default" DestroyOnClose="true" Height="600" Modal="false" KeepInScreenBounds="true" ShowContentDuringLoad="false" AutoSize="false" VisibleStatusbar="false" Behaviors="Close,Move,Resize,Minimize,Maximize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
</asp:Content>
