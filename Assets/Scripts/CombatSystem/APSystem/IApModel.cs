using System;

namespace CombatSystem.APSystem
{
    public interface IApModel
    {
        int GetAP();

        IApPresenter ApPresenter { get;}
        
        Action<int> OnGainAP { get; set; }
        
        Action<int> OnChangeAP { get; set; }
        
        Action<int> OnUseAP { get; set; }
    }
}