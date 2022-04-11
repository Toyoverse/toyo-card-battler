using System;

namespace HealthSystem
{
    public interface IHealth
    {
        float GetHealth();
        
        Action<float> OnGainHP { get; set; }
        
        Action<float> OnTakeDamage { get; set; }
    }
}