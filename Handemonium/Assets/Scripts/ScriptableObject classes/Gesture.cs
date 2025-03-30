using System.Collections.Generic;
using UnityEngine;

namespace RPSLS
{
    [CreateAssetMenu(fileName = "Symbol", menuName = "Gestures")]
    public class Gesture : ScriptableObject
    {
        public GestureType GestureType;
        public List<GestureType> InferiorGestures = new List<GestureType>();
        public Sprite ButtonSprite;
        public GameObject HitParticles;

        public bool Beats(GestureType other)
        {
            return InferiorGestures.Contains(other);
        }
    }

    [System.Serializable]
    public enum GestureType
    {
        None = 0,
        Rock = 1,
        Paper = 2,
        Scissor = 3,
        Lizard = 4,
        Spock = 5
    }
}