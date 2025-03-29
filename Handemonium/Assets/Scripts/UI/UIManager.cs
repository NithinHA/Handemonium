using UnityEngine;

namespace RPSLS.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private MainMenuPanel m_MainMenuPanel;
        [SerializeField] private InGamePanel m_InGamePanel;

        protected override void Start()
        {
            base.Start();
        }
    }
}