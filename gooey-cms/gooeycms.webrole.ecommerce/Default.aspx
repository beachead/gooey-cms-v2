<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.store.Default" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="localCSS" ContentPlaceHolderID="localCSS" runat="server">
	<link rel="stylesheet" type="text/css" href="/css/store.css" />
    <style type="text/css">
    #masthead {
	    background-image: url(../images/bg_masthead_no-line.png);
    }
    </style>
</asp:Content>

<asp:Content id="localJS" ContentPlaceHolderID="localJS" runat="server">
    <script type="text/javascript" src="/js/store.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" />

	<!-- START: content -->
	<div id="content" class="store">
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel" Skin="Windows7" runat="server" />
        <telerik:RadAjaxPanel ID="AjaxPanel" LoadingPanelID="LoadingPanel" runat="server">
        <anthem:HiddenField ID="LastMaxPos" runat="server" />
		<h1><img src="../images/h1_are_you_a_designer.png" width="598" height="21" alt="Are you a designer?  Learn how to make money with Gooey CMS on our" /><a href=""><img src="/images/h1[a]_developer_site.png" width="122" height="21" alt="developer site" /></a></h1>

		<!-- START: filter -->
		<ul id="filter">
			<li>Show all</li>
			<li id="theme-filter"><a href="">Sites</a>
				<div class="menu">
					<ul>

						<li><a href="">sites and themes</a></li>
						<li><a href="">sites</a></li>
						<li><a href="">themes</a></li>
					</ul>
				</div>
			</li>
			<li>for</li>

			<li id="price-filter"><a href="">Any Price</a>
				<div class="menu">
                    <ul>
                        <li><asp:LinkButton ID="LnkButton1" OnCommand="LnkFilterPrice_Click" CommandArgument="500" Text="under $500" runat="server" /></li>
                        <li><asp:LinkButton ID="LnkButton2" OnCommand="LnkFilterPrice_Click" CommandArgument="500,1000" Text="$500 - $1,000" runat="server" /></li>
                        <li><asp:LinkButton ID="LnkButton3" OnCommand="LnkFilterPrice_Click" CommandArgument="1000,5000" Text="$1,000 - $5,000" runat="server" /></li>
                        <li><asp:LinkButton ID="LnkButton4" OnCommand="LnkFilterPrice_Click" CommandArgument="5000,10000" Text="$5,000 - $10,000" runat="server" /></li>
                    </ul>
                </div>
			</li>
		</ul>
		<!-- END: filter -->
		
		<div>
		<!-- START: themes -->
		<ul id="themes-panel">
            <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" runat="server">
                <ItemTemplate>
   			        <li class="theme">  
                        <div class="theme-header">
				            <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c}") %></div>
				            <div class="logo"><asp:Image ID="LogoSrc" Width="115" Height="60" runat="server" /></div>
				            <ul class="options-links">
                                <li>view <asp:HyperLink ID="DemoLink" runat="server">live</asp:HyperLink> or <asp:HyperLink ID="AdminDemoLink" runat="server">admin</asp:HyperLink> demo site</li>
                                <li><a href='./purchase.aspx?g=<%# Eval("Guid") %>'>purchase site</a></li>
                            </ul>
                        </div>
                        <!-- start: preview -->
                        <div class="preview">
                            <ul class="thumbs">
                                    <asp:Repeater ID="ThumbnailImages" runat="server">
                                        <ItemTemplate>
                                            <img src='<%# DataBinder.Eval(Container.DataItem,"Url") %>' width="344" height="167" alt="" />
                                        </ItemTemplate>
                                    </asp:Repeater>
				            </ul>

				            <ul class="thumb-nav"></ul>
				        
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
		<!-- END: themes -->
		</div>
        </telerik:RadAjaxPanel>
	</div>
	<!-- END: content -->

</asp:Content>
