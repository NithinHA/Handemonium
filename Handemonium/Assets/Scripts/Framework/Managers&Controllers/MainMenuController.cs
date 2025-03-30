using System.Threading.Tasks;
using RPSLS.Framework.Services;
using RPSLS.UI;
using UnityEngine;

namespace RPSLS.Framework
{
    public class MainMenuController : Singleton<MainMenuController>
    {
        [SerializeField] private GameObject m_EnvironmentMainMenu;

#region Unity callbacks
        
        protected override void Start()
        {
            base.Start();
            ServiceLocator.GetGameManager().AddListener(OnGameStateChanged);
        }

        protected override void OnDestroy()
        {
            ServiceLocator.GetGameManager()?.RemoveListener(OnGameStateChanged);
            base.OnDestroy();
        }

#endregion

        public void Setup()
        {
            m_EnvironmentMainMenu.SetActive(true);
            // await Animate-in hands
            UIManager.Instance.MainMenuPanel.Show();
        }

        public void Reset()
        {
            m_EnvironmentMainMenu.SetActive(false);
            UIManager.Instance.MainMenuPanel.Hide();
        }

#region Event listeners
        
        private void OnGameStateChanged(GameState prevState, GameState curState)
        {
            if (prevState == GameState.MainMenu && curState != GameState.MainMenu)
            {
                Reset();
            }
            else if (curState == GameState.MainMenu)
            {
                Setup();
            }
        }

        public async void OnPlayClicked()
        {
            // Animate-out hands
            // Perform screenTransition-> Animate-in-> invoke SwitchState(InGame)-> Animate-out 
            ServiceLocator.GetGameManager().SwitchState(GameState.InGame);
        }

#endregion
    }
}