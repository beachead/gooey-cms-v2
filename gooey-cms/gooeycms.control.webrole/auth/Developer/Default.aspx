<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Default" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<%@ Import Namespace="Gooeycms.Business" %>

<asp:Content ID="localJS" ContentPlaceHolderID="localJS" runat="server">
	<script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../scripts/store.js"></script>
</asp:Content>
<asp:Content id="localCSS" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
    <style type="text/css">
    #themes-panel {
        padding-left: 0;
        margin-left: 0;
    }
    
    </style>
</asp:Content>


<asp:Content ID="subnav" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" navSection="developer" navItem="home" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
  
       <h1>DEVELOPERS HOME</h1>
       <p>Create new websites to sell and keep 70% of each sale. Click <a href="site.aspx">package a new site</a> to begin.<br />
       You can add a logo to be used across all the websites you're selling under <a href="settings.aspx">Developer Settings</a>.</p>
       <br /><br />

		    <ul id="themes-panel">
                <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" OnItemCommand="SitePackages_OnItemCommand" runat="server">
                    <ItemTemplate>
			            <li class="theme">    

                            <!-- start: theme-header -->
                            <div class="theme-header">
				                <div class="title">
                                    <%# Eval("Title") %><br />
                                    <%# DataBinder.Eval(Container.DataItem,"Price","{0:c0}") %>
                                </div>
				                <div class="logo"><asp:Image ID="Logo" runat="server" /></div>
				                
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
                                
                                <ul class="options-links">
                                    <li><a href="#" onclick="embed_display('<%# DataBinder.Eval(Container.DataItem,"Guid") %>'); return false;">Embed</a></li>
                                    <li><a href="<%# Eval("Guid") %>" class="addScreenshot">Add Screenshot</a></li>
                                    <li><a href='./Edit.aspx?e=<%# Eval("Guid") %>'>Edit</a></li>
                                    <li><asp:LinkButton ID="BtnDelete" OnClientClick="return confirm('Are you sure you want to delete this site package? \n\n WARNING: This will immediately remove your site from the GooeyCMS Store.');" CommandName="DeletePackage" CommandArgument='<%# Eval("Guid") %>' runat="server">Delete</asp:LinkButton></li>
                                </ul>
                            </div>
                            <!-- end: theme-header -->

				            <div class="theme-add-screenshot" id="ss_<%# Eval("Guid") %>">
                                <asp:FileUpload ID="FileUpload" runat="server" />&nbsp;<asp:Button ID="BtnAddScreenshot" CommandName="AddScreenshot" CommandArgument='<%# Eval("Guid") %>' Text="Add" runat="server" />
                                <a href="#" class="ss_cancel">Cancel</a>
                            </div>

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
				        
                            </div>
                            <!-- end: preview -->

			            </li>                                        
                    </ItemTemplate>
                </asp:Repeater>
            </ul>             
            
            <telerik:RadWindow ID="EmbedWindow" Width="600" Height="200" Modal="true" Behaviors="Close" runat="server">
                <ContentTemplate>
                    Copy-and-paste the below code to embed this site package into your website<br /><br />
                    <fieldset>
                        <legend>Code</legend>
                        <div style="width:560px;overflow:auto;">
                        <xmp id="lblembed"></xmp>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </telerik:RadWindow>
            
            <script type="text/javascript" language="javascript">
                function embed_display(guid) {
                    var lbl = $get('lblembed');
                    lbl.innerHTML = "<iframe src=\"<% Response.Write(GooeyConfigManager.StoreSiteHost); %>/embedded.aspx?g=" + guid + "\" width=\"400\" height=\"350\" frameborder=\"0\" />";

                    var wnd = $find('<%= EmbedWindow.ClientID %>');
                    wnd.show();
                }
            </script>    
</asp:Content>
