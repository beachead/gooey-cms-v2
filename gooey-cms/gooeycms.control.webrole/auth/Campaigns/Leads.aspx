﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Leads.aspx.cs" ValidateRequest="false" Inherits="Gooeycms.Webrole.Control.auth.Campaigns.Leads" %>
<%@ MasterType VirtualPath="~/Secure.master" %>
<%@ Register TagPrefix="gooey" Src="~/Controls/Subnav.ascx" TagName="Subnav" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <gooey:Subnav ID="Subnav" runat="server" NavSection="campaigns" NavItem="leadreport" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <h1>LEAD REPORT</h1>
    <p>This page allows you to export your leads into Excel.</p>
    <telerik:RadAjaxPanel ID="SelectDatePanel" LoadingPanelID="RadAjaxFilterLoadingPanel" runat="server">
    <br />
    Search for leads within the following dates:
    <br /><br />
        <table>
            <tr>
                <td>Start Date:</td>
                <td><telerik:RadDatePicker ID="ReportStartDate" Skin="Outlook" ShowPopupOnFocus="true" runat="server" />
                <asp:RequiredFieldValidator ID="ReportStartDateRequired" ControlToValidate="ReportStartDate" ErrorMessage="*" runat="server"  /> 
                </td>
                <td>End Date:</td>
                <td>
                <telerik:RadDatePicker ID="ReportEndDate" Skin="Outlook" ShowPopupOnFocus="true" runat="server" />
                <asp:RequiredFieldValidator ID="ReportEndDateRequired" ControlToValidate="ReportEndDate" ErrorMessage="*" runat="server"  />
                </td>
                <td><asp:Button ID="BtnFilterDates" OnClick="BtnFilterDate_Click" Text="Search" runat="server" /></td>
            </tr>
        </table>
        <br />

        <asp:Panel ID="SelectPagesPanel" Visible="false" runat="server">
            Select which pages to include in the lead report:
            <br /><br />
            <table>
                <tr>
                    <td><asp:ListBox ID="LstSelectPages" SelectionMode="Multiple" Width="225px" Rows="20" runat="server" /></td>
                </tr>
                <tr>
                    <td style="text-align:right;"><asp:Button ID="BtnGenerateReport" OnClick="BtnGenerateReport_Click" Text="Export" runat="server" /></td>
                </tr>
            </table>
        </asp:Panel>
        
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxFilterLoadingPanel" Skin="Vista" Transparency="50" runat="server">
        <div style="background-color:#fff; height:100%; width:100%">
        <asp:Image ID="LoadingImage" ImageUrl="~/images/loading3.gif" runat="server" />
        </div>
    </telerik:RadAjaxLoadingPanel>

</asp:Content>
