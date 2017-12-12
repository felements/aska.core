using System;
using System.Collections.Concurrent;
using System.Linq;

namespace aska.core.security.UserSession
{
    public class InMemoryUserSessionStore : IUserSessionStore
    {
        //TODO: move to configuration
        public static TimeSpan SessionLifetime = TimeSpan.FromDays(1); 
        

        private readonly Lazy<ConcurrentDictionary<string, SessionData>> _sessions = new Lazy<ConcurrentDictionary<string, SessionData>>(() => new ConcurrentDictionary<string, SessionData>());
        
        public bool Store(string userName, string deviceId, bool dropOtherSessions = false)
        {
            if (dropOtherSessions) DropAll(userName);

            var session = new SessionData(userName, deviceId);
            return _sessions.Value.TryAdd(session.ToString(), session);
        }

        public bool Get(string userName, string deviceId)
        {
            var sessionKey = new SessionData(userName, deviceId).ToString();

            SessionData dt;
            if (!_sessions.Value.TryGetValue(sessionKey, out dt)) return false;
            if (dt.IssueMomentUtc >= DateTime.UtcNow - SessionLifetime) return true;

            _sessions.Value.TryRemove(sessionKey, out dt);
            return false;
        }

        public void Drop(string username, string deviceId)
        {
            var key = new SessionData(username, deviceId).ToString();

            SessionData data;
            _sessions.Value.TryRemove(key, out data);
        }

        public void DropAll(string userName)
        {
            var uname = Normalize(userName);
            var keys = _sessions.Value.Keys.Where(x => x.StartsWith(uname));

            foreach (var key in keys)
            {
                SessionData dt;
                _sessions.Value.TryRemove(key, out dt);
            }
        }

        public void DropAllExceptDevice(string username, string deviceIdException)
        {
            var dt = new SessionData(username, deviceIdException);

            var keys = _sessions.Value.Keys.Where(x => x.StartsWith(dt.UserName)).Where(x=> !string.Equals(x, dt.ToString(), StringComparison.InvariantCultureIgnoreCase) );
            foreach (var key in keys)
            {
                SessionData tempDta;
                _sessions.Value.TryRemove(key, out tempDta);
            }
        }


        public static string Normalize(string credential)
        {
            return (credential ?? "").Trim().ToLowerInvariant().Limit(100);
        }

        public struct SessionData
        {
            public SessionData(string userName, string deviceId)
            {
                if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));
                if (string.IsNullOrWhiteSpace(deviceId)) throw new ArgumentNullException(nameof(deviceId));

                UserName = InMemoryUserSessionStore.Normalize(userName);
                DeviceId = InMemoryUserSessionStore.Normalize(deviceId);
                IssueMomentUtc = DateTime.UtcNow;

                _key = string.Format("{0}_{1}", UserName, DeviceId);
            }

            public string UserName { get; private set; }
            public string DeviceId { get; private set; }
            public DateTime IssueMomentUtc { get; private set; }
            private readonly string _key;

            public override string ToString()
            {
                return _key;
            }
        }
    }
}