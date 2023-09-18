using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType 
{ 
    Bomb, Ring, Brimstone, Tech, Tear
}

public enum ProjectileType
{
    None, Planet, Zigzag
}

public enum ItemType 
{
    AttackSpeedUp, AttackSpeedDown, 
    PowerUp, PowerDown, 
    RangeUp, RangeDown, 
    ProjectileUp, ProjectileDown,
    Bomb, Ring, Brimstone, Tech, Tear,
    ProjectileTypeNone, ProjectileTypePlanet, ProjectileTypeZigzag,
    AttackOptionNone, AttackOptionElectric
}
