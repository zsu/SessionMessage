using System;
using System.Configuration;
namespace SessionMessages
{


    public class SessionMessageFactory : ISessionMessageFactory
    {
        private readonly Type _type;

        public SessionMessageFactory(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                typeName = "SessionMessages.CookieSessionMessageProvider,SessionMessage";

            _type = Type.GetType(typeName, true, true);
        }

        public SessionMessageFactory()
            : this(ConfigurationManager.AppSettings["sessionMessageFactoryTypeName"])
        {
        }

        public ISessionMessageProvider CreateInstance()
        {
            return Activator.CreateInstance(_type) as ISessionMessageProvider;
        }
    }
}