
namespace RPSLS.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public MainMenuPanel MainMenuPanel;
        public InGamePanel InGamePanel;

        public void OnButtonClick()
        {
            AudioManager.Instance?.PlaySound(Constants.Audio.CLICK);
        }
    }
}