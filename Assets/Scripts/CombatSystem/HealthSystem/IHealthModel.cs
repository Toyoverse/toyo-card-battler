using System;
using Player;

namespace HealthSystem
{
    public interface IHealthModel
    {
        float GetHealth();

        IHealthPresenter HealthPresenter{ get;}
        
        PlayerNetworkObject Parent{ get; set; }
        
        Action<float> OnGainHP { get; set; }
        
        Action<float> OnChangeHP { get; set; }
        
        Action<float> OnTakeDamage { get; set; }
        
        Action<float> OnInitialize { get; set; }
    }
}