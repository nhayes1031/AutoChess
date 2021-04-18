using System.Collections.Generic;

namespace Client {
    public class DataStore : MonoSingleton<DataStore> {
        public List<int> playerPorts;

        private void Awake() {
            DontDestroyOnLoad(this);
        }
    }
}
