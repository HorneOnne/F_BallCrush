using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace BallCrush
{
    public class UILanguage : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _englishBtn;
        [SerializeField] private Button _germanBtn;
        [SerializeField] private Button _italianBtn;
        [SerializeField] private Button _norwegianBtn;



        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _languageText;
        [SerializeField] private TextMeshProUGUI _backBtnText;




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
            LoadLanguageFlagUI();

            _backBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.CloseAll();
                UIManager.Instance.DisplayMainMenu(true);
                SoundManager.Instance.PlaySound(SoundType.Button, false);
            });

            _englishBtn.onClick.AddListener(() =>
            {
                LanguageManager.Instance.ChangeLanguague(LanguageManager.Languague.English);
                LoadLanguageFlagUI();
            });

            _germanBtn.onClick.AddListener(() =>
            {
                LanguageManager.Instance.ChangeLanguague(LanguageManager.Languague.German);
                LoadLanguageFlagUI();
            });

            _italianBtn.onClick.AddListener(() =>
            {
                LanguageManager.Instance.ChangeLanguague(LanguageManager.Languague.Italian);
                LoadLanguageFlagUI();
            });

            _norwegianBtn.onClick.AddListener(() =>
            {
                LanguageManager.Instance.ChangeLanguague(LanguageManager.Languague.Norwegian);
                LoadLanguageFlagUI();
            });

        }

        private void OnDestroy()
        {
            _backBtn.onClick.RemoveAllListeners();

            _englishBtn.onClick.RemoveAllListeners();
            _germanBtn.onClick.RemoveAllListeners();
            _italianBtn.onClick.RemoveAllListeners();
            _norwegianBtn.onClick.RemoveAllListeners();
        }



        private void LoadLanguague()
        {
            _languageText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "LANGUAGE");
            _backBtnText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "BACK");           
        }

        private void LoadLanguageFlagUI()
        {
            switch(LanguageManager.Instance.CurrentLanguague)
            {
                case LanguageManager.Languague.English:
                    ChangeAlpha(_englishBtn.image, 1.0f);
                    ChangeAlpha(_germanBtn.image, 0.3f);
                    ChangeAlpha(_italianBtn.image, 0.3f);
                    ChangeAlpha(_norwegianBtn.image, 0.3f);
                    break;
                case LanguageManager.Languague.German:
                    ChangeAlpha(_englishBtn.image, 0.3f);
                    ChangeAlpha(_germanBtn.image, 1.0f);
                    ChangeAlpha(_italianBtn.image, 0.3f);
                    ChangeAlpha(_norwegianBtn.image, 0.3f);
                    break;
                case LanguageManager.Languague.Italian:
                    ChangeAlpha(_englishBtn.image, 0.3f);
                    ChangeAlpha(_germanBtn.image, 0.3f);
                    ChangeAlpha(_italianBtn.image, 1.0f);
                    ChangeAlpha(_norwegianBtn.image, 0.3f);
                    break;
                case LanguageManager.Languague.Norwegian:
                    ChangeAlpha(_englishBtn.image, 0.3f);
                    ChangeAlpha(_germanBtn.image, 0.3f);
                    ChangeAlpha(_italianBtn.image, 0.3f);
                    ChangeAlpha(_norwegianBtn.image, 1.0f);
                    break;
                default:break;
            }
        }

        private void ChangeAlpha(Image image, float targetAlpha)
        {
            Color currentColor = image.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
            image.color = newColor;
        }
    }
}
