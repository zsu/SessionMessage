using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text;
using SessionMessages.Core;

namespace SessionMessages.Mvc
{
	public class FluentSessionMessage : IHtmlString
    {
		//private string _imagePath = UrlHelper.GenerateContentUrl("~/Scripts", new HttpContextWrapper(HttpContext.Current))+"/Images/";
		private int _timeout = 5000;
		private int _extendedTimeout = 0;
		private Position _position = Position.TopRight;
		private bool _newestOnTop = false;
		private AnimitionEffect _showMethod = AnimitionEffect.FadeIn;
		private AnimitionEffect _hideMethod = AnimitionEffect.FadeOut;
		private bool _progressBar = false;
		private bool _closeButton = false;
		private AnimitionEffect _closeMethod = AnimitionEffect.FadeOut;
		//public FluentSessionMessage ImagePath(string path)
		//{
		//	_imagePath = path;
		//	return this;
		//}
		public FluentSessionMessage Timeout(int timeout)
		{
			_timeout = timeout;
			return this;
		}
		public FluentSessionMessage ExtendedTimeout(int timeout)
		{
			_extendedTimeout = timeout;
			return this;
		}
		public FluentSessionMessage DisplayPosition(Position position)
		{
			_position = position;
			return this;
		}
		public FluentSessionMessage ShowAnimitionEffect(AnimitionEffect effect)
		{
			_showMethod = effect;
			return this;
		}
		public FluentSessionMessage HideAnimitionEffect(AnimitionEffect effect)
		{
			_hideMethod = effect;
			return this;
		}
		public FluentSessionMessage CloseAnimitionEffect(AnimitionEffect effect)
		{
			_closeMethod = effect;
			return this;
		}
		public FluentSessionMessage Progressbar(bool enable)
		{
			_progressBar = enable;
			return this;
		}
		public FluentSessionMessage CloseButton(bool enable)
		{
			_closeButton = enable;
			return this;
		}
		public FluentSessionMessage NewestOnTop(bool enable)
		{
			_newestOnTop = enable;
			return this;
		}
		/// <summary>
        /// Render all messages that have been set during execution of the controller action.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public string RenderHtml()
        {
            var messages = string.Empty;
            List<SessionMessage> sessionMessages = SessionMessageManager.GetMessage();
            TagBuilder messageWrapper = null, messageBoxBuilder = null, messageBoxStatusBar = null, messageBoxModalBuilder = null, messageBoxModal = null;
            messageWrapper = new TagBuilder("div");
            messageWrapper.Attributes.Add("id", "messagewrapper");
            messageWrapper.Attributes.Add("style", "display: none");
            if (sessionMessages != null && sessionMessages.Count > 0)
            {
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
                            }
                            messageBoxBuilder = new TagBuilder("div");
                            //messageBoxBuilder.Attributes.Add("id", "messagebox" + i);
                            messageBoxBuilder.Attributes.Add("behavior", (sessionMessage.Behavior).ToString());
                            messageBoxBuilder.Attributes.Add("type", (sessionMessage.Type).ToString());
                            messageBoxBuilder.Attributes.Add("caption", sessionMessage.Caption);
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
                SessionMessageManager.Clear();
            }
            messages = messageWrapper.ToString();
            return messages;
        }
		private string RenderScript()
		{
			StringBuilder scripts = new StringBuilder();
			StringBuilder options = new StringBuilder();
			options.AppendLine(@"toastr.options = {");
			options.AppendFormat(@"closeButton: {0},
                newestOnTop: {1},
                progressBar: {2},
                positionClass: '{3}',
                timeOut: '{4}',
                extendedTimeOut: '{5}',
				showMethod: '{6}',
				hideMethod: '{7}',
				closeMethod: '{8}'", _closeButton.ToString().ToLower(), _newestOnTop.ToString().ToLower(), _progressBar.ToString().ToLower(),
								   ConvertPosition(_position), _timeout, _extendedTimeout, ConvertAnimitioEffect(_showMethod),
			   ConvertAnimitioEffect(_hideMethod), ConvertAnimitioEffect(_closeMethod));
            options.AppendLine(@"};");
            scripts.AppendFormat("<script type='text/javascript'>{0}</script>",options.ToString());
            var scriptPath=UrlHelper.GenerateContentUrl("~/Scripts", new HttpContextWrapper(HttpContext.Current));
            scripts.AppendFormat("<script type='text/javascript' src='{0}/sessionmessage.min.js'></script>",scriptPath);
			return scripts.ToString();
		}
		private string RenderCss()
		{
			StringBuilder css=new StringBuilder();
            var contentPath = UrlHelper.GenerateContentUrl("~/Content", new HttpContextWrapper(HttpContext.Current));
            css.AppendFormat("<link rel='stylesheet' href='{0}/sessionmessage.css' />",contentPath);
			return css.ToString();
		}
		public override string ToString()
		{
			var content = new StringBuilder();
			content.AppendLine(RenderCss());
			content.AppendLine(RenderHtml());
			content.AppendLine(RenderScript());
			return content.ToString();
		}
		public string ToHtmlString()
		{
			return ToString();
		}
		private string ConvertAnimitioEffect(AnimitionEffect effect)
		{
			return effect.ToString().Substring(0,1).ToLower() + effect.ToString().Substring(1);
		}
		private string ConvertPosition(Position position)
		{
			string result=null;
			switch(position)
			{
				case Position.TopCenter:
					result = "toast-top-center";
					break;
				case Position.TopFullWidth:
					result = "toast-top-full-width";
					break;
				case Position.TopLeft:
					result = "toast-top-left";
					break;
				case Position.TopRight:
					result = "toast-top-right";
					break;
				case Position.BottomCenter:
					result = "toast-bottom-center";
					break;
				case Position.BottomFullWidth:
					result = "toast-bottom-full-width";
					break;
				case Position.BottomLeft:
					result = "toast-bottom-left";
					break;
				case Position.BottomRight:
					result = "toast-bottom-right";
					break;
			}
			return result;
		}
    }
	public enum AnimitionEffect
	{
		FadeIn,
		FadeOut,
		SlideUp,
		SlideDown
	}
	public enum Position
	{
		TopCenter,
		TopFullWidth,
		TopLeft,
		TopRight,
		BottomCenter,
		BottomFullWidth,
		BottomLeft,
		BottomRight
	}
}