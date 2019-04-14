using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{


    public class FadeManager : MonoBehaviour
    {
        Image image;

        public enum FadeMode { In, Out }

        static FadeManager instance;
        public static FadeManager Instance
        {
            get => instance;
            private set
            {
                if (instance == null) instance = value;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            image = GetComponent<Image>();
        }

        void Start()
        {
            image.DOFade(0, 1);
        }

        public void Fade(FadeMode inOrOut)
        {
            if (inOrOut == FadeMode.Out)
                image.DOFade(0, GameManager.FadeDelay);

            if (inOrOut == FadeMode.In)
                image.DOFade(1, GameManager.FadeDelay);
        }
    }
}