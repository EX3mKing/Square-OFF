using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public int mazeSizeX;
    public int mazeSizeY;
    public int numOfRooms;
    
    [Header("Prefabs / References")]
    public GameObject roomPrefab;
    public GameObject hallPrefab;
    public GameObject emptyPrefab;
    public GameObject nullPrefab;
    private Room[,] maze;
    
    private Dictionary<Room.Type, char> _roomTypesToChar = new Dictionary<Room.Type, char>()
    {
        {Room.Type.Normal, 'N'},
        {Room.Type.Spawn, 'S'},
        {Room.Type.Empty, '#'},
        {Room.Type.Rest, '+'},
        {Room.Type.Shop, 'P'},
        {Room.Type.Treasure, 'T'},
        {Room.Type.Hall, 'H'},
        {Room.Type.Boss, 'B'},
        {Room.Type.BossHall, 'b'},
        {Room.Type.Null, ' '}
    };
    
    private Dictionary<int, Room.Type> _numericalToRoomTypes = new Dictionary<int, Room.Type>()
    {
        {0, Room.Type.Spawn},
        {1, Room.Type.Normal},
        {2, Room.Type.Empty},
        {3, Room.Type.Hall},
        {4, Room.Type.Shop},
        {5, Room.Type.Treasure},
        {6, Room.Type.Rest},
        {7, Room.Type.Boss},
        {8, Room.Type.BossHall},
        {99, Room.Type.Null}
    };

    private Dictionary<Room.Type, GameObject> _roomPrefabs = new Dictionary<Room.Type, GameObject>();


    private void Start()
    {
        if (mazeSizeX * mazeSizeY < numOfRooms)
        {
            Debug.LogError("Number of rooms is greater than the maze size. Please adjust the values.");
            return;
        }
        GenerateMaze();
    }
    
    public void GenerateMaze()
    {
        // CREATE A BLANK MAZE
        _roomPrefabs = new Dictionary<Room.Type, GameObject>()
        {
            {Room.Type.Normal, roomPrefab},
            {Room.Type.Hall, hallPrefab},
            {Room.Type.Empty, emptyPrefab}
        };
        
        maze = new Room [mazeSizeX,mazeSizeY];
        
        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                maze[x, y] = Instantiate(nullPrefab, new Vector3(x * 20, 0, y * 20), Quaternion.identity)
                    .GetComponent<Room>();
                maze[x,y].type = Room.Type.Empty;
                maze[x,y].x = x;
                maze[x,y].y = y;
                maze[x,y].AssignNeighbours(Room.Type.Null, Room.Type.Null, Room.Type.Null, Room.Type.Null);
            }
        }

        // ADD NORMAL ROOMS
        int temp = numOfRooms;
        while (temp > 0)
        {
            int x = Random.Range(0, mazeSizeX);
            int y = Random.Range(0, mazeSizeY);
            maze[x, y].type = Room.Type.Normal;
            maze[x, y].x = x;
            maze[x, y].y = y;
            temp--;
        }

        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                if (x > 0) maze[x, y].left = maze[x - 1, y].type;
                if (x < mazeSizeX - 1) maze[x, y].right = maze[x + 1, y].type;
                if (y > 0) maze[x, y].down = maze[x, y - 1].type;
                if (y < mazeSizeY - 1) maze[x, y].up = maze[x, y + 1].type;
            }
        }
        
        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                if (maze[x,y].NeighbourTypeNumber(Room.Type.Normal) >= 2 && maze[x,y].type == Room.Type.Empty)
                {
                    maze[x, y].type = Room.Type.Hall;
                }
            }
        }

        // INSTANTIATE THE ROOMS
        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                Instantiate(_roomPrefabs[maze[x, y].type], new Vector3(x * 20, 0, y * 20), 
                    Quaternion.identity).GetComponent<Room>().Copy(maze[x,y]);
            }
        }
        
        // destroy the null rooms
        foreach (var room in maze)
        {
            Destroy(room.gameObject);
        }
    }
}
