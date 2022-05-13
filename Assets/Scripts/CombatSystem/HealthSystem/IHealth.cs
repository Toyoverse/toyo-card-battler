using System;
using HealthSystem.HealthUI;

namespace HealthSystem
{
    public interface IHealth
    {
        float GetHealth();

        IHealthUI HealthUI{ get;}
        
        Action<float> OnGainHP { get; set; }
        
        Action<float> OnChangeHP { get; set; }
        
        Action<float> OnTakeDamage { get; set; }
    }
}