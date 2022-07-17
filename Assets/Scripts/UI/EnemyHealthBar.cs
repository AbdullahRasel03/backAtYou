using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;



    public void SetMaxHealth(int val)
    {
        slider.value = val;
    }

    public void ChangeHealth(int val)
    {
        slider.value = val;
    }


}
