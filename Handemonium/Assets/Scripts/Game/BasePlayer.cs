using UnityEngine;
using UnityEngine.Serialization;

namespace RPSLS.Game
{
    public class BasePlayer : MonoBehaviour
    {
        public Gesture SelectedGesture;
        [SerializeField] private HandDisplay m_HandDisplay; 

        public virtual void PromptInput()
        {
        }

        public virtual void MakeChoice()
        {
        }

        public void ShowHand()
        {
            m_HandDisplay.Show(SelectedGesture.GestureType);
        }
    }
}