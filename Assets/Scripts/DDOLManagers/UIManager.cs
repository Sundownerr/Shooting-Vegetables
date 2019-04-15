using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Game
{
    public class UIManager : MonoBehaviour
    {
        static UIManager instance;
        public static UIManager Instance
        {
            get => instance;
            private set
            {
                if (instance == null) instance = value;
            }
        }

        public event EventHandler GameEnded, EndGameClicked, AllGamesEnded, Paused, UnPaused;
        public event EventHandler<int> GameStarted;
        public event EventHandler<Difficulty> DifficultyChoosed;
        public MenuUI MainMenu;
        public DifficultyUI Difficulty;
        public RecordsUI Records;
        public ChoosePlayersUI ChoosePlayersUI;
        public HUD HUD;
        public PlayerPrepareMessageUI PrepareMessage;
        public GameEndScreenUI GameEndScreen;
        public RecordManager RecordManager;
        public CreatePlayerRecordUI PlayerRecordScreen;
        public SettingsManager SettingsManager;
        public PauseMenuUI PauseMenu;

        int playersAmount;

        void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        void Start()
        {
            MainMenu.OptionsClicked += OnOptionsClicked;
            MainMenu.RecordsClicked += OnRecordsClicked;
            MainMenu.StartGameClicked += OnStartGameClicked;
            Difficulty.DifficultyChoosed += OnDifficultyChoosed;
            ChoosePlayersUI.PlayerAmountChoosed += OnPlayerAmountChoosed;
            HUD.GameEnded += OnGameEnded;
            GameEndScreen.EndGameClicked += OnEndGameClicked;
            GameEndScreen.ContinueGameClicked += OnContinueGameClicked;
            RecordManager.NewRecord += OnNewRecord;
            RecordManager.RecordAdded += OnRecordAdded;
            RecordManager.WillNotRecord += OnWillNotRecord;
            Records.CloseButtonCliked += OnRecordsClosed;
            SettingsManager.CloseButtonClicked += OnSettingsClosing;
            PlayerRecordScreen.CloseButtonClicked += OnPlayerRecordScreenClosed;
            GameManager.Instance.PausePressed += OnPausePressed;
            PauseMenu.ReturnClicked += OnPauseReturnClicked;
            PauseMenu.ExitClicked += OnPauesExitClicked;
        }

        void OnPausePressed(object _, EventArgs e)
        {
            PauseMenu.gameObject.SetActive(true);
        }

        void OnPauesExitClicked(object _, EventArgs e)
        {
            PauseMenu.gameObject.SetActive(false);
            HUD.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
            AllGamesEnded?.Invoke(null, null);
        }

        void OnPauseReturnClicked(object _, EventArgs e)
        {
            PauseMenu.gameObject.SetActive(false);
            UnPaused?.Invoke(null, null);
        }

        void OnPlayerRecordScreenClosed(object _, EventArgs e)
        {
            PlayerRecordScreen.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }

        void OnSettingsClosing(object _, EventArgs e)
        {
            SettingsManager.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }

        void OnRecordsClosed(object _, EventArgs e)
        {
            Records.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
        }

        void OnWillNotRecord(object _, EventArgs e)
        {
            MainMenu.gameObject.SetActive(true);
            AllGamesEnded?.Invoke(null, null);
        }

        void OnRecordAdded(object _, EventArgs e)
        {
            PlayerRecordScreen.gameObject.SetActive(false);
            MainMenu.gameObject.SetActive(true);
            AllGamesEnded?.Invoke(null, null);
        }

        void OnContinueGameClicked(object _, EventArgs e)
        {
            GameEndScreen.gameObject.SetActive(false);
            StartGame();
        }

        void OnNewRecord(object _, RecordEventArgs e)
        {
            PlayerRecordScreen.gameObject.SetActive(true);
            PlayerRecordScreen.Position = e.Position;
            PlayerRecordScreen.Result = e.Result;
        }

        void OnEndGameClicked(object _, EventArgs e)
        {
            GameEndScreen.gameObject.SetActive(false);
            EndGameClicked?.Invoke(null, null);
        }

        void OnGameEnded(object _, EventArgs e)
        {
            EndGame();
        }

        void OnRecordsClicked(object _, EventArgs e)
        {
            MainMenu.gameObject.SetActive(false);
            Records.gameObject.SetActive(true);
        }

        void OnOptionsClicked(object _, EventArgs e)
        {
            MainMenu.gameObject.SetActive(false);
            SettingsManager.gameObject.SetActive(true);
        }

        void OnStartGameClicked(object _, EventArgs e)
        {
            MainMenu.gameObject.SetActive(false);
            Difficulty.gameObject.SetActive(true);
        }

        void OnDifficultyChoosed(object _, Difficulty e)
        {
            Difficulty.gameObject.SetActive(false);
            ChoosePlayersUI.gameObject.SetActive(true);

            DifficultyChoosed?.Invoke(null, e);
        }

        void OnPlayerAmountChoosed(object _, int e)
        {
            ChoosePlayersUI.gameObject.SetActive(false);
            playersAmount = e;

            StartGame();
        }

        void StartGame()
        {
            PrepareMessage.gameObject.SetActive(true);
            PrepareMessage.PlayerNameText.text = $"Игрок {GameManager.Instance.CurrentPlayer + 1}";

            StartCoroutine(HUDActivationDelay());

            IEnumerator HUDActivationDelay()
            {
                FadeManager.Instance.Fade(FadeManager.FadeMode.In);

                yield return new WaitForSeconds(GameManager.FadeDelay);

                GameStarted?.Invoke(null, playersAmount);

                yield return new WaitForSeconds(0.5f);

                FadeManager.Instance.Fade(FadeManager.FadeMode.Out);
                HUD.gameObject.SetActive(true);
                PrepareMessage.gameObject.SetActive(false);

            }
        }

        void EndGame()
        {
            HUD.gameObject.SetActive(false);

            StartCoroutine(HUDActivationDelay());

            IEnumerator HUDActivationDelay()
            {
                FadeManager.Instance.Fade(FadeManager.FadeMode.In);

                yield return new WaitForSeconds(GameManager.FadeDelay);

                GameEnded?.Invoke(null, null);

                yield return new WaitForSeconds(0.5f);

                FadeManager.Instance.Fade(FadeManager.FadeMode.Out);
                GameEndScreen.gameObject.SetActive(true);
            }
        }
    }
}
