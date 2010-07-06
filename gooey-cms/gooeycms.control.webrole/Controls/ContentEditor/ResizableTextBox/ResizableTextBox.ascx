<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResizableTextBox.ascx.cs" Inherits="Beachead.Web.CMS.controls.ResizableTextBox.ResizableTextBox" %>
<link rel="stylesheet" href="<% Response.Write(CssPath); %>" type="text/css" media="screen" />
<script type="text/javascript" src="<% Response.Write(ScriptPath); %>"></script>

<div id="<%=ResizableTextArea.ClientID %>_rtContainer" style="width:100%;height:100%;border:0px;">   
</div>
<asp:TextBox ID="ResizableTextArea" TextMode="MultiLine" Wrap="false" runat="server" /> 

<script language="javascript" type="text/javascript">
    var rt = new ResizeableTextbox('<%=ResizableTextArea.ClientID %>');

    rt.GetContainer().style.left = '0px';
    rt.GetContainer().style.top = '0px';
    rt.SetMaxWidth(2000);
    rt.SetMaxHeight(2000);   
    rt.SetCurrentWidth(800);
    rt.SetCurrentHeight(300);

    var myDiv = document.getElementById('<%=ResizableTextArea.ClientID %>_rtContainer');
    myDiv.appendChild(rt.GetContainer());

    rt.StartListening();
</script>
