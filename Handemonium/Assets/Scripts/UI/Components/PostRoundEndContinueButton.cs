using AYellowpaper.SerializedCollections;
using RPSLS.Framework;
using RPSLS.Player;
using UnityEngine;
using UnityEngine.UI;

namespace RPSLS.UI.Component
{
    public class PostRoundEndContinueButton : MonoBehaviour
    {
        [SerializeField] private Text m_ResultText;
        [SerializeField] private SerializedDictionary<RoundResult, string> m_RoundResultsMap; 
        
        public void Setup(RoundResult result)
        {
            gameObject.SetActive(true);
            m_RoundResultsMap.TryGetValue(result, out string resultString);
            m_ResultText.text = resultString;
        }
        
        public void OnClickContinue()
        {
            gameObject.SetActive(false);
            InGameController.Instance.OnPostRoundEndContinue();
        }
    }
}