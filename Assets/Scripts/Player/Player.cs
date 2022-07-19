using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static event Action OnPlayerDead;
    public static event Action<int> OnHealthChanged;
    [SerializeField] private PlayerData data;
    [SerializeField] private Camera cam;


    private int currentHealth;

    void Start()
    {
        currentHealth = data.playerMaxHealth;
    }

    public void Damage(int amount, Vector3 hitPosition)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            PopupController.GetInstance().popupLevelLost.ShowView();
            OnPlayerDead?.Invoke();
        }
    }

    public int GetPlayerMaxHealth()
    {
        return data.playerMaxHealth;
    }

    public Camera GetFPSCam()
    {
        return this.cam;
    }
}
