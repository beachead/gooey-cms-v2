<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.store.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
	<!-- START: content -->
	<div id="content">
        
        <anthem:HiddenField ID="LastMaxPos" runat="server" />
		<h1><img src="../images/h1_are_you_a_designer.png" width="598" height="21" alt="Are you a designer?  Learn how to make money with Gooey CMS on our" /><a href=""><img src="/images/h1[a]_developer_site.png" width="122" height="21" alt="developer site" /></a></h1>

		<!-- START: filter -->
		<ul id="filter">
			<li>Show all</li>
			<li id="theme-filter"><a href="">Sites and Themes</a>
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
				<div class="menu"></div>
			</li>
		</ul>
		<!-- END: filter -->
		
		<div>
		<!-- START: themes -->
		<ul id="themes-panel">
            <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" runat="server">
                <ItemTemplate>
   			        <li class="theme">  
				        <div class="title">School Site - $1,000</div>
				        <div class="logo"><img src="../images/___placeholder_logo.png" width="93" height="27" alt="" /></div>
				        <div class="prev"><a href=""><img src="../images/btn_prev.png" width="29" height="32" alt="" /></a></div>
				        <div class="next"><a href=""><img src="../images/btn_next.png" width="29" height="32" alt="" /></a></div>
				        <ul class="thumbs">
                                <asp:Repeater ID="ThumbnailImages" runat="server">
                                    <ItemTemplate>
                                        <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                    </ItemTemplate>
                                </asp:Repeater>
				        </ul>
				        <ul class="thumb-nav"></ul>
				        <a class="purchase"><img src="../images/btn_purchase.png" width="137" height="39" alt="" /></a>
				        <asp:HyperLink ID="DemoLink" CssClass="demo" runat="server"><img src="../images/btn_purchase.png" width="137" height="39" alt="View Demo" tooltip="View Demo"  /></asp:HyperLink>
				        <ul class="features">
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
		<!-- END: themes -->
		</div>
	</div>
	<!-- END: content -->

</asp:Content>
