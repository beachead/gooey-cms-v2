<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="embedded.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.embedded" %>
<%@ OutputCache VaryByParam="g" Duration="10" Location="Server"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>gooey cms</title>
	<link rel="stylesheet" type="text/css" href="/css/global.css" />
	<script type="text/javascript" src="/js/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="/js/global.js"></script>
    <script type="text/javascript" src="/js/store.js"></script>

    <link rel="Stylesheet" type="text/css" href="/css/store.css" />
    <style type="text/css">
    body { background-image:none;
           background-color: #ffffff; }    
    div.columns .left-column {
        width: 400px;
    }
    
    div.columns .right-column {
        width: 300px;
        padding-top: 50px;
    }    
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul id="themes-panel" class="collapse-margin">
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
    </div>
    </form>
</body>
</html>
