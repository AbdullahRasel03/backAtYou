using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slabs : KnockableObjects, IKnockable
{
    public void KnockDownObj(Vector3 forcePosition)
    {
        Vector3 forceDirection = (forcePosition - transform.position).normalized;

        rb.AddForce(forceDirection * data.knockDownForce * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
