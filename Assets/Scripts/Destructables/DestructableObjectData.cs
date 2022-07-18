using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Destructable Object Data")]
public class DestructableObjectData : ScriptableObject
{
    public DestructableObjectType destructableObjectType;

    [Header("For Explosives:")]
    public float explosionRadius;
    public int damageDeal;

    [Header("For Chains:")]
    public float chainHitForce;

    [Header("For Breakables: ")]
    public Material insideMaterial;
    public int totalCuts;
    public float shatterForce;

}

public enum DestructableObjectType
{
    Explosives,
    Shatterables,
    Chains
}
