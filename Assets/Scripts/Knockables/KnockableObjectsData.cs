using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Knockable Object Data")]
public class KnockableObjectsData : ScriptableObject
{
    public KnockableObjectsType destructableObjectType;
    public float knockDownForce;

}

public enum KnockableObjectsType
{
    Slabs,
    Trusses

}
