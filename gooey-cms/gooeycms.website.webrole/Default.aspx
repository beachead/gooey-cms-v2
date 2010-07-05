<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="gooeycms.webrole.website.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
 
<head>
	<title>gooey cms</title>
	<link rel="stylesheet" type="text/css" href="css/global.css" />
	<link rel="stylesheet" type="text/css" href="css/home.css" />
	<script type="text/javascript" src="js/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="js/global.js"></script>
</head>
 
<body>
 
<!-- START: main -->
<div id="main">
 
	<!-- START: masthead -->
	<div id="masthead">
		<div id="login"><a href="login.html"><img src="images/btn_login.png" width="104" height="28" alt="" /></a></div>
		<div id="logo"><a href="index.html"><img src="images/logo_gooey_internal.png" width="225" height="75" alt="" /></a></div>
		<div id="nav">
			<ul class="adjacent rollover">
				<li><a href="index.html"><img src="images/nav_home.gif" width="56" height="22" alt="Home" /></a></li>
				<li><a href="tour.html"><img src="images/nav_tour.gif" width="53" height="22" alt="Feature Tour" /></a></li>
				<li><a href="store.html"><img src="images/nav_store.gif" width="61" height="22" alt="Store" class="active" /></a></li>
				<li><a href="pricing.html"><img src="images/nav_pricing.gif" width="72" height="22" alt="Pricing" /></a></li>
				<li><a href="help.html"><img src="images/nav_help.gif" width="43" height="22" alt="Help" /></a></li>
				<li id="signup"><a href="signup.html"><img src="images/nav_signup.gif" width="50" height="30" alt="Signup" /></a></li>
			</ul>
		</div>
	</div>
	<!-- END: masthead -->
 
    <form id="form1" runat="server">
	<!-- START: content -->
	<div id="content">
		<p><img src="images/home/txt_fastest_way.png" width="298" height="142" alt="The Fastest Way to Manage Content On The Web, Hands Down" /></p>
		<p><img src="images/home/txt_starting_at_50.png" width="298" height="20" alt="Starting at $50 Per Month" /></p>
		<p id="feature-tour"><a href=""><img src="images/btn_feature_tour.png" width="137" height="39" alt="Feature Tour" /></a></p>
		<div id="handy">
			<p><img src="images/txt_hey_designers.png" width="571" height="24" alt="Hey Designers! Interested in making money building sites?" /></p>
			<p><img src="images/txt_increasing_revenue.png" width="571" height="33" alt="Increasing revenue is easy with gooey cms" /></p>
		</div>
	</div>
	<!-- END: content -->
 
	<!-- START: features -->
	<div id="features">
 
		<div class="column" id="col-speed">
			<h2><img src="images/home/h2_built_for_speed.png" width="198" height="32" alt="Built For Speed"/></h2>
			<p>See why creating content with Gooey is easier and faster than any other CMS. Isn't your time worth something?</p>
			<p class="learn-more"><a href=""><img src="images/home/txt_learn_more.png" width="86" height="14" alt="" /></a></p>
		</div>
 
		<div class="column" id="col-features">
			<h2><img src="images/home/h2_over_200_features.png" width="238" height="32" alt="Over 200 Features"/></h2>
			<p>Gooey provides all of the items you would expect in a CMS and quite a few pleasant surprises.</p>
			<p class="learn-more"><a href=""><img src="images/home/txt_learn_more.png" width="86" height="14" alt="" /></a></p>
		</div>
 
		<div class="column" id="col-sites">
			<h2><img src="images/home/h2_over_200_sites.png" width="185" height="32" alt="Over 200 Sites"/></h2>
			<p>Imagine being able to create a site with just a click. Browse our store and then purchase a complete site for your business out of the box.</p>
			<p class="learn-more"><a href=""><img src="images/home/txt_learn_more.png" width="86" height="14" alt="" /></a></p>
		</div>
 
	</div>
	<!-- END: features -->
    </form>
 
	<!-- START: footer -->
	<div id="footer">
		<ul class="adjacent">
			<li><a href="">About</a></li>
			<li><a href="">Contact Us</a></li>
			<li><a href="">Terms &amp; Conditions</a></li>
			<li class="last"><a href="">Help</a></li>
		</ul>
		<p>&copy; 2010 Beachead Technologies Inc.</p>
	</div>
	<!-- END: footer -->
 
</div>
<!-- END: main -->
 
</body>
 
</html>
