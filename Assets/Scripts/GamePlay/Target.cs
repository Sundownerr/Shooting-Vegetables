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
        new Rigidbody rigidbody;

        void Start()
        {
            source = GetComponent<AudioSource>();
            rigidbody = GetComponent<Rigidbody>();
            slowMoBoundary = transform.GetChild(0).GetComponent<SphereCollider>();
            source.volume = GameManager.Instance.SoundVolume / 2; ;
            GameManager.Instance.AddAudioSource(source);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (Type == TargetType.Falling)
                if (!collision.gameObject.CompareTag("Tree") && !collision.gameObject.CompareTag("Target"))
                {
                    DestroyTarget();
                    return; 
                }

            if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Terrain"))
                DestroyTarget();

            #region Helper functions

            void PlaySounds()
            {
                source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                source.PlayOneShot(SplashSound);

                if (SecondSound != null)
                    source.PlayOneShot(SecondSound);
            }

            void DestroyTarget()
            {
                if (collision.gameObject.CompareTag("Bullet"))
                    Hit?.Invoke(null, this);

                PlaySounds();
                GetComponent<MeshRenderer>().enabled = false;

                Destroy(Instantiate(Destroyed, transform.position, Quaternion.identity), 5);
                transform.position = -Vector3.zero * 100;
                Destroy(gameObject, 0.7f);

                GameManager.Instance.RemoveTarget(this);
            }

            #endregion
        }

        public void Freeze(bool freeze)
        {
            rigidbody.constraints = freeze ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
        }
    }
}