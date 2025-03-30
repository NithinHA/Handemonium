using System;
using RPSLS.Framework;
using RPSLS.Framework.Services;
using RPSLS.UI.Component;
using TMPro;
using UnityEngine;

namespace RPSLS.UI
{
    public class InGamePanel : BaseUIPanel
    {
        [SerializeField] private TimeRemainProgressBar m_ProgressBar;
        [SerializeField] private TextMeshProUGUI m_ScoreDisplayText;
        [Space]
        [SerializeField] private PostRoundEndContinueButton m_PostRoundEndContinueButton;
        [SerializeField] private GestureCardButton[] m_GestureButtons;

#region Unity callbacks
        
        private void Start()
        {
            ServiceLocator.GetRoundManager().AddListener(OnRoundStateChange);
            ScoreHandler.OnScoreUpdate += OnScoreUpdate;
        }

        private void OnDestroy()
        {
            ServiceLocator.GetRoundManager()?.RemoveListener(OnRoundStateChange);
            ScoreHandler.OnScoreUpdate -= OnScoreUpdate;
        }

#endregion

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            m_ProgressBar.Reset();
            m_ScoreDisplayText.text = "0";
        }

#region Event listeners
        
        private void OnRoundStateChange(RoundState prevState, RoundState curState)
        {
            switch (curState)
            {
                case RoundState.Start:
                    OnRoundBegin();
                    break;
                case RoundState.End:
                    OnRoundEnd();
                    break;
                case RoundState.Result:
                    OnRoundResult();
                    break;
            }
        }

        private void OnRoundBegin()
        {
            m_ProgressBar.OnRoundStarted();
            foreach (GestureCardButton gestureButton in m_GestureButtons)
            {
                gestureButton.OnRoundStart();
            }
        }

        private void OnRoundEnd()
        { }

        private void OnRoundResult()
        {
            // Display end screen -> Win/Lose/Draw with text "Tap to continue..."
            m_PostRoundEndContinueButton.Setup(InGameController.Instance.CurrentRoundResult);
        }

        private void OnScoreUpdate(int score)
        {
            m_ScoreDisplayText.text = score.ToString();
        }

#endregion
        
    }
}