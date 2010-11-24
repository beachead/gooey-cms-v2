<%@ Page Title="" Language="C#" MasterPageFile="~/SecureNoNavigation.Master" AutoEventWireup="true" CodeBehind="Site.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Site" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="localJS" ContentPlaceHolderID="localJS" runat="server">
    <script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
    <script type="text/javascript" src="../../scripts/store.js"></script>
    <script type="text/javascript" type="text/javascript">
        dojo.require("dojox.timing._base");
    </script>
</asp:Content>
<asp:Content ID="localCSS" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
</asp:Content>

<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="developer" navItem="new" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>

    <table cellspacing="0" class="form" style="width:90%;">
        <tr>
            <td style="vertical-align:top;width:40%;">
                <div>
                    <p>
                        Choose the site to package:<br />
                        <asp:DropDownList ID="LstAvailableSites" runat="server" />
                    </p>
                </div>
                <div>
                    <p>
                        Site name:<br />
                        <asp:TextBox ID="TxtTitle" MaxLength="18" runat="server" /></p>
                </div>
                <div>
                    <p>
                        Price:<br />
                        <asp:TextBox ID="TxtPrice" MaxLength="9" runat="server" /></p>
                </div>
                <div>
                    <p>
                        List 3-4 features of this site (one-per line):<br />
                        <asp:TextBox ID="TxtFeatures" TextMode="MultiLine" Rows="5" Columns="40" runat="server" /></p>
                </div>
                <div>
                    <p>
                        Site category:<br />
                        <asp:DropDownList ID="LstCategory" runat="server" />
                    </p>
                </div>
                <div>
                    <table>
                        <tr>
                            <td style="vertical-align:top;"><asp:CheckBox ID="ChkAgreeTos" runat="server" /></td>
                            <td><asp:Label ID="ChkTosText" runat="server" /></td>
                        </tr>
                    </table>
                    <anthem:Button ID="BtnSave" Text="Package &amp; Deploy" OnClick="BtnSave_Click" PreCallBackFunction="validate_site" PostCallBackFunction="deploy_site" runat="server" />
                    <anthem:HiddenField ID="SavedPackageGuid" AutoUpdateAfterCallBack="true" runat="server" />
                </div>
            </td>
            <td valign="top">
                &nbsp;
            </td>
            <td width="100px;">
                &nbsp;
            </td>
            <td style="vertical-align:top;">
                <h2>
                    <img src="/images/steps_to_sell.png" width="272px" height="39px" border="0" /></h2>
                <div class="nice-box">
                    <ol id="distribute-perks">
                        <li>Choose the site you want to package.</li>
                        <li>Name your site<br />
                            (Examples: Real Estate Site, Preschool site).</li>
                        <li>Set a price (Examples: 450.00, 1000.00)</li>
                        <li>Enter a list of key features (click for example).</li>
                        <li>Choose a category that best descibes your site.</li>
                        <li>Choose a category that best descibes your site.</li>
                        <li>Click package & deploy and your site will be packaged up for review.</li>
                        <li>Once your site has been reviewed, you will receive an with requested or changes
                            or it will be available on the store.</li>
                    </ol>
                </div>
            </td>
        </tr>
    </table>

    <telerik:RadWindowManager ID="Singleton" Skin="Windows7" Modal="true" Width="463" Height="237" ShowContentDuringLoad="false" DestroyOnClose="true" Opacity="100" VisibleStatusbar="false" Behaviors="Move" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <script language="javascript" type="text/javascript">
        function validate_site() {
            var box = document.getElementById('<%= ChkAgreeTos.ClientID %>');
            var result = box.checked;
            if (!result)
                alert('You must agree to the terms of service prior to packaging this site.');

            return result;
        }

        function deploy_site() {
            window.radopen('./dopackage.aspx?g=' + document.getElementById('<%=SavedPackageGuid.ClientID %>').value, null);
        }

        function redirect_to_default() {
            alert('Successfully Created Package!');
            window.location = './default.aspx';
        }
</script>
</asp:Content>
