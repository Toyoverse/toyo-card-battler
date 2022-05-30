using System;
using HealthSystem;
using HealthSystem.HealthUI;
using Player;
using Tools;
using UnityEngine;

[UseAttributes]
public class Health : MonoBehaviour, IHealth
{
    public PlayerNetworkObject Parent { get; set; }
    
    private IHealthUI MyHealthUI;
    IHealthUI IHealth.HealthUI => MyHealthUI;
    
    #region GetterAndSetter
    
    public float GetHealth()
    {
        return Parent.Health;
    }
    
    /*public void SetHealth(float _health)
    {
        Parent.Health = _health;
    }
    */

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
        OnInitialize += Initialize;
    }

    void OnDisable()
    {
        OnGainHP -= GainHP;
        OnTakeDamage -= TakeDamage;
        OnChangeHP -= ChangeHP;
        OnInitialize += Initialize;
    }
    
    #endregion

    void Initialize(float _value)
    {
        OnGainHP?.Invoke(_value);
    }
    
    void ChangeHP(float _value)
    {
        var _temp = GetHealth() - _value;
        if (_temp > 0)
            OnGainHP?.Invoke(_temp);
        else
            OnTakeDamage?.Invoke(_temp);
    }
    
    void GainHP(float _value)
    {
        var _health = GetHealth() + _value;
        if (_health > PlayerNetworkObject.MAX_HEALTH)
            _health = PlayerNetworkObject.MAX_HEALTH;
        Parent.Health = _health;
        MyHealthUI?.OnUpdateHealthUI?.Invoke(_health);
    }

    void TakeDamage(float _value)
    {
        var _health = GetHealth() - _value;
        if (_health < 0)
            _health = 0;
        Parent.Health = _health;
        MyHealthUI?.OnUpdateHealthUI?.Invoke(_health);
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
    public Action<float> OnInitialize { get; set; }
}