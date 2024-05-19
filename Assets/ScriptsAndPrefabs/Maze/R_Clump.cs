using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "R_Clump", menuName = "Maze/Rooms/Clump", order = 1)]
public class R_Clump : Room
{
    public override RoomGenerationType Generation => RoomGenerationType.Clump;
    private List<(int, int)> _clump = new List<(int, int)>();

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
        int currentSpace = 0;
        _clump.Add(availableSpaces[spawnRoomIndex]);
        
        int looped = 0;
        
        while (_clump.Count < roomSize  && looped < 10)
        {
            List<(int, int)> cords = AdjacentPossibleSpaces(currentSpace);
            if (cords.Count > 0)
            {
                _clump.Add(cords[Random.Range(0, cords.Count)]);
                currentSpace = Random.Range(0, _clump.Count);
                looped = 0;
            }
            currentSpace = Random.Range(0, _clump.Count);
            looped++;
            if (looped > 10) Debug.Log("looped");
        }
        
        foreach (var clumpSpace in _clump)
        {
            roomsToGenerate.Add(clumpSpace, type);
        }
    }

    private List<(int, int)> AdjacentPossibleSpaces(int clumpSpaceIndex)
    {
        List<(int, int)> spaces = new List<(int, int)>();
        (int, int) up, down, left, right;
        up = Coordinates.Add(_clump[clumpSpaceIndex], (0, 1));
        down = Coordinates.Add(_clump[clumpSpaceIndex], (0, -1));
        left = Coordinates.Add(_clump[clumpSpaceIndex], (-1, 0));
        right = Coordinates.Add(_clump[clumpSpaceIndex], (1, 0));
        
        if (!maze.ContainsKey(up) && !IsSpaceInClump(up)) { spaces.Add(up); }
        if (!maze.ContainsKey(down) && !IsSpaceInClump(down)) { spaces.Add(down); }
        if (!maze.ContainsKey(left) && !IsSpaceInClump(left)) { spaces.Add(left); }
        if (!maze.ContainsKey(right) && !IsSpaceInClump(right)) { spaces.Add(right); }

        return spaces;
    }
    
    private bool IsSpaceInClump((int, int) space)
    {
        foreach (var clumpSpace in _clump)
        {
            if (clumpSpace == space)
            {
                return true;
            }
        }
        return false;
    }

    public override void ClearLogic()
    {
        base.ClearLogic();
        _clump.Clear();
    }
}
