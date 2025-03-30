using System;
using RPSLS.Framework;
using RPSLS.Framework.Services;
using TMPro;
using UnityEngine;

namespace RPSLS.UI
{
    public class InGamePanel : BaseUIPanel
    {
        [SerializeField] private TimeRemainProgressBar m_ProgressBar;
        [SerializeField] private TextMeshProUGUI m_ScoreDisplayText;
        [SerializeField] private PostRoundEndContinueButton m_PostRoundEndContinueButton;

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

        private Action<GestureType> _onGestureClick;

        public void OnClickGesture(int type)
        {
            GestureType gesture = (GestureType)type;
            _onGestureClick?.Invoke(gesture);
        }

        private void UpdatePlayerSelectedGesture(GestureType gestureType)
        {
            InGameController.Instance.PlayerRegistry.PlayerSelf.MakeChoice(gestureType);
        }

        private void HighlightInfoPanelOnTable(GestureType gestureType)
        {
            Debug.Log($"{gestureType} clicked!");
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
            // set button callbacks to perform Player.SelectGesture
            _onGestureClick = UpdatePlayerSelectedGesture;
        }

        private void OnRoundEnd()
        {
            // set button callbacks to perform InfoTable.SelectGesture(gesture);
            _onGestureClick = HighlightInfoPanelOnTable;
        }

        private void OnRoundResult()
        {
            _onGestureClick = HighlightInfoPanelOnTable;
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