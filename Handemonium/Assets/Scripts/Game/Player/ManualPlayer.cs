using RPSLS.Framework;
using UnityEngine;

namespace RPSLS.Player
{
    public class ManualPlayer : BasePlayer
    {
        public override void MakeChoice(GestureType gestureType)
        {
            SelectedGesture = InGameController.Instance.GameRules.GetGestureFromType(gestureType);
            Debug.Log($"Player choice: [{SelectedGesture}]".ToGreen());
            
            // If in multiplayer, send choice to server
            if (Unity.Netcode.NetworkManager.Singleton != null && 
                Unity.Netcode.NetworkManager.Singleton.IsListening)
            {
                var controller = RPSLS.Framework.Controllers.NetworkGameController.Instance;
                if (controller != null)
                {
                    controller.SubmitLocalPlayerChoiceServerRpc(gestureType);
                }
            }
        }
    }
}