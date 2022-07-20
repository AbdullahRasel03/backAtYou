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
    [SerializeField] private Material playerHitMat;

    private IEnumerator hitEffectCoroutine;
    private int currentHealth;

    void Start()
    {
        currentHealth = data.playerMaxHealth;
    }

    public void Damage(int amount, Vector3 hitPosition)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);

        playerHitMat.SetFloat("_FullScreenIntensity", 0.4f);

        if (hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
        }

        hitEffectCoroutine = RemoveHitEffect();

        StartCoroutine(hitEffectCoroutine);

        if (currentHealth <= 0)
        {
            PopupController.GetInstance().popupLevelLost.ShowView();
            OnPlayerDead?.Invoke();
        }
    }

    private IEnumerator RemoveHitEffect()
    {
        float val = 0.4f;

        while (playerHitMat.GetFloat("_FullScreenIntensity") != 0f)
        {
            val -= Time.deltaTime * 0.6f;
            val = Mathf.Clamp(val, 0f, 0.4f);
            playerHitMat.SetFloat("_FullScreenIntensity", val);

            yield return null;
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

    void OnDestroy()
    {
        playerHitMat.SetFloat("_FullScreenIntensity", 0f);
    }


}
