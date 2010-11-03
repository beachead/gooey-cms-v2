<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.Developer._default" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="localJS" ContentPlaceHolderID="localJS" runat="server">
	<script type="text/javascript" src="../../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../../scripts/store.js"></script>
</asp:Content>
<asp:Content id="localCSS" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" type="text/css" href="../../../css/store.css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="global_admin_developer" NavItem="approval" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">

    The following packages are awaiting approval: <br />

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <script language="javascript" type="text/javascript">
        function sendEmail(guid) {
            var wnd = window.radopen("SendEmail.aspx?package=" + guid, null);
        }
    </script>

	<ul id="themes-panel">
        <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" OnItemCommand="SitePackages_OnItemCommand" runat="server">
        <ItemTemplate>
	    <li class="theme">    
            <!-- start: theme-header -->
            <div class="theme-header">
			    <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c0}") %></div>
			    <div class="logo"><asp:Image ID="Logo" runat="server" /></div>
			    <ul class="options-links">
                    <li><asp:HyperLink ID="DemoLink" Target="_blank" runat="server">View Site</asp:HyperLink></li>
                    <li><a href="#" onclick="sendEmail('<%# Eval("Guid") %>'); return false;">Email Owner</a>&nbsp;or&nbsp;<asp:LinkButton ID="BtnApprove" OnClientClick="return confirm('Are you sure you want to approve this site package?');" CommandName="ApprovePackage" CommandArgument='<%# Eval("Guid") %>' runat="server">Approve</asp:LinkButton></li>

                </ul>
            </div>
            <!-- end: theme-header -->

            <!-- start: preview -->
            <div class="preview">
                <ul class="thumbs">
                    <asp:Repeater ID="ThumbnailImages" runat="server">
                        <ItemTemplate>
                            <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                        </ItemTemplate>
                    </asp:Repeater>
			    </ul>
				        
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

    <telerik:RadWindowManager ID="Singleton" Skin="Windows7" Modal="true" Height="300" Width="410" ShowContentDuringLoad="false" AutoSize="true" VisibleStatusbar="false" Behaviors="Close,Move,Resize" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
</asp:Content>
