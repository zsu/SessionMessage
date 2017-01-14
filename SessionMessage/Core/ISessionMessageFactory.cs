namespace SessionMessages
{
    public interface ISessionMessageFactory
    {
        ISessionMessageProvider CreateInstance();
    }
}