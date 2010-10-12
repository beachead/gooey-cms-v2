<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="gooeycms.webrole.betasignup._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>gooey cms: private launch invite</title>
	<link rel="stylesheet" type="text/css" href="css/global.css" />
	<link rel="stylesheet" type="text/css" href="css/invite.css" />
	<script type="text/javascript" src="js/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="js/global2.js"></script>
</head>
<body>
    <form id="form1" action="default.aspx" runat="server">
        <!-- START: main -->
        <div id="main">

	        <!-- START: masthead -->
	        <div id="masthead">
		        <div id="logo"><a href="#"><img src="images/logo_gooey_internal.png" width="225" height="75" alt="Gooey CMS" /></a></div>
		        <div id="nav">
		        </div>
	        </div>
	        <!-- END: masthead -->

	        <!-- START: content -->
	        <div id="content">
		        <h1><IMG SRC="images/build_your_biz.png" WIDTH="495" HEIGHT="37" BORDER="0" ALT="build your business one website at a time."></h1>
		        <div class="callout" id="callout">
        <IMG SRC="images/signup6.png" WIDTH="417" HEIGHT="117" BORDER="0" ALT="Signup for an invitation code to our private launch." /></div>

        <p><br><img src="images/designers_developers.png" width="574" height="248" border="0" alt="designers and developers reimagine how to build, host, and launch your client’s websites reimagine your business. be one of the first to experience gooey cms." /></p>

        <!-- START: signup-form -->
                <asp:Label ID="LblStatus" runat="server" />
		        <ol id="signup-steps">
			        <li>
				        <table cellspacing="0" class="form">
				        <tr>
				        <td class="label"><label for="first-name">First Name</label></td>
				        <td><asp:TextBox ID="TxtFirstname" runat="server" /></td>
				        </tr>
				        <tr>
				        <td class="label"><label for="last-name">Last Name</label></td>
				        <td><asp:TextBox ID="TxtLastname" runat="server" /></td>
				        </tr>
				        <tr>
				        <td class="label"><label for="email">Email</label></td>
				        <td><asp:TextBox ID="TxtEmail" runat="server" /></td>
				        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Button ID="BtnSubmit" OnClick="BtnSubmit_Click" Text="Sign Up" runat="server" /></td>
                        </tr>
				        </table>
			        </li>
		        </ol>

		        <!-- START: signup-form -->

	        </div>
	        <!-- END: content -->


	        <!-- START: footer -->
	        <div id="footer">
		        <ul class="adjacent">
		        </ul>
		        <p>&copy; 2007 - 2010 Beachead Technologies Inc.</p>
	        </div>
	        <!-- END: footer -->

        </div>
    </form>
</body>
</html>
