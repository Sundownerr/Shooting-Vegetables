using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public enum TargetType { Stationary, Falling }

    public class Target : MonoBehaviour
    {
        public event EventHandler<Target> Hit;
        public int Score;
        public GameObject Destroyed;
        public AudioClip SplashSound, SecondSound;
        public TargetType Type { get; set; } = TargetType.Stationary;

        AudioSource source;
        SphereCollider slowMoBoundary;

        void Start()
        {
            source = GetComponent<AudioSource>();
            slowMoBoundary = transform.GetChild(0).GetComponent<SphereCollider>();
            source.volume = GameManager.Instance.SoundVolume / 2; ;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Terrain"))
            {
                if (collision.gameObject.CompareTag("Bullet"))
                    Hit?.Invoke(null, this);

                PlaySounds();
                GetComponent<MeshRenderer>().enabled = false;
                Destroy(GetComponent<SphereCollider>());
                //Destroy(GetComponent<Rigidbody>());

                Destroy(Instantiate(Destroyed, transform.position, Quaternion.identity), 5);
                transform.position = Vector3.zero;

                Destroy(gameObject, 0.7f);
            }

            #region Helper functions

            void PlaySounds()
            {
                source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                source.PlayOneShot(SplashSound, GameManager.Instance.SoundVolume / 2);

                if (SecondSound != null)
                    source.PlayOneShot(SecondSound, GameManager.Instance.SoundVolume / 2);
            }

            #endregion
        }
    }
}