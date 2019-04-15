using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseMenuUI : MonoBehaviour
{
    public event EventHandler ReturnClicked, ExitClicked;
    public Button ReturnButton, ExitButton;

    void Awake()
    {
        ReturnButton.onClick.AddListener(() => ReturnClicked?.Invoke(null, null));
        ExitButton.onClick.AddListener(() => ExitClicked?.Invoke(null, null));
    }
}
