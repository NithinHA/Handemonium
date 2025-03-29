using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RPSLS.Game
{
    public class HandDisplay : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<GestureType, GameObject> m_GestureHands;

        private GameObject _selectedHand;
        
        public void Show(GestureType gestureType)
        {
            Reset();
            _selectedHand = m_GestureHands[gestureType];
            _selectedHand.SetActive(true);
            AnimateHand();
        }

        private void Reset()
        {
            foreach (KeyValuePair<GestureType,GameObject> item in m_GestureHands)
            {
                item.Value.SetActive(false);
            }
        }

        private void AnimateHand()
        {
            
        }
    }
}