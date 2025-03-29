using System.Collections.Generic;
using UnityEngine;

namespace RPSLS
{
    [CreateAssetMenu(fileName = "Rules", menuName = "Rules")]
    public class GameRules : ScriptableObject
    {
        public List<Gesture> AllGestures;
        private Dictionary<GestureType, Gesture> _gestureMap = new Dictionary<GestureType, Gesture>();
        
        private void InitGestureMap()
        {
            foreach (Gesture gesture in AllGestures)
            {
                _gestureMap.Add(gesture.GestureType, gesture);
            }
        }
        
        public Gesture GetGestureFromType(GestureType gestureType)
        {
            if (_gestureMap == null)
                InitGestureMap();
            return _gestureMap[gestureType];
        }

        public Gesture GetWinner(Gesture g1, Gesture g2)
        {
            if (g1.GestureType == g2.GestureType)
                return null;

            if (g1.Beats(g2.GestureType) || g2.GestureType == GestureType.None) return g1;
            if (g2.Beats(g1.GestureType) || g1.GestureType == GestureType.None) return g2;

            return null;
        }
    }
}
