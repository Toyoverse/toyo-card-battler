using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Multiplayer
{
    public class PlayerNetworkBehaviour : NetworkBehaviour
    {
        public override void FixedUpdateNetwork()
        {
            /*if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize();
                _cc.Move(5*data.direction*Runner.DeltaTime);
            }
            */
        }
    }
}

