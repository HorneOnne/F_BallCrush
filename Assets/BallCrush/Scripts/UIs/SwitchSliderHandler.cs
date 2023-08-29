using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace BallCrush
{
    public class SwitchSliderHandler : MonoBehaviour
    {
        public event Action<bool> OnToggleClicked;

        [SerializeField] private Button _switchBtn;
        [SerializeField] private Image _backgroundImage;

        [Header("Sprites")]
        [SerializeField] private Sprite _onImageBackground;
        [SerializeField] private Sprite _offImageBackground;

        public bool ToggleOn = true;


        // Cached
        private float _offsetToggleX;

        private void Awake()
        {
            _offsetToggleX = _switchBtn.transform.localPosition.x;
        }
        private void OnEnable()
        {
            _switchBtn.onClick.AddListener(OnSwitchButtonClicked);
        }

        private void OnDisable()
        {
            _switchBtn.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            UpdateUI();
        }

        public void OnSwitchButtonClicked()
        {
            ToggleOn = !ToggleOn;

            UpdateUI();
            OnToggleClicked?.Invoke(ToggleOn);
        }

        public void UpdateUI()
        {
            if (ToggleOn)
            {         
                Vector3 targetPosition = new Vector3(_offsetToggleX, _switchBtn.transform.localPosition.y, _switchBtn.transform.localPosition.z);
                _switchBtn.transform.localPosition = targetPosition;

                _backgroundImage.sprite = _onImageBackground;
            }
            else
            {
                Vector3 targetPosition = new Vector3(-_offsetToggleX, _switchBtn.transform.localPosition.y, _switchBtn.transform.localPosition.z);
                _switchBtn.transform.localPosition = targetPosition;

                _backgroundImage.sprite = _offImageBackground;
            }
        }



    }
}
