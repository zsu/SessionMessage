/// Author: Zhicheng Su
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Security.AntiXss;
using SessionMessages.Core;

namespace SessionMessages.Mvc
{
    /// <summary>
    /// If we're dealing with ajax requests, any message that is in the view data goes to
    /// the http header.
    /// </summary>
    public class MvcAjaxMessagesFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var response = filterContext.HttpContext.Response;
                List<SessionMessage> sessionMessages = SessionMessageManager.GetMessage();
                if (sessionMessages != null && sessionMessages.Count > 0)
                {
                    string json = null;
                    var messages = sessionMessages.Where(x => x.Behavior == MessageBehaviors.StatusBar).Select(x => new SessionMessageJsonModal { Message = AntiXssEncoder.HtmlEncode(x.Message,false), Type = Enum.GetName(typeof(MessageType), x.Type),Key=x.Key,Caption=x.Caption }).ToList();
                    if (messages != null && messages.Count > 0)
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(messages.GetType());
                        using (MemoryStream ms = new MemoryStream())
                        {
                            ser.WriteObject(ms, messages);
                            json = Encoding.Default.GetString(ms.ToArray());
                            ms.Close();
                        }
                        response.Headers.Add("X-Message", json);
                    }
                    messages = sessionMessages.Where(x => x.Behavior == MessageBehaviors.Modal).Select(x => new SessionMessageJsonModal { Message = x.Message, Type = Enum.GetName(typeof(MessageType), x.Type), Key=x.Key }).ToList();
                    if (messages != null && messages.Count > 0)
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(messages.GetType());
                        using (MemoryStream ms = new MemoryStream())
                        {
                            ser.WriteObject(ms, messages);
                            json = Encoding.Default.GetString(ms.ToArray());
                            ms.Close();
                        }
                        response.Headers.Add("X-ModalMessage", json);
                    }
                    SessionMessageManager.Clear();
                }
            }
        }
        [DataContract]
        private class SessionMessageJsonModal
        {
            [DataMember]
            public string Message
            {
                get;
                set;
            }
            [DataMember]
            public string Type
            {
                get;
                set;
            }
            [DataMember]
            public string Key
            {
                get;
                set;
            }
            [DataMember]
            public string Caption
            {
                get;
                set;
            }
        }
    }
    public class AjaxMessagesFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext filterContext)
        {
            var request = filterContext.Request;
            var headers = request.Headers;
            if (headers.Contains("X-Requested-With") && headers.GetValues("X-Requested-With").FirstOrDefault() == "XMLHttpRequest")
            {
                var response = filterContext.ActionContext.Response;
                if (response != null)
                {
                    List<SessionMessage> sessionMessages = SessionMessageManager.GetMessage();
                    if (sessionMessages != null && sessionMessages.Count > 0)
                    {
                        string json = null;
                        var messages = sessionMessages.Where(x => x.Behavior == MessageBehaviors.StatusBar).Select(x => new SessionMessageJsonModal { Message = AntiXssEncoder.HtmlEncode(x.Message,false), Type = Enum.GetName(typeof(MessageType), x.Type).ToLowerInvariant(), Key = x.Key }).ToList();
                        if (messages != null && messages.Count > 0)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(messages.GetType());
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ser.WriteObject(ms, messages);
                                json = Encoding.Default.GetString(ms.ToArray());
                                ms.Close();
                            }
                            response.Headers.Add("X-Message", json);
                        }
                        messages = sessionMessages.Where(x => x.Behavior == MessageBehaviors.Modal).Select(x => new SessionMessageJsonModal { Message = x.Message, Type = Enum.GetName(typeof(MessageType), x.Type).ToLowerInvariant(), Key = x.Key }).ToList();
                        if (messages != null && messages.Count > 0)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(messages.GetType());
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ser.WriteObject(ms, messages);
                                json = Encoding.Default.GetString(ms.ToArray());
                                ms.Close();
                            }
                            response.Headers.Add("X-ModalMessage", json);
                        }
                        SessionMessageManager.Clear();
                    }
                }
            }
        }
        [DataContract]
        private class SessionMessageJsonModal
        {
            [DataMember]
            public string Message
            {
                get;
                set;
            }
            [DataMember]
            public string Type
            {
                get;
                set;
            }
            [DataMember]
            public string Key
            {
                get;
                set;
            }
        }
    }
}