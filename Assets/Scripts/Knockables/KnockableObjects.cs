using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockableObjects : MonoBehaviour
{
    [SerializeField] protected KnockableObjectsData data;
    [SerializeField] protected Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable dmg = other.gameObject.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.Damage(2);
        }
    }
}
