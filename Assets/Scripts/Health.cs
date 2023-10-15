using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaxHeath = 10;
    [SerializeField] private int CurrentHealth = 0;

    [SerializeField] private Slider HealthSlider;

    void Start()
    {
        CurrentHealth = MaxHeath;
        HealthSlider.maxValue = MaxHeath;
        HealthSlider.value = CurrentHealth;
    }

    public void UpdateHealth(int Hp)
    {
        //update health
        CurrentHealth += Hp;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHeath);

        //ui
        HealthSlider.value = CurrentHealth;

        //death
        if(CurrentHealth == 0)
        {
            Debug.Log("DIED!");
        }
    }
}
