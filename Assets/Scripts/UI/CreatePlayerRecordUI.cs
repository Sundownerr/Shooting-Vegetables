using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    public class CreatePlayerRecordUI : MonoBehaviour
    {
        public event EventHandler CloseButtonClicked;
        public event EventHandler<PlayerRecord> NewRecordCreated;
        public TextMeshProUGUI PlayerName;
        public List<Button> KeyboardButtons;
        public Button ClearSymbolButton, ClearAllButton, SaveButton, CloseButton;
        public int Position;
        public PlayerResult Result;

        void Awake()
        {
            KeyboardButtons.ForEach(key => key.onClick.AddListener(() => PlayerName.text += key.gameObject.name));

            ClearSymbolButton.onClick.AddListener(() =>
            {
                if (PlayerName.text.Length > 0)
                    PlayerName.text = PlayerName.text.Remove(PlayerName.text.Length - 1);
            });

            ClearAllButton.onClick.AddListener(() => PlayerName.text = string.Empty);

            CloseButton.onClick.AddListener(() => CloseButtonClicked?.Invoke(null, null));

            SaveButton.onClick.AddListener(() =>
            {
                if (!string.IsNullOrWhiteSpace(PlayerName.text))
                    NewRecordCreated?.Invoke(null, new PlayerRecord()
                    {
                        Name = PlayerName.text,
                        Score = Result.Score,
                        Date = $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year.ToString().Remove(0, 2)}",
                        Position = Position
                    });
            });
        }

        void OnEnable()
        {
            PlayerName.text = string.Empty;
        }
    }
}