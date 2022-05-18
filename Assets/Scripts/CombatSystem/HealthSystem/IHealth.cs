using System;
using HealthSystem.HealthUI;
using Player;

namespace HealthSystem
{
    public interface IHealth
    {
        float GetHealth();

        IHealthUI HealthUI{ get;}
        
        PlayerNetworkObject Parent{ get; set; }
        
        Action<float> OnGainHP { get; set; }
        
        Action<float> OnChangeHP { get; set; }
        
        Action<float> OnTakeDamage { get; set; }
        
        Action<float> OnInitialize { get; set; }
    }
}