using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

namespace Game
{
    public class ShootSystem : MonoBehaviour
    {
        public event EventHandler<Transform> SlowMoEnabled;
        public event EventHandler SlowMoDisabled, Shot;
        public AudioClip shootSound;
        public AudioSource audioSource;
        public GameObject Bullet;

        EventSystem eventSystem;
        PointerEventData pointerEventData;
        Ray ray;
        List<Rigidbody> bullets = new List<Rigidbody>();
        bool inSlowMo, canUseSlowMo;

        void Awake()
        {
            eventSystem = EventSystem.current;
            pointerEventData = new PointerEventData(eventSystem);
            audioSource.clip = shootSound;
            
            canUseSlowMo = true;
        }

        void Start()
        {
            UIManager.Instance.GameEnded += OnGameEnded;
        }

        void OnGameEnded(object sender, EventArgs e)
        {
            SetSlowMo(false);

            bullets.ForEach(bullet => Destroy(bullet.gameObject));
            bullets.Clear();

            canUseSlowMo = true;
        }

        void Update()
        {
            if (GameManager.Instance.GameState != GameState.InGame) return;

            if (Input.GetKeyDown(KeyCode.F3))
                canUseSlowMo = !canUseSlowMo;

            if (Input.GetMouseButtonDown(0))
                if (!inSlowMo)
                {
                    Shot?.Invoke(null, null);
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var hit))
                    {
                        var newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                        Destroy(newBullet, 0.5f);

                        newBullet.transform.LookAt(hit.point);
                        bullets.Add(newBullet.GetComponent<Rigidbody>());

                        if (hit.transform.gameObject.GetComponent<Target>() != null)
                            SetSlowMo(true, newBullet.transform);
                    }

                    audioSource.volume = GameManager.Instance.SoundVolume / 5;
                    audioSource.DOPitch(inSlowMo ? UnityEngine.Random.Range(0.2f, 0.2f) : 1, 0.1f);
                    audioSource.Play();
                }
                else
                    SetSlowMo(false);

            for (int i = 0; i < bullets.Count; i++)
                if (bullets[i] == null || bullets[i].transform == null)
                {
                    SetSlowMo(false);
                    bullets.RemoveAt(i);
                }
        }

        void FixedUpdate()
        {
            for (int i = 0; i < bullets.Count; i++)
                if (bullets[i] != null && bullets[i].gameObject != null)
                    bullets[i].AddForce(ray.direction * 90, ForceMode.Impulse);
        }

        void SetSlowMo(bool set, Transform targetTransform = null)
        {
            if (set && canUseSlowMo)
                SlowMoEnabled?.Invoke(null, targetTransform);
            else
            {
                audioSource.DOPitch(1, 0.4f);
                SlowMoDisabled?.Invoke(null, null);
            }

            inSlowMo = canUseSlowMo ? set : false;
            Time.timeScale = set && canUseSlowMo ? 0.08f : 1;
        }
    }
}