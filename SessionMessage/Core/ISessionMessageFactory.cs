namespace SessionMessage
{
    public interface ISessionMessageFactory
    {
        ISessionMessageProvider CreateInstance();
    }
}