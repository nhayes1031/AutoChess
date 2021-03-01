namespace Server.Game.Timeline {
    public interface IEvent {
        void OnEnter();
        bool Update(double time, double deltaTime);
        void OnExit();
    }
}
