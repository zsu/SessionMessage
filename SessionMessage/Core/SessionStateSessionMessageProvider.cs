/// Author: Zhicheng Su
using System.Collections.Generic;
using System.Web;

namespace SessionMessages
{
    public class SessionStateSessionMessageProvider:ISessionMessageProvider
    {
        private const string SessionMessageKey = "SessionMessage";
        #region ISessionMessageProvider Members

        public void SetMessage(SessionMessage message)
        {
            if (message == null)
                return;
            List<SessionMessage> messages = GetMessage();
            if (messages == null)
                messages = new List<SessionMessage>();
            if (!string.IsNullOrEmpty(message.Key) && messages.Exists(x => x.Key == message.Key && x.Behavior == message.Behavior))
                return;
            messages.Add(message);
            HttpContext.Current.Session[SessionMessageKey] = messages;
        }
        public List<SessionMessage> GetMessage()
        {
            List<SessionMessage> sessionMessages = HttpContext.Current.Session[SessionMessageKey] as List<SessionMessage>;
            return sessionMessages;
        }

        public void Clear()
        {
            HttpContext.Current.Session[SessionMessageKey] = null;
        }

        #endregion
    }
}
