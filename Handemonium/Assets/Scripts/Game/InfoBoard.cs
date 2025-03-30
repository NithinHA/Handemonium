using UnityEngine;

namespace RPSLS.Game
{
    public class InfoBoard : MonoBehaviour
    {
        [SerializeField] private GestureBoardData[] m_GestureBoardData;

        public void Reset()
        {
            foreach (GestureBoardData item in m_GestureBoardData)
            {
                item.Deactivate();
            }
        }

        public void ToggleHighlight(bool active, Gesture gesture)
        {
            foreach (GestureBoardData item in m_GestureBoardData)
            {
                if (active)
                {
                    item.TryActivate(gesture);
                }
                else
                {
                    item.Deactivate();
                }
            }
        }
    }

    [System.Serializable]
    public struct GestureBoardData
    {
        public Gesture Gesture;
        public GameObject HighlightSuccess;
        public GameObject HighlightFailure;

        public void TryActivate(Gesture other)
        {
            if (Gesture.GestureType == other.GestureType)
            {
                // Draw; Do nothing
            }
            else if (Gesture.Beats(other.GestureType))      // implies, this Gesture can defeat the player.
            {
                HighlightFailure.SetActive(true);
            }
            else if (other.Beats(Gesture.GestureType))      // implies, player's Gesture can beat this.
            {
                HighlightSuccess.SetActive(true);
            }
        }

        public void Deactivate()
        {
            HighlightSuccess.SetActive(false);
            HighlightFailure.SetActive(false);
        }
    }
}