using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuUI: MonoBehaviour
{
    public event EventHandler StartGameClicked;
    public event EventHandler RecordsClicked;
    public event EventHandler OptionsClicked; 

    public Button StartGameButton, RecordsButton, OptionsButton, QuitButton;

    void Awake()
    {
        StartGameButton.onClick.AddListener(() => StartGameClicked?.Invoke(null, null));
        RecordsButton.onClick.AddListener(() => RecordsClicked?.Invoke(null, null));
        OptionsButton.onClick.AddListener(() => OptionsClicked?.Invoke(null, null));
        QuitButton.onClick.AddListener(() => Application.Quit());
    }
}
