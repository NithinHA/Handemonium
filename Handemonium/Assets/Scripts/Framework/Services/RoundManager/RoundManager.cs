using System;
using UnityEngine;

namespace RPSLS.Framework.Services
{
    public class RoundManager : IRoundService
    {
        private RoundState _roundState;
        public RoundState RoundState => _roundState;
        private static Action<RoundState, RoundState> _onRoundStateChanged;

#region Default callbacks

        public void Start()
        { }

        public void OnDestroy()
        { }

#endregion

        public void SwitchState(RoundState state)
        {
            RoundState prev = _roundState;
            _roundState = state;
            Debug.Log($"RoundManager switched => {prev} -> {state}".ToNavy());
            _onRoundStateChanged?.Invoke(prev, _roundState);
        }

        public void AddListener(Action<RoundState, RoundState> listener)
        {
            _onRoundStateChanged += listener;
        }

        public void RemoveListener(Action<RoundState, RoundState> listener)
        {
            _onRoundStateChanged -= listener;
        }
    }
}
