<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="gooeycms.webrole.ecommerce.Default1" %>


<asp:Content ID="BodyPlaceHolder" ContentPlaceHolderID="BodyPlaceholder" runat="server">
 
	
	
  <div>
        <p>1. Create your Gooey CMS Account</p>
        <table>
            <tr>
                <td>First Name</td>
                <td>

                </td>
            </tr>
            <tr>
                <td>Last Name</td>
                <td>
                    <asp:TextBox ID="Lastname" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="LastnameRequired" Text="*" ControlToValidate="Lastname" runat="server" />                    
                </td>
            </tr>
            <tr>
                <td>Email</td>
                <td>
                    <asp:TextBox ID="Email" runat="server" onchange="setUsername(this);" />&nbsp;
                    <asp:RequiredFieldValidator ID="EmailRequired" Text="*" ControlToValidate="Email" runat="server" />                    
                </td>
            </tr>
            <tr>
                <td>Company</td>
                <td>
                    <asp:TextBox ID="Company" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="CompanyRequired" Text="*" ControlToValidate="Company" runat="server" />                    
                </td>
            </tr>
        </table>
    </div>
    
    <div>
        <p>2. Choose a Password</p>
        <table>
            <tr>
                <td>
                    Username<br />
                    <asp:TextBox ID="Username" ReadOnly="true" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Password<br />
                    <asp:TextBox ID="Password1" TextMode="Password" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Confirm Password<br />
                    <asp:TextBox ID="Password2" TextMode="Password" runat="server" />
                    <asp:CompareValidator id="PasswordValidate" runat="server" ErrorMessage="Passwords do not match!" ControlToValidate="Password1" ControlToCompare="Password2"></asp:CompareValidator>                    
                </td>
            </tr>                        
        </table>
    </div>

    <div>
        <asp:ScriptManager ID="ScriptManager" runat="server" />
        <p>3. Create your Gooey CMS account address</p>
        
        <asp:UpdatePanel ID="UpdatePanelAvailable" runat="server">
            <ContentTemplate>
                http://<asp:TextBox ID="Subdomain" OnTextChanged="Subdomain_TextChanged"  runat="server" CausesValidation="true" AutoPostBack="true" />.gooeycms.net            
                <asp:Label ID="LblAvailable" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>        
    </div>
    
    <div>
        <p>4. Choose your options</p>
        <p>
            We offer a few options for some customers who may need to integrate Gooey CMS with their CRM or Analytics system.
        </p>
        
        <table>
            <tr>
                <td><asp:CheckBox ID="SalesForceOption" runat="server" /></td>
                <td>Salesforce Integration - $250 / month<br />
                Description of salesforce integration
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="CreateAccount" OnClick="CreateAccount_Click" Text="Next" runat="server" />
                </td>
            </tr>
        </table>
    </div>	
</asp:Content>
