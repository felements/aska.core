namespace aska.core.security.UserSession
{
    public interface IUserSessionStore
    {
        bool Store(string userName, string deviceId, bool dropOtherSessions = false);

        bool Get(string userName, string deviceId);

        void Drop(string userName, string deviceId);

        void DropAll(string userName);

        void DropAllExceptDevice(string username, string deviceIdException);
    }
}
