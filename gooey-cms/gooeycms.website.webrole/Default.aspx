<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="gooeycms.webrole.website.Default1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceholder" runat="server">
    <!-- START: main -->
<div id="main">
	<!-- START: masthead -->
	<div id="mastheadlogo">
		<!-- START: nav -->
		<div id="nav">
			<ul>
				<li class="active"><a href="/">Home</a></li>
				<li><a href="/tour">Tour</a></li>
				<li><a href="/pricing">Store</a></li>
				<li><a href="/pricing">Plans & Pricing</a></li>
				<li><a href="/help">Help</a></li>
				<li><a href="/signup">Login</a></li>
			</ul>
		</div>
		<!-- END: nav -->
	</div>

	<!-- START: content-wrapper -->
	<div id="content-wrapper">

		<!-- START: content -->
		<div id="page-content">

			<p><IMG SRC="images/best_way.png" WIDTH="424" HEIGHT="118" BORDER="0" ALT="The fastest way to manage content on the web. Hands down."></p>
			<h3>Starting at $50 per month.</h3>

			<div class="button">
				<a href="/tour" title="expanded content will go here for easy access">Sample Button</a>
			</div>

		</div>
		<!-- END: content -->


		<!-- START: resources -->
		<div id="resources">
			<p>gooeycms has <a href="" id="a-features">features</a></p>
			<img src="images/screenshot_placeholder.png" width="594" height="400" border="0" alt=""/>
		</div>
		<!-- END: resources -->

	</div>
	<!-- END: content-wrapper -->
</div>
<!-- END: main -->

<!-- START: main -->
<div id="features">
	<ul>
		<li><a href="#">Feature 1</a></li>
		<li><a href="#">Feature 2</a></li>
		<li><a href="#">Feature 3</a></li>
		<li><a href="#">Feature 4</a></li>
		<li><a href="#">Feature 5</a></li>
		<li><a href="#">Feature 6</a></li>
	</ul>
</div>
<!-- END: main -->
</asp:Content>
