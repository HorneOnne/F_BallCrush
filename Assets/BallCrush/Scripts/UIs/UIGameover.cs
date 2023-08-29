using UnityEngine;
using UnityEngine.UI;

namespace BallCrush
{
    public class UIGameover : CustomCanvas
    {
        [Header("Buttons")]
        [SerializeField] private Button _homeBtn;
        [SerializeField] private Button _replayBtn;
        


        private void Start()
        {
            _replayBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                Loader.Load(Loader.Scene.GameplayScene);
            });

            _homeBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundType.Button, false);

                Loader.Load(Loader.Scene.MenuScene);            
            });
        }

        private void OnDestroy()
        {
            _replayBtn.onClick.RemoveAllListeners();
            _homeBtn.onClick.RemoveAllListeners();
        }
    }
}
