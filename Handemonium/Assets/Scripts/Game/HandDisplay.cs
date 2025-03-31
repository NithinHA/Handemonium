using System.Collections.Generic;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPSLS.Game
{
    public class HandDisplay : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<GestureType, GameObject> m_GestureHands;
        [Header("Translation settings")]
        [SerializeField] private float m_ExtendDuration = 1f;
        [SerializeField] private float m_RetractDuration = 1f;
        [SerializeField] private float m_MoveDistance = 10f;
        [Header("Float settings")]
        [SerializeField] private float m_FloatDuration = 5;
        [SerializeField] private Vector2 m_FloatStrengthRange = new Vector2(.02f, .06f);
        [SerializeField] private float m_FloatRandomness = 8;
        [SerializeField] private int m_FloatVibrato = 1;

        private GameObject _selectedHand;
        private Vector2 _originalPos;
        private Vector2 _targetPos;

        private Tween _floatTween = null;
        
        private void Awake()
        {
            _originalPos = transform.position;
            _targetPos = (Vector2)transform.position + ((Vector2)transform.up * m_MoveDistance);
        }

        public async Task Extend(GestureType gestureType)
        {
            Reset();            // just in case. Not mandatory. 
            _selectedHand = m_GestureHands[gestureType];
            _selectedHand.SetActive(true);
            await TranslateSourceToDest(_originalPos, _targetPos, m_ExtendDuration, Ease.OutBack);
            StartFloating();
        }

        public async Task Retract()
        {
            await TranslateSourceToDest(_targetPos, _originalPos, m_RetractDuration, Ease.InBack);
            StopFloating();
            _selectedHand.SetActive(false);
            _selectedHand = null;
        }

        private void Reset()
        {
            foreach (KeyValuePair<GestureType,GameObject> item in m_GestureHands)
            {
                item.Value.SetActive(false);
            }
        }

        private async Task TranslateSourceToDest(Vector2 source, Vector2 target, float duration, Ease easeMode = Ease.Linear)
        {
            transform.position = source;
            await transform.DOMove(target, duration).SetEase(easeMode).AsyncWaitForCompletion();
        }

#region Floating hand tween control
        
        private void StartFloating()
        {
            float floatStrength = Random.Range(m_FloatStrengthRange.x, m_FloatStrengthRange.y);
            _floatTween = _selectedHand.transform.DOShakePosition(m_FloatDuration, new Vector2(0, floatStrength), m_FloatVibrato, m_FloatRandomness, false, false)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StopFloating()
        {
            _floatTween?.Kill();
            _floatTween = null;
        }

#endregion

    }
}
