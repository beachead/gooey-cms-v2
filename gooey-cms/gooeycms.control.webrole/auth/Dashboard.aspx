<%@ Page Title="" Language="C#" MasterPageFile="~/SecureNoNavigation.Master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Dashboard" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
    <script type="text/javascript" src="../../scripts/store.js"></script>
    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
    <style type="text/css">
    li.theme div.features {
        top: auto;
        bottom: 25px;
    }
    </style>
</asp:Content>
<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="dashboard" navItem="home" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <table border="0">
        <tr>
            <td>
              <div id="head-line" class="main">DASHBOARD</div>
            </td>
            <td>
            </td>
            <td>
              <div id="head-line" class="main">PURCHASES</div>
             </td>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; padding-left: 10px;">
                <br />
                Select a site to manage:<br />
                <asp:DropDownList ID="AvailableSites" runat="server" />
                &nbsp;
                <asp:Button ID="BtnManageSite" Text="Manage Site" OnClick="BtnManageSite_Click" runat="server" />
            </td>
            <td style="padding-left: 80px;">
            </td>
            <td>
                <ul id="themes-panel">
                    <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" OnItemCommand="SitePackages_OnItemCommand"
                        runat="server">
                        <ItemTemplate>
                            <li class="theme">
                                <!-- start: theme-header -->
                                <div class="theme-header">
                                    <div class="title">
                                        <%# Eval("Title") %></div>
                                    <div class="logo">
                                        <asp:Image ID="Logo" runat="server" /></div>
                                </div>
                                <!-- end: theme-header -->
                                <!-- start: preview -->
                                <div class="preview">
                                    <ul class="thumbs">
                                        <asp:Repeater ID="ThumbnailImages" OnItemCommand="SitePackages_OnItemCommand" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <img src='<%# DataBinder.Eval(Container.DataItem,"Url") %>' width="344" height="167"
                                                        alt="" />
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 10px;">
                                        <b>Apply To Site</b>:
                                        <asp:DropDownList ID="LstSites" runat="server" />
                                        &nbsp;<asp:Button ID="BtnApplyPackage" CommandName="ApplyPackage" CommandArgument='<%# Eval("Guid") %>'
                                            Text="Apply" runat="server" />
                                    </div>
                                    <ul class="thumb-nav">
                                    </ul>
                                    <div class="features">
                                        <a href="#" class="showFeatures">Features</a>
                                        <ul>
                                            <asp:Repeater ID="FeatureList" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <%# Container.DataItem %></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                                <!-- end: preview -->
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </td>
        </tr>
    </table>
    <telerik:RadWindowManager ID="Singleton" Skin="Windows7" Modal="true" Width="495"
        Height="260" ShowContentDuringLoad="false" DestroyOnClose="true" VisibleStatusbar="false"
        Behaviors="Move" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <script language="javascript" type="text/javascript">
        function showApplyPackage(siteGuid, packageGuid) {
            window.radopen('./Package-Mgmt/apply.aspx?s=' + siteGuid + '&g=' + packageGuid, null);
        }

        function confirmation(lstbox, type, packageGuid) {
            var box = document.getElementById(lstbox);
            if (box) {
                var siteGuid = box.options[box.selectedIndex].value;
                if (siteGuid == "") {
                    alert('You will now be redirected to the gooey store to create your new subscription. Afterwards you can then come back and apply this site.');
                    window.location = 'http://store.gooeycms.net/signup/';
                    return false;
                } else if (type == "Site") {
                    var result = confirm('WARNING: Applying this site will overwrite ALL of your existing data.\r\nAre you sure you want to continue?');
                    if (result) {
                        showApplyPackage(siteGuid, packageGuid);
                    }
                    return false;
                } else {
                }
            }
        }
    </script>
</asp:Content>
