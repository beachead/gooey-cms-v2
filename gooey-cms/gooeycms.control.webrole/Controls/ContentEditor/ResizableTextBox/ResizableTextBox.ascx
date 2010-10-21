<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResizableTextBox.ascx.cs" Inherits="Beachead.Web.CMS.controls.ResizableTextBox.ResizableTextBox" %>

<link rel="stylesheet" href="<% Response.Write(CssPath); %>" type="text/css" media="screen" />
<style type="text/css">
#<%=ResizableTextArea.ClientID %> {
    width: 100%;
    height: 100px;
    padding: 0;
}
</style>
<script type="text/javascript" src="<% Response.Write(ScriptPath); %>"></script>
<script language="javascript" type="text/javascript">

    dojo.addOnLoad(function () {
        var oTextEditor = dojo.byId('<%=ResizableTextArea.ClientID %>');
        oTextEditor.style.height = (oTextEditor.scrollHeight + 20) + "px";
        oTextEditor.style.overflowY = 'hidden';

        dojo.connect(oTextEditor, "onkeyup", function (e) {

            var nContentHeight = oTextEditor.scrollHeight;
            var nEditorHeight = parseInt(oTextEditor.style.height);
            //var nHeight = oTextEditor.offsetHeight;

            if (nContentHeight > nEditorHeight - 24) {
                dojo.animateProperty({
                    node: oTextEditor,
                    properties: {
                        height: { end: nContentHeight + 24, start: nEditorHeight }
                    }
                }).play();
            }

            if (nEditorHeight > nContentHeight + 16) {
                dojo.animateProperty({
                    node: oTextEditor,
                    properties: {
                        height: { end: nContentHeight, start: nEditorHeight }
                    }
                }).play();
            }


        });
    });

</script>

<asp:TextBox ID="ResizableTextArea" TextMode="MultiLine"  Wrap="false" runat="server" /> 
