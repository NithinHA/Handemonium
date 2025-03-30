using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RPSLS
{
    [CreateAssetMenu(fileName = "Symbol", menuName = "Gestures")]
    public class Gesture : ScriptableObject
    {
        public GestureType GestureType;
        public SerializedDictionary<GestureType, string> GestureAttackData; 
        [Space]
        public Sprite ButtonSprite;
        public GameObject HitParticles;
        
        private List<GestureType> _inferiorGestures;

        public bool Beats(GestureType other)
        {
            if (_inferiorGestures == null || _inferiorGestures.Count == 0)
                _inferiorGestures = GestureAttackData.Keys.ToList();

            return _inferiorGestures.Contains(other);
        }
    }

    [System.Serializable]
    public struct GestureAttackData
    {
        public GestureType Type;
        public string AttackName;
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