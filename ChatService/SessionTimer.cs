using System;
using System.Timers;

namespace Alek.ChatService
{
    public class SessionTimer : IDisposable
    {
        private const int SESSION_EXPIRE_INTERVAL = 30000;

        public Guid UserGuid { get; set; }
        public Timer Timer { get; }

        public event EventHandler<Guid> SessionExpired;

        public SessionTimer(Guid userGuid)
        {
            UserGuid = userGuid;
            Timer = new Timer(SESSION_EXPIRE_INTERVAL);
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Timer.Elapsed -= Timer_Elapsed;
            SessionExpired(this, this.UserGuid);
        }

        public void Dispose()
        {
            Timer.Elapsed -= Timer_Elapsed;
            SessionExpired(this, this.UserGuid);
        }
    }
}
