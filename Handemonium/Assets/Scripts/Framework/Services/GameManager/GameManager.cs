using System;
using UnityEngine;

namespace RPSLS.Framework.Services
{
    public class GameManager : IGameService
    {
        private GameState _gameState;
        public GameState GameState => _gameState;
        private static Action<GameState, GameState> _onGameStateChanged;
        
#region Default callbacks

        public void Start()
        {
        }

        public void OnDestroy()
        {
        }

#endregion

        public void SwitchState(GameState state)
        {
            GameState prev = _gameState;
            _gameState = state;
            _onGameStateChanged?.Invoke(prev, _gameState);
        }

        public void AddListener(Action<GameState, GameState> listener)
        {
            _onGameStateChanged += listener;
        }

        public void RemoveListener(Action<GameState, GameState> listener)
        {
            _onGameStateChanged -= listener;
        }
    }
}