<%@ Page Title="" Language="C#" MasterPageFile="~/SecureNoNavigation.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../scripts/store.js"></script>

    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="http://store.gooeycms.net/signup/">REGISTER NEW SITE</a></li>
            <li><a href="http://store.gooeycms.net/">PURCHASE SITES OR THEMES</a></li> 
        </ul>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

    <table border=0>
    <tr><td><img src="../images/dashboard.png" width="362px" height="45px" border="0" /></td><td style="padding-left:80px;"></td><td> <img src="../images/purchases.png" width="201px" height="61px" border="0" /></td></tr>
    <tr><td colspan="3"><hr /></td></tr>
    <tr>        <td style="vertical-align:top; padding-left:10px;">
                <br />Select a site to manage:<br /><asp:DropDownList ID="AvailableSites" runat="server" />&nbsp;
                <asp:Button ID="BtnManageSite" Text="Manage Site" OnClick="BtnManageSite_Click" runat="server" />                
            </td>
            <td style="padding-left:80px;"></td>
            <td>
		    <ul id="themes-panel">
                <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" OnItemCommand="SitePackages_OnItemCommand" runat="server">
                   <ItemTemplate>
    	            <li class="theme">    
				            <div class="title" style="margin-top:0px;">
                                <%# Eval("Title") %>
                            </div>
				            <ul class="thumbs" style="padding-left:0px;">
                                <asp:Repeater ID="ThumbnailImages" runat="server">
                                    <ItemTemplate>
                                        <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                    </ItemTemplate>
                                </asp:Repeater>
				            </ul>                    
                            <div style="padding-left:20px;padding-top:5px;padding-bottom:10px;">
                                <b>Apply To Site</b>: <asp:DropDownList ID="LstSites" runat="server" />&nbsp;<asp:Button ID="BtnApplyPackage" Text="Apply" runat="server" />
                            </div>
				            <ul class="thumb-nav"></ul>
				            <ul class="features" style="margin:0px;">
                                <asp:Repeater ID="FeatureList" runat="server">
                                    <ItemTemplate>
                                        <li><%# Container.DataItem %></li>
                                    </ItemTemplate>
                                </asp:Repeater>
				            </ul>
			            </li>                                        
                    </ItemTemplate>
                </asp:Repeater>
            </ul>                             
            </td>
        </tr>
    
    </table>
</asp:Content>
