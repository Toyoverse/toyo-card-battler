using System;

public enum CARD_TYPE
{
    HEAVY,
    FAST,
    DEFENSE,
    BOND,
    SUPER
}

public enum ATTACK_TYPE
{
    CYBER,
    PHYSICAL
}

public enum DEFENSE_TYPE
{
    BLOCK,
    DODGE
}

public enum ATTACK_SUB_TYPE
{
    NEUTRAL,
    PIERCING,
    SMASHING,
    SLASHING,
    MAGNETIC,
    ELECTRIC
}

public enum EFFECT_TYPE
{
    GAIN_HP,
    GAIN_AP,
    REDUCE_STAT
}

public enum TOYO_PIECE
{
    HEAD,
    CHEST,
    ARM,
    HAND,
    LEG,
    FOOT
}

public enum TOYO_RARITY
{
    COMMON,
    UNCOMMON,
    RARE,
    LIMITED,
    COLLECTOR,
    PROTOTYPE
}

public enum TOYO_TECHNOALLOY
{
    SIDERITE,
    MERCURY,
    TITANIUM,
    ALUMINUM,
    CARBON,
    SILICON
}

public enum TOYO_STAT
{
    VITALITY,
    RESISTANCE,
    RESILIENCE,
    PHYSICAL_STRENGHT,
    CYBER_FORCE,
    TECHNIQUE,
    ANALYSIS,
    AGILITY,
    SPEED,
    PRECISION,
    STAMINA,
    LUCK
}