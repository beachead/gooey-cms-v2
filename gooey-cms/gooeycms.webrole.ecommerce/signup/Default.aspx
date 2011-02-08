<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.Signup" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" type="text/css" href="/css/signup.css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />

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

        <div style="">
            <br />
            <asp:Label ID="LblStatus" runat="server" />
        </div>
		<div class="callout" id="callout">
			<p style="margin-bottom:10px;">Try Gooey CMS without a credit card, sign&nbsp;up for our <a href="Default.aspx?free">FREE</a> program.</p><p class="x-3" style="padding-left:25px; margin-bottom:10px;">You can upgrade to our Pro Plan anytime for FREE during your 30&nbsp;day trial which expires on <% Response.Write(TrialExpires); %>.</p>
			<p class="x-3" style="padding-left:25px;">You can cancel anytime, but we hope you'll stay.</p>
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
                    <asp:RequiredFieldValidator ID="FirstnameRequired" Text="*" ControlToValidate="Firstname" ValidationGroup="MainGroup" runat="server" />                    
                </td>				
				</tr>
				<tr>
				<td class="label"><label for="last-name">Last Name</label></td>
                <td>
                    <asp:TextBox ID="Lastname" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="LastnameRequired" Text="*" ControlToValidate="Lastname" ValidationGroup="MainGroup" runat="server" />                    
                </td>
				</tr>
				<tr>
				<td class="label"><label for="email">Email</label></td>
                <td>
                    <asp:TextBox ID="Email" runat="server" />&nbsp;
                    <asp:RequiredFieldValidator ID="EmailRequired" Text="*" ControlToValidate="Email" ValidationGroup="MainGroup" runat="server" />                    
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
				<td><asp:TextBox ID="Password1" TextMode="Password" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="* Required" ControlToValidate="Password1" ValidationGroup="MainGroup" runat="server" />
                </td>
				</tr>
				<tr>
				<td class="label"><label for="confirm-password">Confirm Password</label></td>
				<td>                    
				    <asp:TextBox ID="Password2" TextMode="Password" runat="server" />
                    <asp:RequiredFieldValidator ID="PasswordRequired" ErrorMessage="* Required" ControlToValidate="Password2" ValidationGroup="MainGroup" runat="server" />
                    <asp:CompareValidator id="PasswordValidate" runat="server" ErrorMessage="Passwords do not match!" ControlToValidate="Password2" ControlToCompare="Password1"></asp:CompareValidator></td>
               </tr>
				</table>
                </asp:Panel>
                <asp:Panel ID="PnlNoPassword" runat="server">
                Uses your existing GooeyCMS password
                </asp:Panel>
			</li> 
			<li>
				<h2><img src="../images/h2_create_gooey_address.png" width="435" height="35" alt="create your cms address" /></h2>
				<p>Every Gooey CMS website has its own web address. You can change<br />to a custom DNS address at any time (for example, MyWebsite.com).</p>
                
				<asp:UpdatePanel ID="UpdatePanelAvailable" runat="server">                
				<ContentTemplate>				
				<p>http://<asp:TextBox ID="Subdomain" OnTextChanged="Subdomain_TextChanged" CssClass="cms-address std clear-on-focus" runat="server" CausesValidation="false" Text="letters and numbers only" AutoPostBack="true" /><asp:Label ID="DefaultCmsDomain" runat="server" /> 
				    <span id="availability">
				            <asp:Image ID="IsAvailableImage" Visible="false" runat="server" />						    
				    </span>
				</p>
                </ContentTemplate>					
                </asp:UpdatePanel> 						
			</li>		
			<li style="padding-bottom:0px;">
				<h2><img src="../images/h2_choose_options.png" width="435" height="35" alt="choose your options" /></h2>
				<p>Add the power of an integrated demand-gen system and see how easy it is to increase your website's revenue.</p>
				<p>
                    <anthem:CheckBox ID="CampaignOption" AutoUpdateAfterCallBack="true" runat="server" />
					<label id="lbl-campaign-integration" class="option-item" for="salesforce-integration">Campaign integration - $<asp:Label ID="CampaignOptionCost" runat="server" /> / month</label>
				<div id="salesforce-disclaimer">Campaign integration allows you to track your customers offline and online activity.<br />Imagine understanding just how many phone calls your company received and whitepapers were downloaded from your latest email blast or PPC campaign.</div>
				</p>
            </li>
			<asp:Panel ID="OptionsPanel" Visible="true" runat="server">
			<li>
            	<p>
                    <anthem:CheckBox ID="SalesForceOption" OnCheckedChanged="SalesForceOption_Checked" AutoCallBack="true"  runat="server" />
					<label id="lbl-salesforce-integration" class="option-item" for="salesforce-integration">Salesforce integration - $<asp:Label ID="SalesForceCost" runat="server" /> / month</label>
				<div id="salesforce-disclaimer">Salesforce integration allows you to send sales leads from any of your forms directly into Salesforce<br />(requires <a href="http://www.salesforce.com/crm/editions-pricing.jsp">Salesforce Professional with API, Enterprise or Unlimited account</a>).<br />
                Setup takes minutes, as Salesforce fields (including custom fields) are supported with no programming.</div>
				</p>
			</li>
            </asp:Panel>			
            
			<li>
				<h2><img src="../images/h2_close_deal.png" width="435" height="35" alt="close the deal" /></h2>
				<p>
				    <asp:DropDownList ID="SelectedPlan" OnSelectedIndexChanged="SelectedPlan_Changed" AutoPostBack="true" runat="server"></asp:DropDownList>
				</p>
				<p id="disclaimer" style="color:#000000;">by clicking the button below, you agree to our <a href="http://www.gooeycms.com/terms_of_use">terms of service</a>, <a href="http://www.gooeycms.com/privacy">privacy policy</a>, and <a href="http://www.gooeycms.com/help#refund">refund policy</a>.</p>
				<p><asp:ImageButton ID="Create" OnClick="CreateAccount_Click" ValidationGroup="MainGroup" OnClientClick="" ImageUrl="../images/btn_create_acct.png" runat="server" /></p>
			</li>
		</ol>
 
		<!-- START: signup-form -->
        <telerik:RadWindowManager ID="WindowManager" runat="server" />
        <telerik:RadWindow ID="Window" Title="Invite Code" Width="600" Height="350" Modal="true" VisibleOnPageLoad="false" Behaviors="Resize,Move,Close" Skin="Default" runat="server">
            <ContentTemplate>
                <div style="padding:10px;">
                    During our private launch an invite code is required to sign-up for GooeyCMS.<br />If you do not have an invite code you may request one <a href="http://invite.gooeycms.com">here</a>.
                    <br /><br />
                    <b>Invite Code:</b><br />
                    <asp:TextBox ID="InviteCode" Width="500px" TextMode="MultiLine" Rows="3" runat="server" ValidationGroup="InviteGroup" />
                    <asp:RequiredFieldValidator ID="InviteRequired" runat="server" ControlToValidate="InviteCode" Display="None" />
                    <p><asp:ImageButton ID="BtnCreateAccount" OnClick="CreateAccount_Click" ImageUrl="../images/btn_create_acct.png" runat="server" /></p>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>

        <script language="javascript" type="text/javascript">
            function get_invite_code() {
                var result = Page_ClientValidate("MainGroup");
                if (result) {
                    var rad = $find('<%= Window.ClientID %>');
                    rad.show();
                } 
            }
        </script>
	</div>
	<!-- END: content -->
</asp:Content>
