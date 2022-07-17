using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static event Action OnPlayerDead;
    public static event Action<int> OnHealthChanged;
    [SerializeField] private int playerMaxHealth;


    private int currentHealth;

    void Start()
    {
        currentHealth = playerMaxHealth;
    }
    public void Damage(int amount)
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
        return playerMaxHealth;
    }
}
