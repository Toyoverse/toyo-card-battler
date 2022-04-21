using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PlayerReferences : NetworkBehaviour
{
    [SerializeField] private NetworkCharacterController _cc;


    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5*data.direction*Runner.DeltaTime);
        }
    }

    public GameObject hand;
    public GameObject graveyard;
    public GameObject deck;
    public GameObject Toyo;
}
