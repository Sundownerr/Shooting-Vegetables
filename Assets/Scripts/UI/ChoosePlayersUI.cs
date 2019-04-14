using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayersUI : MonoBehaviour
{
    public event EventHandler<int> PlayerAmountChoosed;
    public Button OnePlayerButton, TwoPlayersButton, ThreePlayersButton, FourPlayersButton;

    void Awake()
    {
        OnePlayerButton.onClick.AddListener(() => PlayerAmountChoosed?.Invoke(null, 1));
        TwoPlayersButton.onClick.AddListener(() => PlayerAmountChoosed?.Invoke(null, 2));
        ThreePlayersButton.onClick.AddListener(() => PlayerAmountChoosed?.Invoke(null, 3));
        FourPlayersButton.onClick.AddListener(() => PlayerAmountChoosed?.Invoke(null, 4));
    }
}
