﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SecureNoNavigation.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Default" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="localJS" ContentPlaceHolderID="localJS" runat="server">
	<script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../scripts/store.js"></script>
</asp:Content>
<asp:Content id="localCSS" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
</asp:Content>


<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="developer" navItem="home" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
    <div style="overflow: auto;">
        <table border="0">
            <tr>
                <td style="vertical-align:top; padding: 20px 20px 0 0">
                    <div id="head-line" class="main">BUILD YOUR BUSINESS ONE WEBSITE AT A TIME</div>
                    <p>create new websites to sell and keep 70% of each sale<br />add a logo or mulitple screenshots to your site package<br />view or edit the site package you created</p>                
                </td>
                <td style="vertical-align:top; padding: 45px 20px 0 0">
                    <ul class="check">
	                    <li class="alt">You pick the price</li>
	                    <li>You get 70% of every sale</li>
	                    <li class="alt">Receive revenue checks quarterly</li>
	                    <li>No charge for free sites</li>
	                    <li class="alt">No credit card fees</li>
	                    <li>No hosting fees in store</li>
                    </ul>                
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
		    <ul id="themes-panel">
                <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" OnItemCommand="SitePackages_OnItemCommand" runat="server">
                    <ItemTemplate>
			            <li class="theme">    

                            <!-- start: theme-header -->
                            <div class="theme-header">
				                <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c0}") %></div>
				                <div class="logo"><asp:Image ID="Logo" runat="server" /></div>
				                <ul class="options-links">
                                    <li><a href='./Edit.aspx?e=<%# Eval("Guid") %>'>Edit</a></li>
                                    <li><asp:LinkButton ID="BtnDelete" OnClientClick="return confirm('Are you sure you want to delete this site package? \n\n WARNING: This will immediately remove your site from the GooeyCMS Store.');" CommandName="DeletePackage" CommandArgument='<%# Eval("Guid") %>' runat="server">Delete</asp:LinkButton></li>
                                </ul>
                            </div>
                            <!-- end: theme-header -->

                            <!-- start: preview -->
                            <div class="preview">
                                <ul class="thumbs">
                                    <asp:Repeater ID="ThumbnailImages" OnItemCommand="SitePackages_OnItemCommand" runat="server">
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton OnClientClick="return confirm('Are you sure you want to delete this screenshot?');" CssClass="delete-screenshot" CommandName="DeleteScreenshot" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"PackageGuid") + "|" + DataBinder.Eval(Container.DataItem,"Name") %>' runat="server"></asp:LinkButton>
                                                <img src='<%# DataBinder.Eval(Container.DataItem,"Url") %>' width="344" height="167" alt="" />
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
				                </ul>
                                <ul class="thumb-nav"></ul>
				        
				                <div class="theme-add-screenshot">
                                    <asp:FileUpload ID="FileUpload" runat="server" />&nbsp;<asp:Button ID="BtnAddScreenshot" CommandName="AddScreenshot" CommandArgument='<%# Eval("Guid") %>' Text="add screenshot" runat="server" />
                                </div>
                               
				                <div class="features">
                                    <a href="#" class="showFeatures">Features</a>
				                    <ul>
                                        <asp:Repeater ID="FeatureList" runat="server">
                                            <ItemTemplate>
                                                <li><%# Container.DataItem %></li>
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
</asp:Content>
