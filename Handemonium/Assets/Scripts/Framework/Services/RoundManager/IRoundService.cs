using System;

namespace RPSLS.Framework.Services
{
    public interface IRoundService : IService
    {
        RoundState RoundState { get; }
        void SwitchState(RoundState state);
        
        void AddListener(Action<RoundState, RoundState> listener);
        void RemoveListener(Action<RoundState, RoundState> listener);
    }
    
    public enum RoundState
    {
        Idle,       // player not in-game
        Start,      // round timer started
        End,        // round timer up
        Result,     // post animations result displayed => Win/Lose/Draw
        Exit        // [Unused] player lose => exit from game
    }
}