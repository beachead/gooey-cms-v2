<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.Signup" %>
<asp:Content ContentPlaceHolderID="localCSS" runat="server">
	<link rel="stylesheet" type="text/css" href="/css/signup.css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
	<!-- START: content -->
	<div id="content">
		<h1><img src="../images/h1_minute_away2.png" width="720" height="28" alt="you're a minute away from a great relationship!" /></h1>
        
        <div style="padding-left:70px;">
        <asp:LoginView ID="LoginView" runat="server">
            <AnonymousTemplate>
            Are you an existing client? <asp:HyperLink ID="LnkSignIn" runat="server">Sign In Now</asp:HyperLink>.                
            </AnonymousTemplate>
            <LoggedInTemplate>
                You are currently logged in as <asp:LoginName runat="server" />&nbsp;<asp:LoginStatus ID="LoggedInStatus" LogoutText="Logout" runat="server" />
            </LoggedInTemplate>
        </asp:LoginView>
        </div>

		<div class="callout" id="callout">
			<p>Your 30 day trial lasts until <br /><% Response.Write(TrialExpires); %>. If you don’t want to continue using Gooey CMS, please cancel before <% Response.Write(TrialExpires); %>.</p>
			<p class="x-3">For additional details, <a href="">click here</a>.</p>
		</div>
 
		<!-- START: signup-form -->
		<ol id="signup-steps">
			<li>
				<h2><img src="../images/h2_create_gooey_acct.png" width="435" height="35" alt="create your gooey cms account" /></h2>
				<table cellspacing="0" class="form">
				<tr>
				<td class="label"><label for="first-name">First Name</label></td>
                <td>
                    <asp:TextBox ID="Firstname" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="FirstnameRequired" Text="*" ControlToValidate="Firstname" runat="server" />                    
                </td>				
				</tr>
				<tr>
				<td class="label"><label for="last-name">Last Name</label></td>
                <td>
                    <asp:TextBox ID="Lastname" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="LastnameRequired" Text="*" ControlToValidate="Lastname" runat="server" />                    
                </td>
				</tr>
				<tr>
				<td class="label"><label for="email">Email</label></td>
                <td>
                    <asp:TextBox ID="Email" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="EmailRequired" Text="*" ControlToValidate="Email" runat="server" />                    
                </td>
				</tr>
				<tr>
				<td class="label"><label for="company">Company</label></td>
                <td>
                    <asp:TextBox ID="Company" runat="server" />                   
                </td>
				</tr>
				</table>
			</li>
			<li>
				<h2><img src="../images/h2_create_password.png" width="435" height="35" alt="create your password" /></h2>
                <asp:Panel ID="PnlCreatePassword" runat="server">
				<table cellspacing="0" class="form">
				<tr>
				<td class="label"><label for="password">Password</label></td>
				<td><asp:TextBox ID="Password1" TextMode="Password" runat="server" /></td>
				</tr>
				<tr>
				<td class="label"><label for="confirm-password">Confirm Password</label></td>
				<td>                    
				    <asp:TextBox ID="Password2" TextMode="Password" runat="server" />
                    <asp:CompareValidator id="PasswordValidate" runat="server" ErrorMessage="Passwords do not match!" ControlToValidate="Password1" ControlToCompare="Password2"></asp:CompareValidator></td>
				</tr>
				</table>
                </asp:Panel>
                <asp:Panel ID="PnlNoPassword" runat="server">
                Uses your existing GooeyCMS password
                </asp:Panel>
			</li>
            <asp:ScriptManager ID="ScriptManager" runat="server" />       
			<li>
				<h2><img src="../images/h2_create_gooey_address.png" width="435" height="35" alt="create your cms address" /></h2>
				<p>Every Gooey CMS has its own web address.  You can change it to a<br />custom DNS address at any time (for example, MyWebsite.com).</p>
                
				<asp:UpdatePanel ID="UpdatePanelAvailable" runat="server">                
				<ContentTemplate>				
				<p>http://<asp:TextBox ID="Subdomain" OnTextChanged="Subdomain_TextChanged" CssClass="cms-address std clear-on-focus" runat="server" CausesValidation="true" Text="letters and numbers only" AutoPostBack="true" /><asp:Label ID="DefaultCmsDomain" runat="server" /> 
				    <span id="availability">
				            <asp:Image ID="IsAvailableImage" Visible="false" runat="server" />						    
				    </span>
				</p>
                </ContentTemplate>					
                </asp:UpdatePanel> 						
			</li>		
			<asp:Panel ID="OptionsPanel" Visible="true" runat="server">
			<li>
				<h2><img src="../images/h2_choose_options.png" width="435" height="35" alt="choose your options" /></h2>
				<p>We offer a few options for some customers who may need to<br />integrate Gooey CMS with their CRM or Analytics system.</p>
				<p>
                    <anthem:CheckBox ID="CampaignOption" AutoUpdateAfterCallBack="true" runat="server" />
					<label id="lbl-campaign-integration" class="option-item" for="salesforce-integration">campaign integration - $<asp:Label ID="CampaignOptionCost" runat="server" /> / month</label>
				</p>
				<p>
                    <anthem:CheckBox ID="SalesForceOption" OnCheckedChanged="SalesForceOption_Checked" AutoCallBack="true"  runat="server" />
					<label id="lbl-salesforce-integration" class="option-item" for="salesforce-integration">salesforce integration - $<asp:Label ID="SalesForceCost" runat="server" /> / month</label>
				</p>
				<p id="salesforce-disclaimer">Salesforce Integration allows you to send sales leads from any of your<br />forms directly into Salesforce (requires <a href="#">Salesforce Enterprise account</a>).</p>
			</li>
            </asp:Panel>			
            
			<li>
				<h2><img src="../images/h2_close_deal.png" width="435" height="35" alt="close the deal" /></h2>
				<p>
				    <asp:DropDownList ID="SelectedPlan" OnSelectedIndexChanged="SelectedPlan_Changed" AutoPostBack="true" runat="server"></asp:DropDownList>
				</p>
				<p><asp:ImageButton ID="Create" OnClick="CreateAccount_Click" ImageUrl="../images/btn_create_acct.png" runat="server" /></p>
				<p id="disclaimer">by clicking the button above, you agree to our <a href="">terms of service</a>, <a href="">privacy policy</a>, and <a href="">refund policy</a>.</p>
			</li>
		</ol>
 
		<!-- START: signup-form -->
 
	</div>
	<!-- END: content -->
</asp:Content>
