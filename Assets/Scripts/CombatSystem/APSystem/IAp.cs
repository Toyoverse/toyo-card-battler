using System;
using APSystem.ApUI;

namespace APSystem
{
    public interface IAp
    {
        int GetAP();

        IApUI ApUI { get;}
        
        Action<int> OnGainAP { get; set; }
        
        Action<int> OnUseAP { get; set; }
    }
}