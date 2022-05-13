using System;
using HealthSystem;
using HealthSystem.HealthUI;
using Tools;
using UnityEngine;

[UseAttributes]
public class Health : MonoBehaviour, IHealth
{
    private float health = 100.0f;
    
    #region GetterAndSetter
    
    public float GetHealth()
    {
        return health;
    }

    private IHealthUI MyHealthUI;
    IHealthUI IHealth.HealthUI => MyHealthUI;

    public void SetHealth(float _health)
    {
        health = _health;
    }

    #endregion

    #region CallBacks

    void Awake()
    {
        MyHealthUI = GetComponent<HealthUI>();
    }

    void OnEnable()
    {
        OnGainHP += GainHP;
        OnTakeDamage += TakeDamage;
        OnChangeHP += ChangeHP;
    }

    void OnDisable()
    {
        OnGainHP -= GainHP;
        OnTakeDamage -= TakeDamage;
        OnChangeHP -= ChangeHP;
    }
    
    #endregion

    void ChangeHP(float _value)
    {
        if (_value > 0)
            OnGainHP?.Invoke(_value);
        else
            OnTakeDamage?.Invoke(_value);
    }
    
    void GainHP(float _value)
    {
        health += _value;
        MyHealthUI?.OnUpdateHealthUI?.Invoke(health);
    }

    void TakeDamage(float _value)
    {
        health -= _value;
        MyHealthUI?.OnUpdateHealthUI?.Invoke(health);
    }

    public float testHPValue = 10.0f;
    
    [Button]
    public void TestDamage()
    {
        TakeDamage(testHPValue);
    }

    [Button]
    public void TestHealing()
    {
        GainHP(testHPValue);
    }

    public Action<float> OnGainHP { get; set; }
    public Action<float> OnChangeHP { get; set; }
    public Action<float> OnTakeDamage { get; set; }
}