<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResizableTextBox.ascx.cs" Inherits="Beachead.Web.CMS.controls.ResizableTextBox.ResizableTextBox" %>

<link rel="stylesheet" href="<% Response.Write(CssPath); %>" type="text/css" media="screen" />
<style type="text/css">
#<%=ResizableTextArea.ClientID %> {
    width: 99%;
    height: 100px;
    min-height: 150px;
    padding: 0;
}
</style>
<script type="text/javascript" src="<% Response.Write(ScriptPath); %>"></script>
<script language="javascript" type="text/javascript">
    var ResizableTextEditor = {
        editor: null,
        lineHeight: 20,

        init: function (id) {
            this.editor = dojo.byId(id);
            this.editor.style.overflowY = 'hidden';
            var that = this;
            dojo.connect(this.editor, 'onkeyup', function () {
                that.setEditorHeight();
            });
            this.editor.style.height = this.getAdjustedHeight() + "px";
        },

        getCurrentHeight: function () {
            return parseInt(this.editor.style.height);
        },

        getAdjustedHeight: function () {
            var lines = this.editor.value.split('\n'),
                nCharWidth = 5,
                nEditorOffset = this.editor.offsetWidth - 30,
                nMaxCharsPerLine = nEditorOffset / nCharWidth,
                nExtraLines = 0;

            // when line wrapping is enabled, a line feed isn't a reliable method for line counting.  in this count, add 
            // extra space for any line which is longer than nMaxCharsPerLine
            for (var i = 0, l = lines.length; i < l; i++) {
                if (lines[i].length >= nMaxCharsPerLine) {
                    nExtraLines += Math.floor((lines[i].length - nMaxCharsPerLine) / nMaxCharsPerLine);
                }
            }

            return (this.editor.value.split('\n').length + nExtraLines) * this.lineHeight;
        },

        setEditorHeight: function () {
            var nStart = this.getCurrentHeight(),
                nEnd = this.getAdjustedHeight();

            dojo.animateProperty({
                node: this.editor,
                properties: {
                    height: { start: nStart, end: nEnd }
                }
            }).play();

        }
    }

    dojo.addOnLoad(function () {
        ResizableTextEditor.init('<%=ResizableTextArea.ClientID %>');
    });


</script>

<asp:TextBox ID="ResizableTextArea" TextMode="MultiLine"  Wrap="false" runat="server" /> 
