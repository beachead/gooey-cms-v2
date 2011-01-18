<%@ Page Title="Campaign Elements" Language="C#" MasterPageFile="~/PopupWindow.Master"
    AutoEventWireup="true" CodeBehind="Elements.aspx.cs" ValidateRequest="false"
    Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Elements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadFormDecorator ID="FormDecorator" EnableRoundedCorners="true" DecoratedControls="Fieldset,Textbox,Textarea,Label"
        Skin="Default" runat="server" />

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel" Skin="Default" runat="server" />
    <telerik:RadAjaxPanel ID="AjaxPanel" Skin="Default" LoadingPanelID="AjaxLoadingPanel"
        runat="server">
        <div style="padding: 10px;">
            Campaign elements allow you to create "snippets" of HTML code and target them to
            individual pages. 
            <br />
            <br />
            <fieldset style="width: 600px;">
                <legend>Manage Elements</legend>
                <asp:LinkButton ID="BtnAddNew" OnClick="BtnAddNew_Click" Text="Add New Element" CausesValidation="false" runat="server" />&nbsp;<b>or</b>&nbsp;Edit
                Existing:&nbsp;
                <asp:DropDownList ID="LstExistingElements" runat="server" />
                &nbsp;
                <asp:LinkButton ID="BtnEdit" OnClick="BtnEdit_Click" CausesValidation="false" Text="Edit" runat="server" />&nbsp;
                <asp:LinkButton ID="BtnDelete" OnClick="BtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this campaign element?')" CausesValidation="false" Text="Delete" runat="server" />
            </fieldset>
            <div style="padding-top: 10px;">
                <fieldset style="width: 600px;">
                    <legend>Element Content</legend>
                    <table cellpadding="2" cellspacing="2">
                        <tr>
                            <td>
                                Name
                            </td>
                            <td>
                                <asp:TextBox ID="TxtName" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredField1" ControlToValidate="TxtName" ErrorMessage="*" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Placement
                            </td>
                            <td>
                                <asp:HiddenField ID="ElementGuid" runat="server" />
                                <asp:DropDownList ID="LstPlacement" runat="server">
                                    <asp:ListItem Text="Top" Value="top" />
                                    <asp:ListItem Text="Middle" Value="middle" />
                                    <asp:ListItem Text="Bottom" Value="bottom" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Priority
                            </td>
                            <td>
                                <asp:TextBox ID="TxtPriority" MaxLength="2" Width="30px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; padding-top: 5px;">
                                Targetted Pages
                            </td>
                            <td>
                                <asp:ListBox ID="LstSelectedPages" SelectionMode="Multiple" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; padding-top: 20px;">
                                Element Code
                                <div style="padding-top: 5px;">
                                    <label>
                                        Need a campaign link?&nbsp;<br />
                                        <asp:HyperLink ID="LnkGenerateLink" runat="server">Generate one now!</asp:HyperLink>
                                    </label>
                                </div>
                            </td>
                            <td>
                                <label>Insert HTML or GooeyScript below.<br />(GooeyMarkup is not supported on campaign elements)</label>
                                <br />
                                <asp:TextBox ID="TxtContent" Rows="10" Columns="50" TextMode="MultiLine" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="BtnSave" OnClick="BtnSave_Click" Text="Save" runat="server" />&nbsp;
                                <asp:Label ID="LblStatus" ForeColor="Green" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="width: 600px">
                    <legend>Embed Code</legend>Insert the below markup tags in the pages or page templates
                    you wish to target
                    <br />
                    <div style="padding-top: 4px;">
                        {campaign-top}
                        <br />
                        {campaign-middle}
                        <br />
                        {campaign-bottom}
                        <br />
                    </div>
                </fieldset>
            </div>
        </div>
    </telerik:RadAjaxPanel>

    <telerik:RadWindowManager ID="RadWindowManager" Modal="false" Width="600px" Height="270px" Behaviors="Close,Resize,Pin,Move" Skin="Default" runat="server" />
</asp:Content>
