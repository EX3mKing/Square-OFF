using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "R_Prefab", menuName = "Maze/Rooms/Prefab", order = 2)]
public class R_Prefab : Room
{
    public override RoomGenerationType Generation => RoomGenerationType.Prefab;

    public override Dictionary<(int,int), RoomType> Generate(Dictionary<(int,int), RoomType> mazeReference)
    {
        base.Generate(mazeReference);
        return roomsToGenerate;
    }
}
