<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarkupEditor.ascx.cs" Inherits="Beachead.Web.CMS.controls.MarkupEditor" %>
<%@ Register src="~/controls/ContentEditor/ResizableTextBox/ResizableTextBox.ascx" tagname="ResizableTextBox" tagprefix="uc" %>
    <link rel="stylesheet" type="text/css" href="<% Response.Write(EditorCssPath); %>" />
    <script type="text/javascript" src="<% Response.Write(EditorScriptPath); %>"></script>  
    <script type="text/javascript">
        dojo.addOnLoad(function () {
            dojo.connect(dojo.byId("preview-frame"), "onload", "endPreview");

            var oHelpPanel = dojo.query("#helpPanel"),
                oHelpTopics = dojo.query("#helpTopics a"),
                oHelpContentItems = dojo.query('#helpContents li.help-content-item'),
                oHelpLink = dojo.query("#HelpLink");

            dojo.forEach(oHelpTopics, function (current, index) {
                var oTopic = current;
                dojo.connect(oTopic, 'onclick', function (e) {
                    e.preventDefault();
                    oHelpTopics.removeClass('active');
                    oTopic.addClass('active');
                    var sActiveTopic = dojo.attr(oTopic, 'href');
                    dojo.forEach(oHelpContentItems, function (item) {
                        if (sActiveTopic.indexOf(item.id) != -1) {
                            item.style.display = 'block';
                        } else {
                            item.style.display = 'none';
                        }
                    });
                    return false;
                });
            });

        });

        function showHelpPanel() {
            var oHelpPanel = document.getElementById("helpPanel");
            if (oHelpPanel.style.display == 'none') {
                dojo.fx.wipeIn({ node: oHelpPanel }).play();
            } else {
                dojo.fx.wipeOut({ node: oHelpPanel }).play();
            }
            return false;
        }
    </script>  
    <style type="text/css">
        #waitDialog .dijitDialogTitleBar .dijitDialogCloseIcon {
            display:none;
        }
        #helpContentsList {
            margin: 0;
            padding: 0;
        }
    </style>
    <div dojoType="dijit.Dialog" id="waitDialog" closable="false" draggable="false">
        Please wait while your preview is generated.
    </div>
    
    <% if (ShowPreviewWindow) { %>
    <div dojoType="dijit.Dialog" id="preview-panel" style="width:90%;height:90%;border:0px;overflow:auto;"  title="Page Preview" closeable="true">
            <iframe id="preview-frame" src=""></iframe>
    </div>
    <% } %>
        <asp:Panel ID="ToolbarPanel" runat="server">
            <ul id="FormatUl">
                <li><a href="#" onclick="javascript:return __Wrap('**', '**','<%=PageMarkupText.TextboxId %>');" title="Bold" class="formatlink" id="BoldLink"></a></li>
                <li><a href="#" onclick="javascript:return __Wrap('*', '*','<%=PageMarkupText.TextboxId %>');" title="Italics" class="formatlink" id="ItalicLink"></a></li>
                <li><a href="#" onclick="javascript:return __Wrap('__ ', ' __','<%=PageMarkupText.TextboxId %>');" title="Underline" class="formatlink" id="UnderlineLink"></a></li>
                <li><a href="#" onclick="javascript:return __Wrap('# ', '','<%=PageMarkupText.TextboxId %>');" title="H1 Heading" class="formatlink" id="H1Link"></a></li>
                <li><a href="#" onclick="javascript:return __Wrap('## ', '','<%=PageMarkupText.TextboxId %>');" title="H2 Heading" class="formatlink" id="H2Link"></a></li>
                <li><a href="#" onclick="javascript:return __Wrap('\r\n* Item 1\r\n* Item 2 ', '\r\n','<%=PageMarkupText.TextboxId %>');" title="Unordered List" class="formatlink" id="UnorderedList"></a></li>
                <li><a href="#" onclick="javascript:return __Wrap('\r\n1. Item 1\r\n2. Item 2 ', '\r\n','<%=PageMarkupText.TextboxId %>');" title="Ordered List" class="formatlink" id="OrderedList"></a></li>
                <li><a href="#" onclick="javascript:return __Insert('{BR}','<%=PageMarkupText.TextboxId %>');" title="Line Break" class="formatlink" id="BrLink"></a></li>
                <li><a href="#" onclick="javascript:window.open('ImageBrowser.aspx?<%=ImageBrowserQuerystring %>','','width=700,height=500,left=400,top=400,titlebar=no,toolbar=no,resizable=no,modal=yes,centerscreen=yes;scroll=no;status=no,menubar=no,location=no'); return false;" title="Image Browser" class="formatlink" id="ImageLink"></a></li>
                <li><a href="#" onclick="javascript:showFormEditor(); return false;" title="Form Editor" class="formatlink" id="FormLink"></a></li>             
                <% if (ShowPreviewWindow) { %><li><anthem:LinkButton ID="PreviewLink" class="PreviewLink" runat="server" OnClientClick="keypressHandler(null); return false;" ToolTip="Page Preview" CssClass="formatlink PreviewLink" /></li><% } %>
                <li><a href="#" onclick="javascript:showHelpPanel(); return false;" title="Show Markup Help" id="HelpLink">Help</a></li>
            </ul>
        </asp:Panel>
        <!-- START: helpPanel -->
        <div id="helpPanel" style="display: none;">

            <!-- START: helpTopics -->
            <div id="helpTopics">
                <ul class="flat">
                    <li><a class="active" href="#helpHeadings">Headings</a></li>
                    <li><a href="#helpFormatting">Formatting</a></li>
                    <li><a href="#helpLists">Lists</a></li>
                    <li><a href="#helpLinks">Links</a></li>
                    <li><a href="#helpImages">Images</a></li>
                </ul>
            </div>
            <!-- END: helpTopics -->

            <!-- START: helpContents -->
            <div id="helpContents">
                <ul id="helpContentsList">
                    <li id="helpHeadings" class="help-content-item" style="display: block;">
                        <table cellspacing="0">
                        <tr>
                        <th class="first">Element</th>
                        <th>You Type...</th>
                        <th>You Get...</th>
                        </tr>

                        <tr>
                        <td class="first">First Level Heading</td>
                        <td># Heading</td>
                        <td><h1>Heading</h1></td>
                        </tr>

                        <tr>
                        <td class="first">Second Level Heading</td>
                        <td>## Heading</td>
                        <td><h2>Heading</h2></td>
                        </tr>


                        <tr>
                        <td class="first">Third Level Heading</td>
                        <td>### Heading</td>
                        <td><h3>Heading</h3></td>
                        </tr>
                        </table>
                    </li>

                    <li id="helpFormatting" class="help-content-item">
                        <table cellspacing="0">
                        <tr>
                        <th class="first">Element</th>
                        <th>You Type...</th>
                        <th>You Get...</th>
                        </tr>

                        <tr>
                        <td class="first">Bold</td>
                        <td>**Text**</td>
                        <td><strong>Text</strong></td>
                        </tr>

                        <tr>
                        <td class="first">Italics</td>
                        <td>*Heading*</td>
                        <td><em>Text</em></td>
                        </tr>
                        </table>
                    </li>

                    <li id="helpLists" class="help-content-item">
                        <table cellspacing="0">
                        <tr>
                        <th class="first">Element</th>
                        <th>You Type...</th>
                        <th>You Get...</th>
                        </tr>

                        <tr>
                        <td class="first">Unordered List</td>
                        <td>
                            * List Item<br />
                            * List Item<br />
                            * List Item<br />
                            </pre>
                        </td>
                        <td>
                            <ul>
                                <li>List Item</li>
                                <li>List Item</li>
                                <li>List Item</li>
                            </ul>
                        </td>
                        </tr>

                        <tr>
                        <td class="first">Unordered List w/ Nested List</td>
                        <td>
                            * List Item<br />
                            * List Item<br />
                            &nbsp;&nbsp;&nbsp;&nbsp;* List Item<br />
                            * List Item<br />
                            </pre>
                        </td>
                        <td>
                            <ul>
                                <li>List Item</li>
                                <li>List Item
                                    <ul>
                                        <li>List Item</li>
                                    </ul>
                                </li>
                                <li>List Item</li>
                            </ul>
                        </td>
                        </tr>

                        <tr>
                        <td class="first">Ordered List</td>
                        <td>
                            1. List Item<br />
                            2. List Item<br />
                            3. List Item<br />
                        </td>
                        <td>
                            <ol>
                                <li>List Item</li>
                                <li>List Item </li>
                                <li>List Item</li>
                            </ol>
                        </td>
                        </tr>

                        <tr>
                        <td class="first">Ordered List w/ Nested List</td>
                        <td>
                            1. List Item<br />
                            2. List Item<br />
                            &nbsp;&nbsp;&nbsp;&nbsp;1. List Item<br />
                            3. List Item<br />
                        </td>
                        <td>
                            <ol>
                                <li>List Item</li>
                                <li>List Item
                                    <ol>
                                        <li>List Item</li>
                                    </ol>
                                </li>
                                <li>List Item</li>
                            </ol>
                        </td>
                        </tr>
                        </table>
                    </li>

                    <li id="helpLinks" class="help-content-item">
                        <table cellspacing="0">
                        <tr>
                        <th class="first">Element</th>
                        <th>You Type...</th>
                        <th>You Get...</th>
                        </tr>

                        <tr>
                        <td class="first">Link</td>
                        <td>[link text](http://www.gooeycms.com)</td>
                        <td<a href="http://www.gooeycms.com">link text</a></td>
                        </tr>

                        <tr>
                        <td class="first">Link w/ Title</td>
                        <td>[link text](http://www.gooeycms.com "Optional title")</td>
                        <td><a href="http://www.gooeycms.com" title="Optional title">link text</a></td>
                        </tr>

                        <tr>
                        <td class="first">Linked Image</td>
                        <td>[ ![alt text](/images/progress_green.gif) ](http://gooeycms.com "Optional title")</td>
                        <td><a href="http://www.gooeycms.com" title="Optional title"><img src="/images/progress_green.gif" alt="alt text" /></a></td>
                        </tr>
                        </table>
                    </li>

                    <li id="helpImages" class="help-content-item">
                        <table cellspacing="0">
                        <tr>
                        <th class="first">Element</th>
                        <th>You Type...</th>
                        <th>You Get...</th>
                        </tr>

                        <tr>
                        <td class="first">Image</td>
                        <td>![alt text](/images/progress_green.gif)</td>
                        <td><img src="/images/progress_green.gif" alt="alt text"></td>
                        </tr>

                        <tr>
                        <td class="first">Image w/ Alt Text</td>
                        <td>![alt text](/images/progress_green.gif "Optional title")</td>
                        <td><img src="/images/progress_green.gif" alt="alt text" title="Optional title"></td>
                        </tr>
                        </table>
                    </li>

                </ul>
            </div>
            <!-- END: helpContents -->
        </div>
        <!-- END: helpPanel -->

        <script type="text/javascript" language="javascript">
            function editor_wrap() {
                var chk = dojo.byId('chkwrap');
                var popup = dojo.byId('<%= PageMarkupText.TextboxId %>');
                if (chk.checked) {
                    if (popup.wrap) {
                        popup.wrap = "soft";
                    } else {
                        popup.setAttribute("wrap", "soft");
                        var newpopup = popup.cloneNode(true);
                        newpopup.value = popup.value;
                        popup.parentNode.replaceChild(newpopup, popup);
                    }
                } else {
                    if (popup.wrap) {
                        popup.wrap = "off";
                    } else {
                        popup.setAttribute("wrap", "off");
                        var newpopup = popup.cloneNode(true);
                        newpopup.value = popup.value;
                        popup.parentNode.replaceChild(newpopup, popup);
                    }

                }
            }
        </script>

        <input type="checkbox" id="chkwrap" onclick="editor_wrap();" />wrap
        <div style="max-height:430px; overflow:auto;"> 
            <uc:ResizableTextBox ID="PageMarkupText" runat="server" />
        </div>

<script language="javascript" type="text/javascript">
    var parent = null;

    function getCurrentEditorText() {
        var inline = dojo.byId('<%= PageMarkupText.TextboxId  %>');
        return inline.value;
    }

    function doEditorInsert(text) {
        __Insert(text,'<%=PageMarkupText.TextboxId %>');
    }


    function onimage_selected(imageName) {
        if (imageName.substr(-3) === 'swf') {
            __Insert('~/' + imageName,'<%=PageMarkupText.TextboxId %>');
        }
        else {
        <% if (UseStandardImageTags) { %>
            __Insert('../images/' + imageName,'<%=PageMarkupText.TextboxId %>');
        <% } else { %>
            __Insert('[[image:~/' + imageName + ']]{BR}','<%=PageMarkupText.TextboxId %>');
        <% } %>
        }
    }

    function keypressHandler(obj) {
        if (window.mytimeout) window.clearTimeout(window.mytimeout);
        startPreview();
        Anthem_InvokeControlMethod('<%=this.ClientID %>','Preview_Click', [],
            function (result) {
                DisplayPreview(result.value);
            }
        );
    }

    function startPreview() {
        var preview = dojo.byId("preview-frame");

        preview.style.width = (screen.width - 200) + "px";
        preview.style.height = (screen.height - 300) + "px";
        var pane = dijit.byId("preview-panel");
        pane.show();

        dijit.byId('waitDialog').show();
    }

    function endPreview() {
        dijit.byId('waitDialog').hide();
    }

    function DisplayPreview(url) {
        if (url.indexOf("::ALERT::") != -1) {
            alert(url.replace("::ALERT::", ""));
        }
        else {
            var iframe = dojo.byId("preview-frame");
            iframe.src = url;
        }
    }
    
    $('<%=PageMarkupText.TextboxId %>').addEvent('keydown', function(event) {
            if (event.key == 'tab') {
                return __Insert ('    ','<%=PageMarkupText.TextboxId %>');
            }
        });

</script>