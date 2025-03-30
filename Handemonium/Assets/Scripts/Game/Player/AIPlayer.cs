using RPSLS.AIStrategies;
using RPSLS.Framework;
using UnityEngine;

namespace RPSLS.Player
{
    public class AIPlayer : BasePlayer
    {
        private IAIStrategy _strategy;

        protected override void Awake()
        {
            _strategy = new AIStrategyPureRandom();
            // _strategy = new AIStrategyWeightedRandom();
        }
        
        public override void MakeChoice(GestureType gestureType = GestureType.None)
        {
            SelectedGesture = _strategy.ChooseGesture(InGameController.Instance.GameRules.AllGestures);
            Debug.Log($"AI player choice made => {SelectedGesture}".ToBrown());
        }

        public override void OnRoundBegin()
        {
            base.OnRoundBegin();
            MakeChoice();
        }

    }
}