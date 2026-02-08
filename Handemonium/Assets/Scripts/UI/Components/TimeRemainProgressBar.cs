using System;
using DG.Tweening;
using RPSLS.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace RPSLS.UI.Component
{
    public class TimeRemainProgressBar : MonoBehaviour
    {
        [SerializeField] private Image m_FillImage;
        
        private InGameController _inGameController = null;
        private Tween _activeTween;

        private void Start()
        {
            _inGameController = InGameController.Instance;
        }

        public void OnRoundStarted()
        {
            Reset();
            StartTicking();
        }

        private void StartTicking()
        {
            if (_inGameController == null)
                _inGameController = InGameController.Instance;

            float duration = _inGameController.DecisionTimerInSeconds;
            _activeTween = m_FillImage.DOFillAmount(0f, duration).SetEase(Ease.OutSine).OnComplete(() =>
            {
                _inGameController.OnTimerComplete();
            });
        }

        public void Reset()
        {
            m_FillImage.fillAmount = 1f;
            if (_activeTween.IsActive())
                _activeTween.Kill();
        }
    }
}