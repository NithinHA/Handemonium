using System.Collections;
using System.Collections.Generic;
using RPSLS.Framework.Services;
using RPSLS.Game;
using RPSLS.Player;
using Unity.Netcode;
using UnityEngine;

namespace RPSLS.Framework.Controllers
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkGameController : NetworkBehaviour
    {
        public static NetworkGameController Instance { get; private set; }

        [SerializeField] private GameRules _gameRules;
        public int RoundsToWin = 3;

        private Dictionary<ulong, GestureType> _playerChoices = new Dictionary<ulong, GestureType>();
        private Dictionary<ulong, int> _playerScores = new Dictionary<ulong, int>();
        private HashSet<ulong> _readyPlayers = new HashSet<ulong>();

        private void Awake()
        {
            Instance = this;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _playerScores.Clear();
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                
                // If we already have players (Host + Client connected fast?)
                CheckStartGame();
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                 NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            CheckStartGame();
        }

        private void CheckStartGame()
        {
            if (NetworkManager.Singleton.ConnectedClientsIds.Count >= 2)
            {
                // Both players connected. Move them to Game View.
                EnterGameClientRpc();
            }
        }

        [ClientRpc]
        private void EnterGameClientRpc()
        {
            Debug.Log("Both players connected. Entering Game.");
            Dictionary<object, object> payload = new Dictionary<object, object>()
            {
                {Constants.EventConstants.GAME_MODE, Constants.EventConstants.GAME_MODE_MULTIPLAYER}
            };
            ServiceLocator.GetGameManager().SwitchState(GameState.InGame, payload);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetPlayerReadyServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            if (_readyPlayers.Contains(clientId)) return;

            _readyPlayers.Add(clientId);
            Debug.Log($"Player {clientId} is Ready. Total: {_readyPlayers.Count}");

            if (_readyPlayers.Count >= 2)
            {
                _readyPlayers.Clear();
                StartCoroutine(StartRoundDelay(0.5f));
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetPlayerContinueReadyServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            if (_readyPlayers.Contains(clientId)) return;

            _readyPlayers.Add(clientId);
            Debug.Log($"Player {clientId} ready to continue. Total: {_readyPlayers.Count}");

            if (_readyPlayers.Count >= 2)
            {
                _readyPlayers.Clear();
                ContinueToNextRoundClientRpc();
            }
        }

        [ClientRpc]
        private void ContinueToNextRoundClientRpc()
        {
            // Both players are ready - continue to next round (multiplayer flow)
            InGameController.Instance.ContinueToNextRoundMultiplayer();
        }

        [ServerRpc(RequireOwnership = false)]
        public void NotifyPlayerLeavingServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            Debug.Log($"Player {clientId} is leaving the game");
            
            // Notify all clients that a player has left
            PlayerLeftClientRpc();
        }

        [ClientRpc]
        private void PlayerLeftClientRpc()
        {
            InGameController.Instance.ExitOnBackClick();
        }

        private IEnumerator StartRoundDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartRoundClientRpc();
        }

        [ClientRpc]
        private void StartRoundClientRpc()
        {
             ServiceLocator.GetRoundManager().SwitchState(RoundState.Start);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SubmitLocalPlayerChoiceServerRpc(GestureType gestureType, ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            _playerChoices[clientId] = gestureType;
            Debug.Log($"Received choice from client {clientId}: {gestureType}");

            SyncPlayerChoicesClientRpc(gestureType, clientId);
        }

        [ClientRpc]
        private void SyncPlayerChoicesClientRpc(GestureType gestureType, ulong clientId)
        {
            if (NetworkManager.Singleton.LocalClientId == clientId)     // We are only concerned about opponent's choice here.
                return;
            
            RemotePlayer remotePlayer = InGameController.Instance.GetPlayerOpponent as RemotePlayer;
            if (remotePlayer != null)
                remotePlayer.SetChoiceFromNetwork(gestureType);
        }
    }
}
