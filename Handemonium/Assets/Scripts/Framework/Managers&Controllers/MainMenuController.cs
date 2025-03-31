using System.Collections.Generic;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using RPSLS.Framework.Services;
using RPSLS.Game;
using RPSLS.UI;
using UnityEngine;

namespace RPSLS.Framework
{
    public class MainMenuController : Singleton<MainMenuController>
    {
        [SerializeField] private GameObject m_EnvironmentMainMenu;
        [SerializeField] private SerializedDictionary<GestureType, HandDisplay> m_AllHands;

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

        private async void Setup()
        {
            m_EnvironmentMainMenu.SetActive(true);
            UIManager.Instance.MainMenuPanel.Show();
            await ShowAllHands();
            UIManager.Instance.MainMenuPanel.DelayedEnablePlayButton();
        }

        private void Reset()
        {
            m_EnvironmentMainMenu.SetActive(false);
            UIManager.Instance.MainMenuPanel.Hide();
        }

        private async Task ShowAllHands()
        {
            foreach (KeyValuePair<GestureType, HandDisplay> hand in m_AllHands)
            {
                await hand.Value.Extend(hand.Key);
            }
        }

        private async Task HideAllHands()
        {
            List<Task> tasks = new List<Task>();
            foreach (KeyValuePair<GestureType, HandDisplay> hand in m_AllHands)
            {
                tasks.Add(hand.Value.Retract());
            }
            await Task.WhenAll(tasks);
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
            await HideAllHands();
            // Perform screenTransition-> Animate-in-> invoke SwitchState(InGame)-> Animate-out 
            ServiceLocator.GetGameManager().SwitchState(GameState.InGame);
        }

#endregion
    }
}