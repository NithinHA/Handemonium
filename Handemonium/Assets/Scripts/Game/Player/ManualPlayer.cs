using RPSLS.Framework;
using UnityEngine;

namespace RPSLS.Player
{
    public class ManualPlayer : BasePlayer
    {
        public override void MakeChoice(GestureType gestureType)
        {
            SelectedGesture = InGameController.Instance.GameRules.GetGestureFromType(gestureType);
            Debug.Log($"Manual player choice made => {SelectedGesture}".ToBrown());
        }
    }
}