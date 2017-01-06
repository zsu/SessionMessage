using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text;

namespace SessionMessage
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Render all messages that have been set during execution of the controller action.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IHtmlString RenderMessages(this HtmlHelper htmlHelper)
        {
            var messages = string.Empty;
            List<SessionMessage> sessionMessages = SessionMessageManager.GetMessage();
            if (sessionMessages != null && sessionMessages.Count > 0)
            {
				TagBuilder messageWrapper=null,messageBoxBuilder = null, messageBoxStatusBar = null, messageBoxModalBuilder = null, messageBoxModal = null;
				messageWrapper = new TagBuilder("div");
				messageWrapper.Attributes.Add("id", "messagewrapper");
				messageWrapper.Attributes.Add("style", "display: none");
                for (int i = 0; i < sessionMessages.Count; i++)
                {
                    var sessionMessage = sessionMessages[i];
                    switch (sessionMessage.Behavior)
                    {
                        case MessageBehaviors.Modal:
                            if (messageBoxModal == null)
                            {
                                messageBoxModal = new TagBuilder("div");
                                messageBoxModal.Attributes.Add("id", "messageboxmodal");
                                //messageBoxModal.Attributes.Add("behavior", ((int)sessionMessage.Behavior).ToString());
                            }
                            messageBoxModalBuilder = new TagBuilder("div");
                            //messageBoxModalBuilder.Attributes.Add("id", "messagebox" + i);
                            //messageBoxModalBuilder.Attributes.Add("behavior", ((int)sessionMessage.Behavior).ToString());
                            messageBoxModalBuilder.AddCssClass(String.Format("messagebox {0}", Enum.GetName(typeof(MessageType), sessionMessage.Type).ToLowerInvariant()));
                            if(!string.IsNullOrEmpty(sessionMessage.Key))
                                messageBoxBuilder.Attributes.Add("key", sessionMessage.Key);
                            messageBoxModalBuilder.InnerHtml += sessionMessage.Message;
                            messageBoxModal.InnerHtml += messageBoxModalBuilder.ToString();
                            break;
                        case MessageBehaviors.StatusBar:
                        default:
                            if (messageBoxStatusBar == null)
                            {
                                messageBoxStatusBar = new TagBuilder("div");
                                messageBoxStatusBar.Attributes.Add("id", "messageboxstatusbar");
                                //messageBoxStatusBar.Attributes.Add("behavior", ((int)sessionMessage.Behavior).ToString());
                            }
                            messageBoxBuilder = new TagBuilder("div");
                            //messageBoxBuilder.Attributes.Add("id", "messagebox" + i);
                            //messageBoxBuilder.Attributes.Add("behavior", ((int)sessionMessage.Behavior).ToString());
                            messageBoxBuilder.AddCssClass(String.Format("messagebox {0}", Enum.GetName(typeof(MessageType), sessionMessage.Type).ToLowerInvariant()));
                            if(!string.IsNullOrEmpty(sessionMessage.Key))
                                messageBoxBuilder.Attributes.Add("key", sessionMessage.Key);
                            messageBoxBuilder.InnerHtml += sessionMessage.Message;
                            messageBoxStatusBar.InnerHtml += messageBoxBuilder.ToString();
                            break;
                    }
                }
                messages = messageBoxStatusBar == null || string.IsNullOrEmpty(messageBoxStatusBar.ToString()) ? null : messageBoxStatusBar.ToString();
				messages+= messageBoxModal == null || string.IsNullOrEmpty(messageBoxModal.ToString()) ? null : messageBoxModal.ToString();
				messageWrapper.InnerHtml += messages;
				messages = messageWrapper.ToString();
                SessionMessageManager.Clear();
            }
            //return MvcHtmlString.Create(messages);
			return htmlHelper.Raw(RenderCss()+"\n"+messages+RenderScript());
        }
		private static string RenderScript()
		{
			StringBuilder scripts = new StringBuilder();
			scripts.AppendLine("<script type='text/javascript'>");
			scripts.AppendLine(@"$().ready(function () {
    handleAjaxMessages();
    displayMessages();
});
function displayMessages() {
    var messagewrapper = $('#messagewrapper');
    var messageboxstatusbar = $('#messageboxstatusbar');
    var messageboxmodal = $('#messageboxmodal');
    if (messagewrapper.children().length > 0) {
        if (messageboxstatusbar && messageboxstatusbar.children().length > 0) {
            var timeoutId;
            messagewrapper.mouseenter(function () {
                if (timeoutId) {
                    clearTimeout(timeoutId);
                    messagewrapper.stop(true).css('opacity', 1).show();
                }
            }).mouseleave(function () {
                timeoutId = setTimeout(function () {
                    messagewrapper.slideUp('slow');
                }, 5000);
            });
            messagewrapper.show();
            // display status message for 5 sec only
            timeoutId = setTimeout(function () {
                messagewrapper.slideUp('slow');
            }, 5000);
            $(document).click(function () {
                clearMessages();
            });
        }
        if (messageboxmodal && messageboxmodal.children().length > 0) {
            messageboxmodal.dialog({
                bgiframe: true,
                autoOpen: false,
                modal: true,
                title: 'Message',
                close: function (event, ui) {
                    try {
                        $(this).dialog('destroy').remove();
                        clearMessages();
                        return true;
                    }
                    catch (e) {
                        return true;
                    }
                }
            });
            messageboxmodal.dialog('open');
            messagewrapper.show();
        }
    }
    else {
        messagewrapper.hide();
    }
}

