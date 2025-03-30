using System;
using System.Threading.Tasks;
using RPSLS.Framework;
using RPSLS.Game;
using UnityEngine;

namespace RPSLS.Player
{
    public abstract class BasePlayer : MonoBehaviour
    {
        [SerializeField] protected HandDisplay m_HandDisplay;
        [SerializeField] private Transform m_HitParticlesParent;

        public Gesture SelectedGesture { get; protected set; }
        private GestureType SelectedGestureType => SelectedGesture == null ? GestureType.None : SelectedGesture.GestureType;

        protected virtual void Awake()
        {
            ResetStats();
        }

        public void ResetStats()
        {
            SelectedGesture = InGameController.Instance.GameRules.EmptyHandGesture;
        }

        public abstract void MakeChoice(GestureType gestureType);

        public async Task ShowHand()
        {
            await m_HandDisplay.Extend(SelectedGestureType);
        }

        public async Task HideHand()
        {
            await m_HandDisplay.Retract();
        }

        public virtual void OnRoundBegin()
        {
        }

        public void OnHit(Gesture otherGesture)
        {
            Instantiate(otherGesture.HitParticles, m_HitParticlesParent);
        }
    }
}
