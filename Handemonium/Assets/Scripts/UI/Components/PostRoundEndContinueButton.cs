using System;
using AYellowpaper.SerializedCollections;
using RPSLS.Framework;
using RPSLS.Player;
using TMPro;
using UnityEngine;

namespace RPSLS.UI
{
    public class PostRoundEndContinueButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_ResultText;
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