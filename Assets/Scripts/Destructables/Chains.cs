using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chains : DestructableObject, IDestructable
{
    [SerializeField] private HingeJoint connectedHingeJoint;
    [SerializeField] private Rigidbody secondLastChainRB;

    public void DestroyObj()
    {
        Destroy(connectedHingeJoint);
        secondLastChainRB.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
    }
}
