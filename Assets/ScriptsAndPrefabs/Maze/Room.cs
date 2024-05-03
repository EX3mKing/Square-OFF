using System.Collections.Generic;
using UnityEngine;

public class Room : ScriptableObject
{
    public int x;
    public int y;
    public RoomType type;
    public int spawnDistanceMin = 1;
    public int spawnDistanceMax;
    public List<(int,int)> availableSpaces = new List<(int, int)>();

    public Dictionary<(int, int), RoomType> maze;
    public int sectionIndex;
    public int spawnRoomIndex;
    
    public virtual RoomGenerationType Generation { get; private set; }

    public virtual Dictionary<(int, int), RoomType> Generate(Dictionary<(int,int), RoomType> mazeReference,  int index)
    {
        ClearLogic();
        AssignMazeInfo(mazeReference, index);
        FindSpawnableSpaces();
        PickSpawnSpace();
        GenerationLogic();

        return maze;
    }
    
    public virtual void FindSpawnableSpaces()
    {
        foreach (var room in maze)
        {
            for (int i = spawnDistanceMin; i < spawnDistanceMax + 1; i++)
            {
                for (int j = spawnDistanceMin; j < spawnDistanceMax - i + 1 ; j++)
                {
                    CheckAndAdd((j, i), room.Key);
                    CheckAndAdd((-j, i), room.Key);
                    CheckAndAdd((j, -i), room.Key);
                    CheckAndAdd((-j, -i), room.Key);
                }
                
                CheckAndAdd((0,i), room.Key);
                CheckAndAdd((0,-i), room.Key);
                CheckAndAdd((i,0), room.Key);
                CheckAndAdd((-i,0), room.Key);
            }
        }
    }
    
    public virtual void CheckAndAdd((int, int) coordinates, (int, int) room)
    {
        (int, int) globalCoordinates = Coordinates.Add(coordinates, room);
        if (!maze.ContainsKey(globalCoordinates))
        {
            availableSpaces.Add(globalCoordinates);
        }
    }
    
    public virtual void AssignMazeInfo(Dictionary<(int,int), RoomType> mazeReference, int index)
    {
        sectionIndex = index;
        maze = mazeReference;
    }

    public virtual void PickSpawnSpace()
    {
        
    }
    
    public virtual void GenerationLogic()
    {
        
    }

    public virtual void ClearLogic()
    {
        availableSpaces.Clear();
    }
}
