﻿using CombatSystem.APSystem;
using Fusion;
using HealthSystem;
using CardSystem.PlayerHand;
using ToyoSystem;

namespace Player
{
    public interface IPlayer
    {
        IPlayerHand PlayerHand { get; }
        IFullToyo FullToyo { get; }
        HealthModel PlayerHealthModel { get; }
        ApModel PlayerApModel { get; }
        PlayerRef NetworkPlayerRef { get; set; }

    }
}