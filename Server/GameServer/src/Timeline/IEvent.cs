using Lidgren.Network;
using System.Collections.Generic;

namespace Server.Game.Timeline {
    public interface IEvent {
        void OnEnter(Dictionary<NetConnection, PlayerData> playerDatas);
        bool Update(double time, double deltaTime);
        void OnExit();
    }
}
