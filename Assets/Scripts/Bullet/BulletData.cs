using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Data")]
public class BulletData : ScriptableObject
{
    public float bulletSpeedNormal = 10f;
    public float bulletSpeedSlow = 1f;
    public float bulletSpeedDeflect = 500f;
    public int damage = 1;
    public LayerMask layerMask;
}
