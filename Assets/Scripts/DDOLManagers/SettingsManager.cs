using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Game
{
    public class SettingsManager : MonoBehaviour
    {
        public event EventHandler CloseButtonClicked;
        public Button CloseButton;
        public Slider SoundVolumeSlider;
        public TMP_Dropdown ResolutionDropDown;
        public Toggle EnhanceGraphicsToggle;

        public GameObject PPPrefab;

        void Awake()
        {
            EnhanceGraphicsToggle.onValueChanged.AddListener((value) => PPPrefab.SetActive(value));
            ResolutionDropDown.onValueChanged.AddListener((value) =>
            {
                if (value == 0)
                    Screen.SetResolution(1920, 1080, Screen.fullScreen);

                if (value == 1)
                    Screen.SetResolution(1280, 720, Screen.fullScreen);

                if (value == 2)
                    Screen.SetResolution(800, 600, Screen.fullScreen);
            });

            SoundVolumeSlider.onValueChanged.AddListener((value) => GameManager.Instance.SoundVolume = value / 2);
            CloseButton.onClick.AddListener(() => CloseButtonClicked?.Invoke(null, null));
        }
    }
}