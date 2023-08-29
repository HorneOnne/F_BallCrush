using UnityEngine;

namespace BallCrush
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public UIMainMenu UIMainMenu;
        public UISettings UISettings;
        public UILanguage UILanguage;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CloseAll();
            DisplayMainMenu(true);
        }

        public void CloseAll()
        {
            DisplayMainMenu(false);
            DisplaySettingsMenu(false);
            DisplayLanguageMenu(false);
        }

        public void DisplayMainMenu(bool isActive)
        {
            UIMainMenu.DisplayCanvas(isActive);
        }

        public void DisplaySettingsMenu(bool isActive)
        {
            UISettings.DisplayCanvas(isActive);
        }

        public void DisplayLanguageMenu(bool isActive)
        {
            UILanguage.DisplayCanvas(isActive);
        }
    }
}
