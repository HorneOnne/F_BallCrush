using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BallCrush
{
    public class UISettings : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _backBtn;


        [Header("Sliders")]
        [SerializeField] private SwitchSliderHandler _soundSlider;
        [SerializeField] private SwitchSliderHandler _musicSlider;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _settingsText;
        [SerializeField] private TextMeshProUGUI _soundText;
        [SerializeField] private TextMeshProUGUI _musicText;
        [SerializeField] private TextMeshProUGUI _backBtnText;



        private void OnEnable()
        {
            LanguageManager.OnLanguageChanged += LoadLanguague;

            _soundSlider.OnToggleClicked += OnToggleSound;
            _musicSlider.OnToggleClicked += OnToggleMusic;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= LoadLanguague;

            _soundSlider.OnToggleClicked -= OnToggleSound;
            _musicSlider.OnToggleClicked -= OnToggleMusic;
        }


        private void Start()
        {
            LoadLanguague();
            Debug.Log(SoundManager.Instance.isMusicActive);
            _soundSlider.ToggleOn = SoundManager.Instance.isSoundFXActive;
            _musicSlider.ToggleOn = SoundManager.Instance.isMusicActive;
            _soundSlider.UpdateUI();
            _musicSlider.UpdateUI();


            _backBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.CloseAll();
                UIManager.Instance.DisplayMainMenu(true);
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });
        }

        private void OnDestroy()
        {
            _backBtn.onClick.RemoveAllListeners();
        }

        private void OnToggleSound(bool isToggleOn)
        {
            SoundManager.Instance.isSoundFXActive = isToggleOn;
            SoundManager.Instance.MuteSoundFX(!SoundManager.Instance.isSoundFXActive);
        }


        private void OnToggleMusic(bool isToggleOn)
        {
            SoundManager.Instance.isMusicActive = isToggleOn;
            SoundManager.Instance.MuteBackground(!SoundManager.Instance.isMusicActive);
        }



        private void LoadLanguague()
        {
            switch (LanguageManager.Instance.CurrentLanguague)
            {
                default:
                case LanguageManager.Languague.English:
                    _settingsText.fontSize = 110;
                    _soundText.fontSize = 70;
                    _musicText.fontSize = 70;
                    _backBtnText.fontSize = 70;
                    break;
                case LanguageManager.Languague.Norwegian:
                case LanguageManager.Languague.Italian:
                case LanguageManager.Languague.German:
                    _settingsText.fontSize = 80;
                    _soundText.fontSize = 70;
                    _musicText.fontSize = 70;
                    _backBtnText.fontSize = 70;
                    break;
            }

            _settingsText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "SETTINGS");
            _soundText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "SOUND");
            _musicText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "MUSIC");
            _backBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "BACK");
        }
    }
}
