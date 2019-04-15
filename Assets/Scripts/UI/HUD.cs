using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


namespace Game
{
    public class HUD : MonoBehaviour
    {
        public event EventHandler GameEnded;
        public GameObject ReticlePrefab, GetReadyPrefab;
        public TextMeshProUGUI TimerText, ScoreText;
        public ShootSystem ShootSystem;

        WaitForSeconds delay = new WaitForSeconds(1);
        int remainingTime;
        bool reticleDisabled;
        bool cursorPrevState;

        void Awake()
        {
            ReticlePrefab = Instantiate(ReticlePrefab, transform);
        }

        void Start()
        {
            GameManager.Instance.TargetHit += OnTargetHit;
            ShootSystem.SlowMoEnabled += OnSlowMoEnabled;
            ShootSystem.SlowMoDisabled += OnSlowMoDisabled;
            ShootSystem.PausePressed += OnPaused;
            UIManager.Instance.UnPaused += OnUnPaused;
        }

        void OnUnPaused(object _, EventArgs e) => Cursor.visible = !reticleDisabled;
        void OnSlowMoEnabled(object _, Transform e)
        {
            ReticlePrefab.gameObject.SetActive(false);
            Cursor.visible = false;
        }
        void OnTargetHit(object _, int e) => ScoreText.text = e.ToString();

        void OnPaused(object _, EventArgs e)
        {
            cursorPrevState = Cursor.visible;
            Cursor.visible = true;
        }

        void OnSlowMoDisabled(object _, EventArgs e)
        {
            if (!reticleDisabled)
            {
                ReticlePrefab.gameObject.SetActive(true);
                Cursor.visible = true;
            }
        }

        void Update()
        {
            if (GameManager.Instance.IsGamePaused) return;

            if (Input.GetKeyDown(KeyCode.F1))
            {
                ReticlePrefab.gameObject.SetActive(!ReticlePrefab.gameObject.activeSelf);
                reticleDisabled = !reticleDisabled;
                Cursor.visible = !Cursor.visible;
            }

            ReticlePrefab.transform.position = Input.mousePosition;
        }

        void OnDisable()
        {
            ReticlePrefab.gameObject.SetActive(false);
            Cursor.visible = true;
        }

        void OnEnable()
        {
            remainingTime = 60;
            TimerText.text = $"{remainingTime}";
            ScoreText.text = "0";

            StartCoroutine(DelayedStart());

            IEnumerator DelayedStart()
            {
                yield return new WaitForSeconds(0.3f);

                ReticlePrefab.gameObject.SetActive(true);

                StartCoroutine(Timer());

                IEnumerator Timer()
                {
                    while (remainingTime > 0)
                    {
                        yield return delay;
                        remainingTime--;
                        TimerText.text = $"{remainingTime}";
                    }

                    GameEnded?.Invoke(null, null);
                }
            }
        }
    }
}
