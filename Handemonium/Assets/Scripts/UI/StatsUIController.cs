using RPSLS.Framework;
using TMPro;
using UnityEngine;

namespace RPSLS.UI
{
    public class StatsUIController : MonoBehaviour
    {
        [Header("Stats UI References")]
        [SerializeField] private TextMeshProUGUI m_WinsText;
        [SerializeField] private TextMeshProUGUI m_LossesText;
        [SerializeField] private TextMeshProUGUI m_BestScoreText;

        private void Start()
        {
            UpdateStatsUI();
        }

        private void OnEnable()
        {
            UpdateStatsUI();
        }

        public void UpdateStatsUI()
        {
            var stats = ServiceLocator.GetStatsManager()?.GetStats();
            if (stats != null)
            {
                if (m_WinsText) m_WinsText.text = $"Wins: {stats.Wins}";
                if (m_LossesText) m_LossesText.text = $"Losses: {stats.Losses}";
                if (m_BestScoreText) m_BestScoreText.text = $"Best Score: {stats.BestScore}";
            }
        }
    }
}
