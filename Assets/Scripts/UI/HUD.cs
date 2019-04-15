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

        void Awake()
        {
            ReticlePrefab = Instantiate(ReticlePrefab, transform);
        }

        void Start()
        {
            GameManager.Instance.TargetHit += OnTargetHit;
            ShootSystem.SlowMoEnabled += OnSlowMoEnabled;
            ShootSystem.SlowMoDisabled += OnSlowMoDisabled;
        }

        void OnSlowMoDisabled(object _, EventArgs e)
        {
            ReticlePrefab.gameObject.SetActive(true);
        }

        void OnSlowMoEnabled(object _, Transform e)
        {
            ReticlePrefab.gameObject.SetActive(false);
        }

        void OnTargetHit(object _, int e)
        {
            ScoreText.text = e.ToString();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) ReticlePrefab.gameObject.SetActive(!ReticlePrefab.gameObject.activeSelf);
            ReticlePrefab.transform.position = Input.mousePosition;
        }

        void OnDisable()
        {
            ReticlePrefab.gameObject.SetActive(false);
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
