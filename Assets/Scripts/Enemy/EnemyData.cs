using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data")]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType;
    public float playerYBound;
    public float playerXBound;
    public GameObject bulletPrefab;
    public GameObject healthBarPrefab;
    public int maxHealth;

    [Header("Data For Static Enemies: ")]
    public float shootDelayMin;
    public float shootDelayMax;

    [Header("Data For Moving Enemies: ")]
    public int movingSpeed;
    public float movingTime;
}

public enum EnemyType
{
    Static,
    Moving
}
