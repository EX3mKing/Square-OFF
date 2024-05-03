using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    // [Header("Maze Settings")]
    // public int mazeSizeX;
    // public int mazeSizeY;
    // public int numOfRooms;
    
    [Header("Prefabs / References")] 
    public GameObject hallPrefab;
    public GameObject emptyPrefab;
    public GameObject roomPrefab;
    public GameObject spawnPrefab;
    
    public List<Room> roomsToGenerate;
    
    private Dictionary<(int,int), RoomType> _maze = new Dictionary<(int, int), RoomType>();
    private Dictionary<(int,int), RoomType> _spawnSpace = new Dictionary<(int, int), RoomType>();
    private Dictionary<RoomType, GameObject> _roomPrefabs;
    

    private void Awake()
    {
        _roomPrefabs = new Dictionary<RoomType, GameObject>()
        {
            {RoomType.Normal, roomPrefab},
            {RoomType.Spawn, emptyPrefab},
            {RoomType.Hall, hallPrefab},
            {RoomType.Empty, emptyPrefab}
        };
    }


    private void Start()
    {
        GenerateMaze();
    }
    
    public void GenerateMaze()
    {
        _maze[(0, 0)] = RoomType.Spawn;
        int index = 0;
        foreach (var room in roomsToGenerate)
        {
            _maze = room.Generate(_maze, index);
            index++;
        }
        
        InstantiateRooms(_maze);
    }
    
    private void InstantiateRooms(Dictionary<(int,int), RoomType> maze)
    {
        foreach (var room in maze)
        {
            Instantiate(_roomPrefabs[room.Value], new Vector3(room.Key.Item1 * 20, 0, 
                    room.Key.Item2 * 20), Quaternion.identity);
        }
    }
}
