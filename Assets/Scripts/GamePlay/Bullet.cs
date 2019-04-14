using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool isHit;

    void OnCollisionEnter(Collision collision)
    {
        if (!isHit)
        {
            isHit = true;

            Destroy(GetComponent<CapsuleCollider>());

            Destroy(gameObject);
        }
    }
}
