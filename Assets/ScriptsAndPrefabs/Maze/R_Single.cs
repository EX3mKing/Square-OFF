using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "R_Single", menuName = "Maze/Room/Single", order = 0)]
public class R_Single : Room
{
    public override RoomGenerationType Generation => RoomGenerationType.Single;
    

    public override Dictionary<(int,int), RoomType> Generate(Dictionary<(int,int), RoomType> mazeReference, int index)
    {
        base.Generate(mazeReference, index);
        return maze;
    }
    public override void PickSpawnSpace()
    {
        spawnRoomIndex = Random.Range(0, availableSpaces.Count);
    }
    
    public override void GenerationLogic()
    {
        maze.Add(availableSpaces[spawnRoomIndex], RoomType.Normal);
    }
}
