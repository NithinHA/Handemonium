using DG.Tweening;
using RPSLS.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSLS.UI
{
    public class MainMenuPanel : BaseUIPanel
    {
        [SerializeField] private Image m_PlayButtonImage;
        [SerializeField] private float m_PlayButtonAlphaDuration = .5f;
        [Space]
        [SerializeField] private TextMeshProUGUI m_HighscoreText;
        [Header("Music button")]
        [SerializeField] private Image m_MusicImage;
        [SerializeField] private Sprite m_MusicOn;
        [SerializeField] private Sprite m_MusicOff;

        private bool _isMusicOn = true;
        
        public override void Show()
        {
            base.Show();
            SetMusicButton();
            m_HighscoreText.text = $"{ServiceLocator.GetHighscoreService().GetHighscore()}";

            Color color = m_PlayButtonImage.color;
            color.a = 0;
            m_PlayButtonImage.color = color;
        }

        public override void Hide()
        {
            base.Hide();
            TogglePlayButtonInteraction(false);
        }

        public void DelayedEnablePlayButton()
        {
            Utility.ImageFadeEffect(m_PlayButtonImage, duration: m_PlayButtonAlphaDuration, easeMode: Ease.InSine,
                onComplete: () => TogglePlayButtonInteraction(true));
        }

#region On click callbacks

        public void OnClickPlay()
        {
            TogglePlayButtonInteraction(false);
            UIManager.Instance.OnButtonClick();
            MainMenuController.Instance.OnPlayClicked();
        }

        public void OnClickMusicToggle()
        {
            Sound sound = AudioManager.Instance?.GetSound(Constants.Audio.BGM);
            if (sound == null)
                return;

            _isMusicOn = !_isMusicOn;
            sound.Source.mute = !_isMusicOn;
            SetMusicButton();
            UIManager.Instance.OnButtonClick();
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