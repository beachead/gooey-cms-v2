<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResizableTextBox.ascx.cs" Inherits="Beachead.Web.CMS.controls.ResizableTextBox.ResizableTextBox" %>

<link rel="stylesheet" href="<% Response.Write(CssPath); %>" type="text/css" media="screen" />
<style type="text/css">
#<%=ResizableTextArea.ClientID %> {
    width: 90%;
    height: 100px;
    min-height: 150px;
    padding: 0;
}
</style>
<script type="text/javascript" src="<% Response.Write(ScriptPath); %>"></script>
<script language="javascript" type="text/javascript">
    var ResizableTextEditor = {
        editor: null,
        lineHeight: 17,

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
            return this.editor.value.split('\n').length * this.lineHeight;
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
