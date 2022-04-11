using System;
using HealthSystem;
using HealthSystem.HealthUI;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    private float health = 100.0f;
    
    #region GetterAndSetter
    
    public float GetHealth()
    {
        return health;
    }
    
    public void SetHealth(float _health)
    {
        health = _health;
    }

    #endregion

    #region CallBacks
    
    void OnEnable()
    {
        OnGainHP += GainHP;
        OnTakeDamage += TakeDamage;
    }

    void OnDisable()
    {
        OnGainHP -= GainHP;
        OnTakeDamage -= TakeDamage;
    }
    
    #endregion

    void GainHP(float _value)
    {
        health += _value;
        //IHealthUI
    }

    void TakeDamage(float _value)
    {
        health -= _value;
    }

    public Action<float> OnGainHP { get; set; }
    public Action<float> OnTakeDamage { get; set; }
}