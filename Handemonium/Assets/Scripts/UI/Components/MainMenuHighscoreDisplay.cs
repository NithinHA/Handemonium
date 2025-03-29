using RPSLS.Framework;
using RPSLS.Framework.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSLS.UI
{
    public class MainMenuHighscoreDisplay : MonoBehaviour
    {
        [SerializeField] private Image m_BgImage;
        [SerializeField] private Image m_Divider;
        [SerializeField] private TextMeshProUGUI m_Title;
        [SerializeField] private TextMeshProUGUI m_Amount;

        public void Setup()
        {
            m_Amount.text = $"{ServiceLocator.GetHighscoreService().GetHighscore()}";
            Utility.ImageFadeEffect(m_BgImage);
            Utility.ImageFadeEffect(m_Divider, onComplete: OnTweenComplete);
        }

        public void Reset()
        {
            m_Title.gameObject.SetActive(false);
            m_Amount.gameObject.SetActive(false);
        }

        private void OnTweenComplete()
        {
            m_Title.gameObject.SetActive(true);
            m_Amount.gameObject.SetActive(true);
        }
    }
}