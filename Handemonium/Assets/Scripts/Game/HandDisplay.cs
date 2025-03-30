using System.Collections.Generic;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RPSLS.Game
{
    public class HandDisplay : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<GestureType, GameObject> m_GestureHands;
        [SerializeField] private float m_MoveDuration = 1f;
        [SerializeField] private float m_MoveDistance = 10f;
        [Space]
        [SerializeField] 

        private GameObject _selectedHand;
        private Vector2 _originalPos;
        private Vector2 _targetPos;
        private float _threshold = 1f;

        private void Start()
        {
            _originalPos = transform.position;
            _targetPos = (Vector2)transform.position + ((Vector2)transform.up * m_MoveDistance);
        }

        public async Task Extend(GestureType gestureType)
        {
            Reset();            // just in case. Not mandatory. 
            _selectedHand = m_GestureHands[gestureType];
            _selectedHand.SetActive(true);
            await TranslateSourceToDest(_originalPos, _targetPos);
        }

        public async Task Retract()
        {
            await TranslateSourceToDest(_targetPos, _originalPos);
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

        private async Task TranslateSourceToDest(Vector2 source, Vector2 target)
        {
            transform.position = source;
            await transform.DOMove(target, m_MoveDuration).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
        
        void RandomMovement()
        {
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0);
            transform.DOMove(transform.position + randomOffset, 1f)
                .SetEase(Ease.InOutSine)
                .OnComplete(RandomMovement); // Recursively call for continuous movement
        }
    }
}