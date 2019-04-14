using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game
{
    public class DifficultyUI : MonoBehaviour
    {
        public event EventHandler<Difficulty> DifficultyChoosed;
        public Button EasyButton, NormalButton, HardButton;

        void Awake()
        {
            EasyButton.onClick.AddListener(() => DifficultyChoosed?.Invoke(null, Difficulty.Easy));
            NormalButton.onClick.AddListener(() => DifficultyChoosed?.Invoke(null, Difficulty.Normal));
            HardButton.onClick.AddListener(() => DifficultyChoosed?.Invoke(null, Difficulty.Hard));
        }
    }
}