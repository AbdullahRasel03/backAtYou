using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillArea;

    void Start()
    {
        int health = LevelController.GetInstance().GetPlayer().GetPlayerMaxHealth();
        slider.maxValue = health;
        slider.value = health;
        fillArea.color = Color.green;
    }

    void OnEnable()
    {
        Player.OnHealthChanged += HealthChanged;
    }

    void OnDisable()
    {
        Player.OnHealthChanged -= HealthChanged;
    }

    private void HealthChanged(int val)
    {
        slider.value = val;

        if (val <= 2)
        {
            fillArea.color = Color.red;
        }

        else
        {
            fillArea.color = Color.green;
        }

    }
}
