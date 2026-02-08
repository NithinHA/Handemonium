using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPSLS.Framework.Services
{
    public class GameManager : IGameService
    {
        private GameState _gameState;
        public GameState GameState => _gameState;
        private static Action<GameState, GameState, Dictionary<object, object>> _onGameStateChanged;
        
#region Default callbacks

        public void Start()
        {
            AddListener(OnGameStateChanged);
        }

        public void OnDestroy()
        {
            RemoveListener(OnGameStateChanged);
        }

#endregion

        public void SwitchState(GameState state, Dictionary<object, object> payload = null)
        {
            GameState prev = _gameState;
            _gameState = state;
            _onGameStateChanged?.Invoke(prev, _gameState, payload);
        }

        public void AddListener(Action<GameState, GameState, Dictionary<object, object>> listener)
        {
            _onGameStateChanged += listener;
        }

        public void RemoveListener(Action<GameState, GameState, Dictionary<object, object>> listener)
        {
            _onGameStateChanged -= listener;
        }

#region Event listeners

        private void OnGameStateChanged(GameState prevState, GameState curState, Dictionary<object, object> payload = null)
        {
            if (prevState == GameState.Bootstrap)
                AudioManager.Instance?.PlaySound(Constants.Audio.BGM);
        }

#endregion
    }
}