using RPSLS.Framework;
using RPSLS.UI.Component;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RPSLS.UI
{
    public class MainMenuPanel : BaseUIPanel
    {
        [SerializeField] private Image m_PlayButtonImage;
        [SerializeField] private MainMenuHighscoreDisplay m_MainMenuHighscore;
        [Header("Music button")]
        [SerializeField] private Image m_MusicImage;
        [SerializeField] private Sprite m_MusicOn;
        [SerializeField] private Sprite m_MusicOff;

        private bool _isMusicOn = true;
        
        public override void Show()
        {
            base.Show();
            Utility.ImageFadeEffect(m_PlayButtonImage, onComplete: () => TogglePlayButtonInteraction(true));
            Utility.ImageFadeEffect(m_MusicImage);
            m_MainMenuHighscore.Setup();
            SetMusicButton();
        }

        public override void Hide()
        {
            base.Hide();
            TogglePlayButtonInteraction(false);
            m_MainMenuHighscore.Reset();
        }

#region On click callbacks

        public void OnClickPlay()
        {
            TogglePlayButtonInteraction(false);
            MainMenuController.Instance.OnPlayClicked();
        }

        public void OnClickMusicToggle()
        {
            Sound audio = AudioManager.Instance.GetSound(Constants.Audio.BGM);
            if (audio == null)
                return;

            _isMusicOn = !_isMusicOn;
            audio.Source.mute = !_isMusicOn;
            SetMusicButton();
        }

#endregion

        private void TogglePlayButtonInteraction(bool active)
        {
            m_PlayButtonImage.raycastTarget = active;
        }

        private void SetMusicButton()
        {
            m_MusicImage.sprite = _isMusicOn ? m_MusicOn : m_MusicOff;
        }
    }
}