using RPSLS.Framework;
using RPSLS.Framework.Controllers;
using UnityEngine;

namespace RPSLS.Player
{
    /// <summary>
    /// Represents the remote opponent in a multiplayer game.
    /// Does not handle input - only receives choices from the network.
    /// </summary>
    public class RemotePlayer : BasePlayer
    {
        public override void MakeChoice(GestureType gestureType)
        {
            // RemotePlayer doesn't make choices locally
            // Choices are set via SetChoiceFromNetwork
        }

        /// <summary>
        /// Called by NetworkGameController to set the opponent's choice received from network
        /// </summary>
        public void SetChoiceFromNetwork(GestureType gestureType)
        {
            SelectedGesture = InGameController.Instance.GameRules.GetGestureFromType(gestureType);
            Debug.Log($"Remote player choice received: [{SelectedGesture}]".ToOlive());
        }

        public override void OnRoundBegin()
        {
            base.OnRoundBegin();
            // Remote player waits for network data, doesn't make choices locally
        }
    }
}
