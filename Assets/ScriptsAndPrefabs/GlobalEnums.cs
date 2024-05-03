using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnums : MonoBehaviour
{
    
}
public enum Owner
{
    Player,
    Enemy,
    Neutral,
}

public enum RoomType
{
    Normal,
    Empty,
    Spawn,
    Rest,
    Shop,
    Treasure,
    Hall,
    Boss,
    BossHall,
    Null
}

public enum RoomGenerationType 
{
    Single,
    Clump,
    Prefab
}

public enum WeaponType
{
    Pistol,
    Rifle,
    Shotgun,
    Sniper,
    Rocket,
    Melee,
    Special,
    None,
}
public enum Element
{
    Cyan,
    Yellow,
    Magenta,
    Black, // can be used in any weapon slot, generally used for special weapons or melee weapons
}

public static class Coordinates
{
    public static (int, int) Add((int, int) coords1, (int, int) coords2)
    {
        return (coords1.Item1 + coords2.Item1, coords1.Item2 + coords2.Item2);
    }
}
