using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace BallCrush
{
    public class UIMainMenu : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _settingsBtn;
        [SerializeField] private Button _languageBtn;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _playBtnText;
        [SerializeField] private TextMeshProUGUI _settingsBtnText;
        [SerializeField] private TextMeshProUGUI _languageBtnText;



        private void OnEnable()
        {
            LanguageManager.OnLanguageChanged += LoadLanguague;
        }

        private void OnDisable()
        {
            LanguageManager.OnLanguageChanged -= LoadLanguague;
        }

        private void Start()
        {
            LoadLanguague();

            _playBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                Loader.Load(Loader.Scene.GameplayScene);
            });

            _settingsBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                UIManager.Instance.CloseAll();
                UIManager.Instance.DisplaySettingsMenu(true);
            });

            _languageBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                UIManager.Instance.CloseAll();
                UIManager.Instance.DisplayLanguageMenu(true);
            });
        }

        private void OnDestroy()
        {
            _playBtn.onClick.RemoveAllListeners();
            _settingsBtn.onClick.RemoveAllListeners();
            _languageBtn.onClick.RemoveAllListeners();
        }



        private void LoadLanguague()
        {

            switch(LanguageManager.Instance.CurrentLanguague)
            {
                default:
                case LanguageManager.Languague.English:
                    _playBtnText.fontSize = 70;
                    _settingsBtnText.fontSize = 70;
                    _languageBtnText.fontSize = 70;
                    break;
                case LanguageManager.Languague.Norwegian:
                case LanguageManager.Languague.Italian:
                case LanguageManager.Languague.German:
                    _playBtnText.fontSize = 55;
                    _settingsBtnText.fontSize = 55;
                    _languageBtnText.fontSize = 55;
                    break;
            }


            _playBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "PLAY");
            _settingsBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "SETTINGS");
            _languageBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "LANGUAGE");
        }
    }
}
