<%@ Page Title="Activate Phone" Language="C#" MasterPageFile="~/PopupWindow.Master" AutoEventWireup="true" CodeBehind="ActivatePhone.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.ActivatePhone" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding:10px;">
    <asp:Label ID="LblStatus" runat="server" /><br />

    <asp:MultiView ID="PhoneNumberViews" runat="server">
        <asp:View ID="ConfigureView" runat="server">
            <div style="padding-bottom:10px;">
            Choose a phone number:<br />
            <asp:DropDownList ID="LstAvailablePhoneNumbers" runat="server" />
            </div>

            <div>
            Set forwarding number: (<i>leave blank to use default</i>):<br />
            <asp:TextBox ID="TxtForwardingNumber" MaxLength="12" runat="server" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TxtForwardingNumber" ValidationExpression="\d{3}-\d{3}-\d{4}" Text="* 555-555-5555" runat="server" />
            </div>
            
            <div>
                <asp:Button ID="BtnSave" OnClick="BtnSave_Click" Text="Activate Phone" runat="server" />
            </div>        
        </asp:View>
        <asp:View ID="ExistingView" runat="server">
            Active Phone Number: <asp:Label ID="LblActivePhone" runat="server" />&nbsp;
            <asp:LinkButton ID="BtnDeactivate" OnClick="BtnDeactivate_Click" OnClientClick="return confirm('Are you sure you want to deactivate this number? This will release this number for others to provision.');" Text="Deactivate Number" runat="server" />

            <div style="padding-top:10px;">
                You may include this campaign specific phone number on your pages by using the following markup tag:<br />
                <code style="padding:10px;">
                    {phone}
                </code>
                <br /><br />
                GooeyCMS automatically inserts the appropriate phone number based upon the active campaign. <br />
            </div>
        </asp:View>
        <asp:View ID="MaxAllowedView" runat="server">
            We're sorry, but you have reached your maximum number of allowed phone numbers.<br />
            To add another number you will first need to deactivate an existing number.
            <br /><br />
            For further assistance please contact customer support.
        </asp:View>
    </asp:MultiView>

    </div>
</asp:Content>
