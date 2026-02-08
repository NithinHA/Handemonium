using Unity.Netcode;
using UnityEngine;

namespace RPSLS.Framework.Services
{
    /// <summary>
    /// A simple connection manager that uses direct IP connection (UnityTransport).
    /// Does not use Lobby or Relay services.
    /// </summary>
    public class MultiplayerConnectionManager : MonoBehaviour
    {
        public static MultiplayerConnectionManager Instance { get; private set; }

        private void Start()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnServerStarted += OnServerStarted;
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            }
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            }
        }

        private void OnServerStarted()
        {
            Debug.Log("MultiplayerConnectionManager: OnServerStarted. Waiting for players...");
        }

        private void OnClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                 Debug.Log("MultiplayerConnectionManager: Connected as Client. Waiting for Game Start...");
            }
        }

        private void OnClientDisconnect(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                Debug.Log("MultiplayerConnectionManager: Disconnected. Switching to GameState.MainMenu");
                ServiceLocator.GetGameManager()?.SwitchState(GameState.MainMenu);
            }
        }

        // Connects as Host (Server + Client) on 127.0.0.1 (default)
        public void CreateGame()
        {
            Debug.Log("Starting Host (Direct)...");
            NetworkManager.Singleton.StartHost();
        }

        // Connects as Client to 127.0.0.1 (default)
        // To connect to a different IP, you'd need to set UnityTransport.ConnectionData
        public void QuickJoinGame()
        {
            Debug.Log("Starting Client (Direct)...");
            NetworkManager.Singleton.StartClient();
        }
    }
}
