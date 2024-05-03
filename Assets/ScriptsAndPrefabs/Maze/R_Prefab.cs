using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "R_Prefab", menuName = "Maze/Room/Prefab", order = 3)]
public class R_Prefab : Room
{
    public override RoomGenerationType Generation => RoomGenerationType.Prefab;

    public override Dictionary<(int,int), RoomType> Generate(Dictionary<(int,int), RoomType> mazeReference, int index)
    {
        base.Generate(mazeReference, index);
        return maze;
    }
}
