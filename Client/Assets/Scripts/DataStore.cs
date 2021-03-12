using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client {

    public class DataStore : MonoSingleton<DataStore> {
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        public List<int> playerPorts;
    }
}
