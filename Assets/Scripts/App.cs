using UnityEngine;

namespace ExpernetVR
{
    public class App : MonoBehaviour
    {
        public string roomName;
        public string jwt;
        public string username;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

