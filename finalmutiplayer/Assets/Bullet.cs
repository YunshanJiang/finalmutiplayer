using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        var player = hit.GetComponent<player>();
        if (health != null)
        {
            health.TakeDamage(20);
        }

        Destroy(gameObject);
    }
}
