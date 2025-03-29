
namespace RPSLS.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public MainMenuPanel MainMenuPanel;
        public InGamePanel InGamePanel;

        protected override void Start()
        {
            base.Start();
        }
    }
}