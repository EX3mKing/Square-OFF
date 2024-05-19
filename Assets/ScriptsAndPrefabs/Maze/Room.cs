using System.Collections.Generic;
using UnityEngine;

public class Room : ScriptableObject
{
    public RoomType type;
    public virtual int roomSize { get; set; }
    public int spawnDistanceMin = 1;
    public int spawnDistanceMax = 2;
    public int spawnRoomIndex;
    public List<(int,int)> availableSpaces = new List<(int, int)>();
    public (int, int) closestSpace;
    public List<(int,int)> hallSpaces = new List<(int, int)>();
    public Dictionary<(int, int), RoomType> maze;
    public Dictionary<(int, int), RoomType> roomsToGenerate = new Dictionary<(int, int), RoomType>();
    
    public virtual RoomGenerationType Generation { get; private set; }

    public virtual Dictionary<(int, int), RoomType> Generate(Dictionary<(int,int), RoomType> mazeReference)
    {
        ClearLogic();
        AssignMazeInfo(mazeReference);
        FindSpawnableSpaces();
        PickSpawnSpace();
        FindClosestSpace();
        GenerationLogic();

        return roomsToGenerate;
    }
    
    public virtual void FindSpawnableSpaces()
    {
        ClearLogic();
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

    public virtual void FindClosestSpace()
    {
        ClearLogic();
        (int, int)room = availableSpaces[spawnRoomIndex];
        for (int i = spawnDistanceMin; i < spawnDistanceMax + 1; i++)
        {
            if (Check((0, i), room))
            { closestSpace = (0, i); return; }
            if (Check((0, -i), room))
            { closestSpace = (0, -i); return; }
            if (Check((i, 0), room))
            { closestSpace = (i, 0); return; }
            if (Check((-i, 0), room))
            { closestSpace = (-i, 0); return; }
            
            for (int j = spawnDistanceMin; j < spawnDistanceMax - i + 1 ; j++)
            {
                if (Check((j, i), room))
                { closestSpace = (j, i); return; }
                if (Check((-j, i), room))
                { closestSpace = (-j, i); return; }
                if (Check((j, -i), room))
                { closestSpace = (j, -i); return; }
                if (Check((-j, -i), room))
                { closestSpace = (-j, -i); return; }
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
    
    public virtual bool Check((int, int) coordinates, (int, int) room)
    {
        return maze.ContainsKey(Coordinates.Add(coordinates, room));
    }
    
    public virtual void AssignMazeInfo(Dictionary<(int,int), RoomType> mazeReference)
    {
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
        roomsToGenerate.Clear();
    }
}
