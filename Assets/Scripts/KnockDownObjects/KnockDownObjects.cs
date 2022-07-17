using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDownObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable dmg = other.gameObject.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.Damage(2);
        }
    }
}
