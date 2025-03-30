using RPSLS.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSLS.UI
{
    public class MainMenuHighscoreDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject m_Container;
        [SerializeField] private Image m_BgImage;
        [SerializeField] private TextMeshProUGUI m_Amount;

        public void Setup()
        {
            m_Amount.text = $"{ServiceLocator.GetHighscoreService().GetHighscore()}";
            Utility.ImageFadeEffect(m_BgImage, onComplete: OnTweenComplete);
        }

        public void Reset()
        {
            m_Container.SetActive(false);
        }

        private void OnTweenComplete()
        {
            m_Container.SetActive(true);
        }
    }
}