using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Game
{
    public class GameEndScreenUI : MonoBehaviour
    {
        public event EventHandler EndGameClicked, ContinueGameClicked;
        public Button EndGameButton, ContinueGameButton;
        public GameObject PlayerResultPrefab;
        public Transform PlayerResultGroup;

        List<PlayerResultUI> results = new List<PlayerResultUI>();

        void Awake()
        {
            EndGameButton.onClick.AddListener(() => EndGameClicked?.Invoke(null, null));
            ContinueGameButton.onClick.AddListener(() => ContinueGameClicked?.Invoke(null, null));
            DeactivateButtons();
        }

        void OnEnable()
        {
            for (int i = 0; i < GameManager.Instance.CurrentPlayer; i++)
            {
                var newResult = Instantiate(PlayerResultPrefab, PlayerResultGroup).GetComponent<PlayerResultUI>();

                newResult.PlayerNameText.text = $"Игрок {i + 1}";
                newResult.ScoreText.text = GameManager.Instance.PlayerResults[i].Score.ToString();
                newResult.AccuracyText.text = $"{GameManager.Instance.PlayerResults[i].Accuracy}%";

                results.Add(newResult);
            }

            if (GameManager.Instance.MaxPlayers > 1 && GameManager.Instance.CurrentPlayer < GameManager.Instance.MaxPlayers)
                ContinueGameButton.gameObject.SetActive(true);
            else
                EndGameButton.gameObject.SetActive(true);
        }

        void OnDisable()
        {
            results.ForEach(result => Destroy(result.gameObject));
            results.Clear();
            DeactivateButtons();
        }

        void DeactivateButtons()
        {
            EndGameButton.gameObject.SetActive(false);
            ContinueGameButton.gameObject.SetActive(false);
        }
    }
}