<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Site.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Site" %>

<asp:Content ID="localJS" ContentPlaceHolderID="localJS" runat="server">
	<script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../scripts/store.js"></script>
</asp:Content>

<asp:Content ID="localCSS" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="Default.aspx">DEVELOPERS HOME</a></li>
            <li class="on">PACKAGE A NEW SITE</li>
            <li><a href="./theme.aspx?g=<%=System.Guid.NewGuid().ToString() %>">PACKAGE A NEW THEME</a></li> 
            <li class="last"><a href="Settings.aspx">DEVELOPER SETTINGS</a></li> 
        </ul>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

   <table cellspacing="0" class="form">
        <tr>
            <td valign="top">
<div>
   <p>Choose the site to package:<br />
   <asp:DropDownList ID="LstAvailableSites"  runat="server" /></p>
</div>

<div>
    <p>Site name:<br />
    <asp:TextBox ID="TxtTitle" MaxLength="18" runat="server" /></p>
</div>

<div>
   <p>Price:<br />
   <asp:TextBox ID="TxtPrice" MaxLength="9" runat="server" /></p>
</div>

<div>
    <p>List 3-4 features of this site (one-per line):<br />
    <asp:TextBox ID="TxtFeatures" TextMode=MultiLine Rows="5" Columns="40" runat="server" /></p>
</div>

<div>
    <p>Site category:<br />
    <asp:DropDownList ID="LstCategory" runat="server" /></p>
</div>

<div>
<anthem:Button ID="BtnSave" Text="Package &amp; Deploy" OnClick="BtnSave_Click" PostCallBackFunction="deploy_site"  runat="server" />
<anthem:HiddenField ID="SavedPackageGuid" AutoUpdateAfterCallBack="true" runat="server" />
</div>
      
            
            
            
            
            </td>
            <td valign="top">&nbsp;</td>
        <td width="100px;">&nbsp;</td>
 <td>
    <h2><img src="/images/steps_to_sell.png" width="272px" height="39px" border="0" /></h2>
    <div class="nice-box">
        <ol id="distribute-perks">
	        <li>Choose the site you want to package.</li>
	        <li>Name your site<br />(Examples: Real Estate Site, Preschool site).</li>
	        <li>Set a price (Examples: 450.00, 1000.00)</li>
	        <li>Enter a list of key features (click for example).</li>
	        <li>Choose a category that best descibes your site.</li>
	        <li>Choose a category that best descibes your site.</li>
	        <li>Click package & deploy and your site will be packaged up for review.</li>
	        <li>Once your site has been reviewed, you will receive an with requested or changes or it will be available on the store.</li>
        </ol>
    </div>
 </td>       </tr>
    </table>






<script language="javascript" type="text/javascript">
    function deploy_site() {
        window.open('./dopackage.aspx?g=' + document.getElementById('<%=SavedPackageGuid.ClientID %>').value, '', 'top=40,left=200,width=450,height=115,scrollbars=no');
        window.location = './default.aspx';
    }
</script>

</asp:Content>
