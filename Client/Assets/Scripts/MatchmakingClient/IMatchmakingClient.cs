using System;

namespace Client.Matchmaking {
    public interface IMatchmakingClient {
        Action<bool> Connected { get; set; }
        Action GameFound { get; set; }
        Action<bool> Queued { get; set; }

        void Disconnect();
        void CancelMatchmakingRequest();
        void SendMatchmakingRequest();
    }
}
