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
    HP_MOD,
    AP_MOD,
    CHANGE_STAT,
    CARD_MOD_DAMAGE, 
    CARD_MOD_COST, 
    CARD_MOD_TRUE_DAMAGE,
    CARD_MOD_LIFE_STEAL,
    RULE_MOD, 
    STUN,
    DISCARD
}

public enum TOYO_PIECE
{
    HEAD,
    CHEST,
    R_ARM,
    L_ARM,
    R_HAND,
    L_HAND,
    R_LEG,
    L_LEG,
    R_FOOT,
    L_FOOT
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

public enum TOYO_TYPE
{
    ALLY,
    ENEMY
}