using System;
using DG.Tweening;
using RPSLS.Framework;
using RPSLS.Framework.Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace RPSLS.UI.Component
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    public class GestureCardButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Gesture m_Gesture;
        [Space]
        [SerializeField] private GameObject m_Highlight;
        [SerializeField] private Image m_GestureImage;
        [SerializeField] private TextMeshProUGUI m_GestureText;
        [Header("Hold settings")]
        [SerializeField] private float m_ScaleFactor = .2f;
        [SerializeField] private float m_MoveUpAmount = 5;
        [SerializeField] private float m_TransitionDuration = .4f;
        [Header("Float settings")]
        [SerializeField] private float m_FloatDuration = 5;
        [SerializeField] private Vector2 m_FloatStrengthRange = new Vector2(10, 15);
        [SerializeField] private float m_FloatRandomness = 8;
        [SerializeField] private int m_FloatVibrato = 1;

        private Canvas _canvas;
        private int _sortingOrderNormal;
        private int _sortingOrderSelected => _sortingOrderNormal + 1;

        private RectTransform _rectTransform;
        private Vector3 _originalPosition;
        private Vector3 _originalScale;

        private Tween _floatTween;
        private static Action<GestureType, RoundState> OnGestureButtonClickEvent;

#region Unity callbacks

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _sortingOrderNormal = _canvas.sortingOrder;
            m_GestureImage.sprite = m_Gesture.ButtonSprite;
            m_GestureText.text = m_Gesture.GestureType.ToString().ToUpper();
            
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
            _originalScale = _rectTransform.localScale;

            OnGestureButtonClickEvent += OnOtherGestureSelected;
        }

        private void OnEnable()
        {
            StartFloating();
        }

        private void OnDisable()
        {
            StopFloating();
            ToggleHighlight(false);
            // reset position and scale
            _rectTransform.anchoredPosition = _originalPosition;
            _rectTransform.localScale = _originalScale;
        }

        private void OnDestroy()
        {
            OnGestureButtonClickEvent -= OnOtherGestureSelected;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            StopFloating();
            _canvas.sortingOrder = _sortingOrderSelected;
            _rectTransform.DOAnchorPosY(_originalPosition.y + m_MoveUpAmount, m_TransitionDuration).SetEase(Ease.OutQuad);
            transform.DOScale(_originalScale * m_ScaleFactor, m_TransitionDuration).SetEase(Ease.OutBack);

            InGameController.Instance.GetInfoBoard.ToggleHighlight(true, m_Gesture);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _rectTransform.DOAnchorPosY(_originalPosition.y, m_TransitionDuration).SetEase(Ease.InQuad);
            transform.DOScale(_originalScale, m_TransitionDuration).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _canvas.sortingOrder = _sortingOrderNormal;
                    StartFloating();
                });

            InGameController.Instance.GetInfoBoard.ToggleHighlight(false, m_Gesture);
        }

#endregion

        private void StartFloating()
        {
            float floatStrength = Random.Range(m_FloatStrengthRange.x, m_FloatStrengthRange.y);
            _floatTween = _rectTransform.DOShakeAnchorPos(m_FloatDuration, new Vector2(0, floatStrength), m_FloatVibrato, m_FloatRandomness, false, false)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StopFloating()
        {
            _floatTween?.Kill();
            _floatTween = null;
        }


#region Event listeners

        public void OnGestureClick()
        {
            RoundState roundState = ServiceLocator.GetRoundManager().RoundState;
            OnGestureButtonClickEvent?.Invoke(m_Gesture.GestureType, roundState);
            switch (roundState)
            {
                case RoundState.Start:
                    InGameController.Instance.GetPlayerSelf.MakeChoice(m_Gesture.GestureType);
                    ToggleHighlight(true);
                    break;
                case RoundState.End:
                    break;
                case RoundState.Result:
                    break;
                case RoundState.Exit:
                    break;
            }
        }

        private void OnOtherGestureSelected(GestureType other, RoundState roundStateDuringClick)
        {
            if (other == m_Gesture.GestureType) return;

            if (roundStateDuringClick == RoundState.Start)
                ToggleHighlight(false);
        }

        public void OnRoundStart()
        {
            ToggleHighlight(false);
        }

#endregion

        private void ToggleHighlight(bool active)
        {
            m_Highlight.SetActive(active);
        }

    }
}