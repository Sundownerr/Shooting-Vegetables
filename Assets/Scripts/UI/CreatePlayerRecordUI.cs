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
        public event EventHandler<PlayerRecord> NewRecordCreated;
        public TextMeshProUGUI PlayerName;
        public List<Button> KeyboardButtons;
        public Button ClearSymbolButton, ClearAllButton, SaveButton;
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

            SaveButton.onClick.AddListener(() => NewRecordCreated(null,
                new PlayerRecord()
                {
                    Name = string.IsNullOrWhiteSpace(PlayerName.text) ? "_" : PlayerName.text,
                    Score = Result.Score,
                    Date = $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year.ToString().Remove(0, 2)}",
                    Position = Position
                }));
        }

        void OnEnable()
        {
            PlayerName.text = string.Empty;
        }
    }
}