function clearMessages() {
    $('#messagewrapper').fadeOut(500, function () {
        $('#messagewrapper').empty();
    });
}

function handleAjaxMessages() {
    $(document).ajaxSuccess(function (event, request) {
        if (request.getResponseHeader('FORCE_REDIRECT') !== null) {
            window.location = request.getResponseHeader('FORCE_REDIRECT');
            return;
        }
        checkAndHandleMessageFromHeader(request);
    })
    //.ajaxError(function (event, request) {
    //    if (request.getResponseHeader('FORCE_REDIRECT') !== null) {
    //        window.location = request.getResponseHeader('FORCE_REDIRECT');
    //        return;
    //    }
    //    var responseMessage, exception;
    //    try {//Error handling for POST calls
    //        var jsonResult = JSON.parse(request.responseText);
    //        if (jsonResult && jsonResult.Message) {
    //            responseMessage = jsonResult.Message;
    //            if (jsonResult.ExceptionMessage) {
    //                exception = 'Message: ' + jsonResult.Message + ' Exception: ' + jsonResult.ExceptionMessage;
    //                if (jsonResult.StackTrace)
    //                    exception += jsonResult.StackTrace;
    //            }
    //        }
    //    }

    //    catch (ex) {//Error handling for GET calls
    //        if (request.responseText)
    //            responseMessage = request.responseText;
    //        else
    //            responseMessage = 'Status: '' + request.statusText + ''. Error code: ' + request.status;
    //    }
    //    if (exception)
    //        log.error(exception);
    //    else
    //        log.error(responseMessage);
    //    //var message = '<div id='messagebox' behavior=' + '2' + ' class='messagebox ' + 'error' + ''>' + responseMessage + '</div>';
    //    //displayMessage(message, 'error', 2);
    //});
}

