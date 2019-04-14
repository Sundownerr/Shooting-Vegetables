using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraManager : MonoBehaviour
    {
        static CameraManager instance;
        public static CameraManager Instance
        {
            get => instance;
            private set
            {
                if (instance == null) instance = value;
            }
        }

        public Cinemachine.CinemachineVirtualCamera BulletCamera;
        public ShootSystem ShootSystem;

        void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }

        void Start()
        {
            ShootSystem.SlowMoEnabled += OnSlowMoEnabled;
            ShootSystem.SlowMoDisabled += OnSlowMoDisabled;
        }

        private void OnSlowMoDisabled(object sender, EventArgs e)
        {
            BulletCamera.Priority = 1;
            BulletCamera.Follow = null;
            BulletCamera.LookAt = null;
            
        }

        private void OnSlowMoEnabled(object sender, Transform e)
        {
            BulletCamera.Priority = 100;
            BulletCamera.Follow = e;
            BulletCamera.LookAt = e;
            
        }
    }
}