using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "R_Single", menuName = "Maze/Rooms/Single", order = 0)]
public class R_Single : Room
{
    public override RoomGenerationType Generation => RoomGenerationType.Single;
    public override int roomSize => 1;

    public override Dictionary<(int,int), RoomType> Generate(Dictionary<(int,int), RoomType> mazeReference)
    {
        base.Generate(mazeReference);
        return roomsToGenerate;
    }
    public override void PickSpawnSpace()
    {
        spawnRoomIndex = Random.Range(0, availableSpaces.Count);
    }
    
    public override void GenerationLogic()
    {
        roomsToGenerate.Add(availableSpaces[spawnRoomIndex], type);
    }
}
