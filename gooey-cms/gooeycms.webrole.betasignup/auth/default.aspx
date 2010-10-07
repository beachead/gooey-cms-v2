<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="gooeycms.webrole.betasignup.auth._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GooeyCMS Invite Manager</title>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dijit/themes/claro/claro.css" />
    <script src="http://ajax.googleapis.com/ajax/libs/dojo/1.5/dojo/dojo.xd.js" djConfig="parseOnLoad: true" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        dojo.require("dojo.parser");
        dojo.require("dijit.TitlePane");
        dojo.require("dijit.dijit");
        dojo.require("dijit.Dialog");
        dojo.require("dijit.layout.TabContainer");
        dojo.require("dijit.layout.ContentPane");
        dojo.require("dijit.form.ValidationTextBox");
        dojo.require("dijit.Tooltip");
    </script>
</head>
<body class="claro">
    <form id="form1" runat="server">
        <div>
        <b>Gooey CMS Invite Manager</b>
        </div>
        
        <div id="mainTabContainer" dojoType="dijit.layout.TabContainer" style="width:90em;height:700px;overflow:auto;">
            <div id="invitesTab" dojoType="dijit.layout.ContentPane" title="Manage Invites">
                Invite Requests: <br />
                <asp:GridView ID="InviteRequests" 
                        OnRowCommand="InviteRequests_OnItemCommand"
                        OnRowDataBound="InviteRequests_OnItemDataBound"
                        AutoGenerateColumns="False" PageSize="25" 
                        ForeColor="#333333" GridLines="None" runat="server" 
                        CellPadding="4" UpdateAfterCallBack="True" Width="80%">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="Requested">
                            <ItemTemplate>
                                <asp:Label ID="Requested" Text='<%# Eval("Created") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="Email" Text='<%# Eval("Email") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Firstname">
                            <ItemTemplate>
                                <asp:Label ID="Firstname" Text='<%# Eval("Firstname") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lastname">
                            <ItemTemplate>
                                <asp:Label ID="Lastname" Text='<%# Eval("Lastname") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Issued">
                            <ItemTemplate>
                                <asp:Label ID="Issued" Text='<%# Eval("Issued") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="BtnApprove" CommandName="InviteApprove" CommandArgument='<%# Eval("Guid") %>' Text="Approve" runat="server" />
                                <asp:Button ID="BtnDelete" CommandName="InviteDelete" CommandArgument='<%# Eval("Guid") %>' Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this invite?');" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>        
                    </Columns>
                    <EmptyDataTemplate>
                        No Invites Found
                    </EmptyDataTemplate>
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                </asp:GridView>
            </div>
            <div id="emailTab" style="display:none;" dojoType="dijit.layout.ContentPane" title="Manage Invite Email">
                Input the text of the invite email below.<br />
                You can use {firstname}, {lastname}, {email} as variables within the email itself.
                <br /><br />
                <asp:Button ID="BtnSaveInviteEmail" Text="Save" OnClick="BtnSaveInviteEmail_Click" runat="server" />
                <asp:TextBox ID="TxtInviteEmail" Rows="35" Columns="115" TextMode="MultiLine" runat="server" />
            </div>
            <div id="responseTab" style="display:none;" dojoType="dijit.layout.ContentPane" title="Manage Approved Email">
                Input the text of the approved email below.<br />
                You can use {firstname}, {lastname}, {email}, {token} as variables within the email itself.
                <br /><br />
                <asp:Button ID="BtnSaveApproveEmail" Text="Save" OnClick="BtnSaveApproveEmail_Click" runat="server" />
                <asp:TextBox ID="TxtApproveEmail" Rows="35" Columns="115" TextMode="MultiLine" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
