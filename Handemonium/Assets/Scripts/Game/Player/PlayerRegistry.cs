using System.Collections.Generic;
using System.Threading.Tasks;
using RPSLS.Framework;
using RPSLS.Framework.Services;
using RPSLS.Game;
using UnityEngine;

namespace RPSLS.Player
{
    public class PlayerRegistry : MonoBehaviour
    {
        public BasePlayer PlayerSelf;
        public BasePlayer PlayerOpponent;

#region Unity callbacks

        private void Start()
        {
            ServiceLocator.GetRoundManager().AddListener(OnRoundStateChange);
        }

        private void OnDestroy()
        {
            ServiceLocator.GetRoundManager()?.RemoveListener(OnRoundStateChange);
        }

#endregion
        
        public void Setup() { }

        public void Reset()
        {
            PlayerSelf.ResetStats();
            PlayerOpponent.ResetStats();
        }
        
        private async Task ShowPlayerHands()
        {
            List<Task> tasks = new List<Task>
            {
                PlayerSelf.ShowHand(),
                PlayerOpponent.ShowHand()
            };
            await Task.WhenAll(tasks);
            await Task.Delay(400);  // a small delay

            InGameController.Instance.ComputeResult();
        }

        public async Task HidePlayerHands()
        {
            List<Task> tasks = new List<Task>
            {
                PlayerSelf.HideHand(),
                PlayerOpponent.HideHand()
            };
            await Task.WhenAll(tasks);
        }

        public RoundResultDesc GetRoundResult()
        {
            Gesture selfGesture = PlayerSelf.SelectedGesture;
            Gesture opponentGesture = PlayerOpponent.SelectedGesture;
            Gesture result = InGameController.Instance.GameRules.GetWinner(selfGesture, opponentGesture);

            if (result == selfGesture)
                return new RoundResultDesc(selfGesture, opponentGesture, RoundResultState.Win);
            if (result == opponentGesture)
                return new RoundResultDesc(opponentGesture, selfGesture, RoundResultState.Lose);
            return new RoundResultDesc(selfGesture, opponentGesture, RoundResultState.Draw);
        }

#region Event listeners

        private void OnRoundStateChange(RoundState prevState, RoundState curState)
        {
            switch (curState)
            {
                case RoundState.Start:
                    OnRoundBegin(prevState, curState);
                    break;
                case RoundState.End:
                    OnRoundEnd(prevState, curState);
                    break;
                case RoundState.Result:
                    OnRoundResult(prevState, curState);
                    break;
            }
        }
        
        private void OnRoundBegin(RoundState prevState, RoundState curState)
        {
            PlayerSelf.OnRoundBegin();
            PlayerOpponent.OnRoundBegin();
        }

        private void OnRoundEnd(RoundState prevState, RoundState curState)
        {
            Task task = ShowPlayerHands();
        }

        private void OnRoundResult(RoundState prevState, RoundState curState)
        {
            switch (InGameController.Instance.CurrentRoundResult.Result)
            {
                case RoundResultState.Win:
                    PlayerOpponent.OnHit(PlayerSelf.SelectedGesture);
                    break;
                case RoundResultState.Lose:
                    PlayerSelf.OnHit(PlayerOpponent.SelectedGesture);
                    break;
                case RoundResultState.Draw:
                    break;
            }
        }
        
#endregion

    }
}
