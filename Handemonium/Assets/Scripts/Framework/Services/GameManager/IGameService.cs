using System;
using System.Collections.Generic;

namespace RPSLS.Framework.Services
{
    public interface IGameService : IService
    {
        GameState GameState { get; }
        
        void SwitchState(GameState state, Dictionary<object, object> payload = null);
        
        void AddListener(Action<GameState, GameState, Dictionary<object, object>> listener);
        void RemoveListener(Action<GameState, GameState, Dictionary<object, object>> listener);
    }

    public enum GameState
    {
        Bootstrap, MainMenu, InGame
    }
}