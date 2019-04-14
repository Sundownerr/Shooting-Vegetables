using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class TreeTarget : MonoBehaviour
    {
        public int Score;
        public event EventHandler<TreeTarget> Hit;

        List<Target> defaultTargets = new List<Target>();

        void Awake()
        {
            defaultTargets = new List<Target>(GetComponentsInChildren<Target>());
            Score = 300;
        }

        void Start()
        {
            GameManager.Instance.AddTreeTarget(this);
        }

        void OnCollisionEnter(Collision collider)
        {
            if (collider.gameObject.CompareTag("Bullet"))
            {
                Hit?.Invoke(null, this);

                var targets = new List<Target>(GetComponentsInChildren<Target>());

                if (targets.Count > 0)
                    targets.ForEach(target =>
                    {
                        var rigidBody = target.GetComponent<Rigidbody>();

                        if (rigidBody != null)
                            rigidBody.useGravity = true;

                        target.transform.SetParent(null);
                    });
                else
                    defaultTargets.ForEach(target =>
                    {
                        var newTarget = Instantiate(target.gameObject, target.transform.position, target.transform.rotation);
                        var targetSystem = newTarget.GetComponent<Target>();

                        newTarget.SetActive(true);
                        newTarget.GetComponent<Rigidbody>().useGravity = true;

                        targetSystem.Score = 300;
                        targetSystem.Type = TargetType.Falling;
                        GameManager.Instance.AddTarget(newTarget.GetComponent<Target>());
                    });
            }
        }
    }
}