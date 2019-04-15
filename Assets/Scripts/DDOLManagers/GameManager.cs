using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum GameState
    {
        InMainMenu,
        BetweenGames,
        InGame
    }

    public enum Difficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }

    public class PlayerResult
    {
        public int Score, Accuracy;
    }

    public class GameManager : MonoBehaviour
    {

        static GameManager instance;
        public static GameManager Instance
        {
            get => instance;
            private set
            {
                if (instance == null) instance = value;
            }
        }

        float soundVolume = 0.5f;
        public float SoundVolume
        {
            get => soundVolume;
            set
            {
                soundVolume = value;
                audioSource.volume = SoundVolume / 3;
            }
        }

        float prevTimeScale;
        bool isGamePaused;
        public bool IsGamePaused
        {
            get => isGamePaused;
            private set
            {
                isGamePaused = value;

                if (Time.timeScale != 0) prevTimeScale = Time.timeScale;
                Time.timeScale = isGamePaused ? 0 : prevTimeScale;
            }
        }

        public const float FadeDelay = 0.3f;

        public event EventHandler<int> TargetHit;
        public event EventHandler PausePressed;

        public GameState GameState { get; private set; }
        public List<PlayerResult> PlayerResults { get; private set; } = new List<PlayerResult>(4);
        public int MaxPlayers { get; private set; }
        public int CurrentPlayer { get; private set; }
        public int ShotsFired { get; private set; }
        public int ShotsHit { get; private set; }
        public int Score { get; set; }
        public float PlayerAccuracy { get; private set; }
        public Difficulty GameDifficulty { get; private set; }
        public List<Target> Targets;
        public ShootSystem ShootSystem;

        List<AudioSource> audioSources = new List<AudioSource>();
        AudioSource audioSource;
        List<Vector3> targetsPositions = new List<Vector3>();
        List<Target> targets = new List<Target>();
        Vector3[] difficultyPositions = new Vector3[3];
        int hitStreak = 0;
        bool bulletFired;


        void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;

            audioSource = GetComponent<AudioSource>();
            AddAudioSource(audioSource);

            Targets.ForEach(target => targetsPositions.Add(target.transform.position));

            difficultyPositions[(int)Difficulty.Easy] = ShootSystem.transform.position + new Vector3(5, 0, 5);
            difficultyPositions[(int)Difficulty.Normal] = ShootSystem.transform.position + new Vector3(9, 0, 9);
            difficultyPositions[(int)Difficulty.Hard] = ShootSystem.transform.position + new Vector3(14, 0, 14);

            ShootSystem.transform.position = difficultyPositions[(int)Difficulty.Easy];
        }

        void Start()
        {
            GameState = GameState.InMainMenu;
            UIManager.Instance.GameStarted += OnGameStarted;
            UIManager.Instance.GameEnded += OnGameEnded;
            UIManager.Instance.DifficultyChoosed += OnDifficultyChoosed;
            UIManager.Instance.AllGamesEnded += OnAllGamesEnded;
            UIManager.Instance.UnPaused += OnUnPaused;
            UIManager.Instance.HUDDeactivated += OnHUDDeactivated;

            ShootSystem.Shot += OnShot;
            ShootSystem.SlowMoEnabled += OnSlowMoEnabled;
            ShootSystem.SlowMoDisabled += OnSlowMoDisabled;
            ShootSystem.PausePressed += OnPausePressed;
        }

        void OnHUDDeactivated(object sender, EventArgs e) => GameState = GameState.BetweenGames;
        void OnDifficultyChoosed(object _, Difficulty e) => GameDifficulty = e;
        void OnUnPaused(object _, EventArgs e) => ChangePause();

        void OnSlowMoDisabled(object _, EventArgs e) =>
           targets.ForEach(target => { if (target.Type == TargetType.Falling) target.Freeze(false); });

        void OnSlowMoEnabled(object _, Transform e) =>
            targets.ForEach(target => { if (target.Type == TargetType.Falling) target.Freeze(true); });

        void OnPausePressed(object _, EventArgs e)
        {
            PausePressed?.Invoke(null, null);
            ChangePause();
        }

        void ChangePause()
        {
            IsGamePaused = !IsGamePaused;

            audioSources.RemoveAll(src => src == null || src.gameObject == null);
            audioSources.ForEach(src =>
           {
               if (IsGamePaused)
                   src.Pause();
               else
                   src.UnPause();
           });
        }

        void OnAllGamesEnded(object _, EventArgs e)
        {
            if (IsGamePaused) ChangePause();

            GameState = GameState.InMainMenu;
            ShootSystem.transform.position = difficultyPositions[(int)Difficulty.Easy];
            MaxPlayers = 0;
            CurrentPlayer = 0;
            ShotsFired = 0;
            ShotsHit = 0;
            Score = 0;
            PlayerAccuracy = 0;
            PlayerResults.Clear();
        }

        void OnGameEnded(object _, EventArgs e)
        {
            if (IsGamePaused) ChangePause();

            PlayerAccuracy = ShotsFired > 0 ? (float)ShotsHit / (float)ShotsFired * 100f : 0;

            PlayerResults.Add(new PlayerResult() { Score = Score, Accuracy = (int)PlayerAccuracy });
            GameState = GameState.BetweenGames;
        }

        void OnShot(object _, EventArgs e)
        {
            ShotsFired += 1;

            if (bulletFired)
                hitStreak = 0;
            bulletFired = true;
        }

        void OnTargetHit(object _, Target e)
        {
            if (e.Type == TargetType.Falling)
            {
                Score += e.Score * (1 + hitStreak);
                hitStreak += hitStreak <= 3 ? 1 : 0;
                bulletFired = false;
            }
            else
            {
                hitStreak = 0;
                Score += e.Score;
            }

            ShotsHit += 1;
            targets.Remove(e);
            TargetHit?.Invoke(null, Score);
        }

        void OnTreeTargetHit(object _, TreeTarget e)
        {
            hitStreak = 0;
            ShotsHit += 1;
            Score += e.Score;
            TargetHit?.Invoke(null, Score);
        }

        void OnGameStarted(object _, int e)
        {
            ShootSystem.transform.position = difficultyPositions[(int)GameDifficulty];

            MaxPlayers = e;
            CurrentPlayer++;
            GameState = GameState.InGame;
            ShotsHit = 0;
            ShotsFired = 0;
            Score = 0;

            StartCoroutine(PrepareGame());

            IEnumerator PrepareGame()
            {
                yield return new WaitForSeconds(0.5f);

                targets.ForEach(target => { if (target != null && target.gameObject != null) Destroy(target.gameObject); });
                targets.Clear();

                Targets.ForEach(target =>
                {
                    target.gameObject.SetActive(true);
                    targets.Add(Instantiate(target, target.transform.position, target.transform.rotation, target.transform.parent));
                    targets[targets.Count - 1].gameObject.SetActive(true);
                    AddTarget(targets[targets.Count - 1]);
                    target.gameObject.SetActive(false);
                });
            }
        }

        public void AddTarget(Target target)
        {
            target.Hit += OnTargetHit;

            if (!targets.Contains(target))
                targets.Add(target);
        }
        
        public void AddTreeTarget(TreeTarget target) => target.Hit += OnTreeTargetHit;
        public void RemoveTarget(Target target) => targets.Remove(target);
        public void AddAudioSource(AudioSource audioSource) => audioSources.Add(audioSource);

    }
}
