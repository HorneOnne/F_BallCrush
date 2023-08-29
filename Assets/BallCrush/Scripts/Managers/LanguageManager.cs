using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace BallCrush
{

    public class LanguageManager : MonoBehaviour
    {
        public static LanguageManager Instance { get; private set; }
        public static System.Action OnLanguageChanged;

        public TMP_FontAsset NormalFont;
        public TMP_FontAsset RusFont;

        private Dictionary<string, WordDict> dict = new Dictionary<string, WordDict>()
        {
            {"PLAY", new WordDict("PLAY", "SPILLE", "GIOCARE", "SPIELEN")},
            {"SETTINGS", new WordDict("SETTINGS", "INNSTILLINGER", "IMPOSTAZIONI", "EINSTELLUNGEN")},
            {"LANGUAGE", new WordDict("LANGUAGE", "SPRÅK", "LINGUA", "SPRACHE")},
            {"SOUND", new WordDict("SOUND", "LYD", "SUONO", "KLANG")},
            {"MUSIC", new WordDict("MUSIC", "MUSIKK", "MUSICA", "MUSIK")},
            {"BACK", new WordDict("BACK", "TILBAKE", "INDIETRO", "ZURÜCK")},
            {"PAUSE", new WordDict("PAUSE", "PAUSE", "PAUSE", "PAUSE")},
            {"HOME", new WordDict("HOME", "HJEM", "CASA", "HEIM")},
            {"GAME\nOVER", new WordDict("GAME\nOVER", "SPILL\nOVER", "GIOCO\nSOPRA", "SPIEL\nÜBER")},
            {"SCORE", new WordDict("SCORE", "SCORE", "PUNTO", "PUNKTZAHL")},
            {"RECORD", new WordDict("RECORD", "TA OPP", "DOCUMENTAZIONE", "AUFZEICHNEN")},
  
        };


        public enum Languague
        {
            English,
            German,
            Italian,
            Norwegian
        }

        public Languague CurrentLanguague;


        private void Awake()
        {
            // Check if an instance already exists, and destroy the duplicate
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            // Make the GameObject persist across scenes
            DontDestroyOnLoad(this.gameObject);
        }


        public void ChangeLanguague(Languague languague)
        {
            this.CurrentLanguague = languague;
            OnLanguageChanged?.Invoke();
        }

        public string GetWord(Languague type, string word)
        {
            if (dict.ContainsKey(word))
            {
                return dict[word].GetWord(type);
            }
            return "";
        }
    }

    public class WordDict
    {
        public string English;
        public string German;
        public string Italian;
        public string Norwegian;

        public WordDict(string english, string norwegian, string italian, string german)
        {
            English = english;
            German = german;
            Italian = italian;
            Norwegian = norwegian;
        }

        public string GetWord(LanguageManager.Languague language)
        {
            switch (language)
            {
                default:
                case LanguageManager.Languague.English:
                    return English;
                case LanguageManager.Languague.German:
                    return German;
                case LanguageManager.Languague.Italian:
                    return Italian;
                case LanguageManager.Languague.Norwegian:
                    return Norwegian;
            }
        }
    }
}
