using System.Threading.Tasks;
using RPSLS.Framework.Services;
using RPSLS.UI;

namespace RPSLS.Framework
{
    public class MainMenuController : Singleton<MainMenuController>
    {

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
            // for each hand sprite, call Show()
            // call MainMenuPanle.Show()
            UIManager.Instance.MainMenuPanel.Show();
        }

        public async Task Reset()
        {
            // call Hide() on hands
            // hide UI
            UIManager.Instance.MainMenuPanel.Hide();
            await Task.Delay(1000); // dummy
        }

#region Event listeners
        
        private void OnGameStateChanged(GameState prevState, GameState curState)
        {
            if (curState == GameState.MainMenu)
            {
                Setup();
            }
        }

        public async void OnPlayClicked()
        {
            await Reset();
            // Perform screenTransition-> AnimateIn-> invoke SwitchScreenState()-> AnimateOut 
            ServiceLocator.GetGameManager().SwitchState(GameState.InGame);
        }

#endregion
    }
}