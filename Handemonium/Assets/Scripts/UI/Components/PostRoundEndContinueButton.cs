using AYellowpaper.SerializedCollections;
using RPSLS.Framework;
using RPSLS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace RPSLS.UI.Component
{
    public class PostRoundEndContinueButton : MonoBehaviour
    {
        [SerializeField] private Text m_ResultDesc;
        [SerializeField] private Text m_ResultText;
        [SerializeField] private SerializedDictionary<RoundResultState, string> m_RoundResultsMap; 
        
        public void Setup(RoundResultDesc result)
        {
            gameObject.SetActive(true);
            m_RoundResultsMap.TryGetValue(result.Result, out string resultString);
            m_ResultText.text = resultString;

            m_ResultDesc.text = string.Empty;
            if (result.Winner.GestureType == GestureType.None || result.Loser.GestureType == GestureType.None)
                return;
            if (result.Result == RoundResultState.Draw)
                return;

            string attackName = result.Winner.GestureAttackData[result.Loser.GestureType];
            attackName = result.Result == RoundResultState.Win ? 
                attackName.ToLime() : 
                attackName.ToOrange();
            m_ResultDesc.text = $"{result.Winner.GestureType}\n{attackName}\n{result.Loser.GestureType}";
        }

        public void Reset()
        {
            gameObject.SetActive(false);
        }

        public void OnClickContinue()
        {
            InGameController.Instance.OnPostRoundEndContinue();
            Reset();
        }
    }
}