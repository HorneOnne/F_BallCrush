using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BallCrush
{
    public class UIGameplay : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _homeBtn;
        [SerializeField] private Button _replayBtn;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _moveText;



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


            _homeBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);
                
                Loader.Load(Loader.Scene.MenuScene);
            });

            _replayBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                Loader.Load(Loader.Scene.GameplayScene);
            });

        }

        private void OnDestroy()
        {
            _homeBtn.onClick.RemoveAllListeners();
            _replayBtn.onClick.RemoveAllListeners();
        }

   

        private void LoadLanguague()
        {
            //if (LanguageManager.Instance.CurrentLanguague == LanguageManager.Languague.Eng)
            //{
            //    _levelText.font = LanguageManager.Instance.NormalFont;
            //    _moveText.font = LanguageManager.Instance.NormalFont;

            //    _levelText.fontSize = 40;
            //    _moveText.fontSize = 40;
            //}
            //else if (LanguageManager.Instance.CurrentLanguague == LanguageManager.Languague.Ger)
            //{
            //    _levelText.font = LanguageManager.Instance.NormalFont;
            //    _moveText.font = LanguageManager.Instance.NormalFont;

            //    _levelText.fontSize = 35;
            //    _moveText.fontSize = 35;
            //}
            //else
            //{
            //    _levelText.font = LanguageManager.Instance.RusFont;
            //    _moveText.font = LanguageManager.Instance.RusFont;

            //    _levelText.fontSize = 27;
            //    _moveText.fontSize = 25;
            //}

            //_levelText.text = LanguageManager.Instance.GetWord(LanguageManager.Instance.CurrentLanguague, "LEVEL");
        }
    }
}