function checkAndHandleMessageFromHeader(request) {
    var msg = request.getResponseHeader('X-Message');
    if (msg) {
        displayMessage(msg);
    }
    msg = request.getResponseHeader('X-ModalMessage');
    if (msg) {
        displayModalMessage(msg);
    }
}
function displayMessage(message) {
    var jsonResult = JSON.parse(message);
    if (jsonResult) {
        jQuery('<div/>', {
            id: 'messageboxstatusbar',
            class: 'messagebox'
        }).appendTo('#messagewrapper');
        var messageboxstatusbar = $('#messageboxstatusbar');
        //var messageboxmodal = $('#messageboxmodal');
        var loaded = false;
        //if ((messageboxstatusbar && messageboxstatusbar.children().length > 0) || (messageboxmodal && messageboxmodal.children().length > 0)) {
        //    loaded = true;
        //}
        $.each(jsonResult, function (i, item) {
            if ((messageboxstatusbar && messageboxstatusbar.children().length > 0) && item.Key) {
                if (messageboxstatusbar.children('div[key=' + item.Key + ']').length != 0)
                    return true;
            }
            jQuery('<div/>', {
                class: 'messagebox ' + item.Type,
                text: item.Message,
                key: item.Key
            }).appendTo('#messageboxstatusbar');
        });
        displayMessages();
    }
}
function displayModalMessage(message) {
    var jsonResult = JSON.parse(message);
    if (jsonResult) {
        jQuery('<div/>', {
            id: 'messageboxmodal',
            class: 'messagebox'
        }).appendTo('#messagewrapper');
        //var messageboxstatusbar = $('#messageboxstatusbar');
        var messageboxmodal = $('#messageboxmodal');
        var loaded = false;
        //if ((messageboxstatusbar && messageboxstatusbar.children().length > 0) || (messageboxmodal && messageboxmodal.children().length > 0)) {
        //    loaded = true;
        //}
        $.each(jsonResult, function (i, item) {
            if ((messageboxmodal && messageboxmodal.children().length > 0) && item.Key) {
                if (messageboxmodal.children('div[key=' + item.Key + ']').length != 0)
                    return true;
            }
            jQuery('<div/>', {
                class: 'messagebox ' + item.Type,
                text: item.Message,
                key: item.Key
            }).appendTo('#messageboxmodal');
        });
        displayMessages();
    }
}");
			scripts.AppendLine("</script>");
			return scripts.ToString();
		}
		private static string RenderCss()
		{
			string css=@"<style>
	#messagewrapper {
    position: fixed;
    top: 0px;
    right: 10px;
    z-index: 10000;
    margin: 5px auto;
    /*width: 500px;*/
}

    #messagewrapper .messagebox {
        /*padding: 12px 10px 10px 30px;*/
        box-shadow: 5px 5px 10px #000;
    }

    #messagewrapper .success {
        color: #060;
        background: #cfc url(/Content/Images/ok.gif) no-repeat 8px 12px !important;
    }

    #messagewrapper .info {
        color: #06f;
        background: #0df url(/Content/Images/info.gif) no-repeat 8px 12px !important;
    }

    #messagewrapper .warning {
        color: #c60;
        background: #ffc url(/Content/Images/warn.gif) no-repeat 8px 12px !important;
    }

    #messagewrapper .error {
        color: #c00;
        background: #fcc url(/Content/Images/fail.gif) no-repeat 8px 12px !important;
    }

.messagebox.success {
    background: #cfc url(/Content/Images/ok.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
}

.messagebox.info {
    background: #0df url(/Content/Images/info.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
    font-size: 0.85em;
    font-weight: bold;
}

.messagebox.warning {
    background: #ffc url(/Content/Images/warn.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
}

.messagebox.error {
    background: #fcc url(/Content/Images/fail.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
}

.ui-dialog-content .messagebox.success {
    background: url(/Content/Images/ok.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
}

.ui-dialog-content .messagebox.info {
    background: url(/Content/Images/info.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
    font-size: 0.85em;
    font-weight: bold;
}

.ui-dialog-content .messagebox.warning {
    background: url(/Content/Images/warn.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
}

.ui-dialog-content .messagebox.error {
    background: url(/Content/Images/fail.gif) no-repeat 8px 12px !important;
    text-align: left;
    padding: 12px 10px 10px 30px !important;
}
</style>";
			return css;
		}
    }
}