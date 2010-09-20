<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Site.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

<div>
Choose the site you would like to package for sale within the Gooey Store: <br />
<asp:DropDownList ID="LstAvailableSites" runat="server" />
</div>

<div>
Input a title for this site: <br />
<asp:TextBox ID="TxtTitle" runat="server" />
</div>

<div>
Input the price you would like to sell this site for: <br />
<asp:TextBox ID="TxtPrice" runat="server" />
</div>

<div>
Input 3-4 features of this site (one-per line): <br />
<asp:TextBox ID="TxtFeatures" TextMode=MultiLine Rows="5" Columns="50" runat="server" />
</div>

<div>
Choose a category for this site: <br />
<asp:DropDownList ID="LstCategory" runat="server" />
</div>

<div>
<asp:Button ID="BtnSave_Noajax" Text="Package (noajax)" OnClick="BtnSave_Click" runat="server" />
<button onclick="deploy_site">Deploy Site</button>

<anthem:Button ID="BtnSave" Text="Package" OnClick="BtnSave_Click" PreCallBackFunction="ajax_presave" PostCallBackFunction="deploy_site"  runat="server" />
<anthem:HiddenField ID="SavedPackageGuid" AutoUpdateAfterCallBack="true" runat="server" />
</div>

<div dojoType="dijit.Dialog" id="processing" title="Packaging Site..." style="display:none;" closable="false" draggable="false">
    <table>
        <tr>
            <td>
                Please be patient while we package your site for resale within the GooeyCMS Store. <br /><br />
                (This may take a few minutes. Do <b>NOT</b> refresh or leave this page)
            </td>
            <td>
              <img id="Img1" alt="Spinner" src="~/images/spinner.gif" runat="server" />
            </td>
        </tr>
    </table>
</div>

<div dojoType="dijit.Dialog" id="deploy-processing" title="Deploying Demo Site..." style="display:none;" closable="false" draggable="false">
    <table>
        <tr>
            <td>
                Please be patient while we create and deploy your demo site to GooeyCMS. <br /><br />
                (This may take a few minutes. Do <b>NOT</b> refresh or leave this page)
            </td>
            <td>
              <img id="Img2" alt="Spinner" src="~/images/spinner.gif" runat="server" />
            </td>
        </tr>
    </table>
</div>

<script language="javascript" type="text/javascript">
    function ajax_presave() {
        var div = dijit.byId('processing');
        div.show();
    }

    function deploy_site() {
        var old = dijit.byId('processing');
        old.hide();

        var div = dijit.byId('deploy-processing');
        div.show();

        Anthem_InvokePageMethod(
            'DoDeploySite',
            [document.getElementById('<%= SavedPackageGuid.ClientID %>').value],
            function (result) {
                alert('Your site was successfully packaged and deployed.');
                window.location = './default.aspx';
            }); 
    }
</script>

</asp:Content>
