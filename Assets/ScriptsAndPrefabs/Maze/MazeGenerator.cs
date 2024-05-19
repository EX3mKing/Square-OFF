using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    public GameObject hallPrefab;
    public GameObject emptyPrefab;
    public GameObject roomPrefab;
    public GameObject spawnPrefab;
    
    public List<Room> sectionsToGenerate;
    
    private Dictionary<(int,int), RoomType> _maze = new Dictionary<(int, int), RoomType>();
    private Dictionary<int, (int, int)[]> _mazeSections = new Dictionary<int, (int, int)[]>();
    private Dictionary<RoomType, GameObject> _roomPrefabs;
    private int _sectionIndex = 0; // index 0 is spawn, index 1 are halls, index i is room

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
        SpawnRoom(new KeyValuePair<(int, int), RoomType>((0, 0), RoomType.Spawn), 0);
        _mazeSections.Add(0, new (int, int)[] {(0, 0)});
        _sectionIndex = 2;
        
        foreach (var section in sectionsToGenerate)
        {
            (int, int)[] temp = new (int, int)[section.roomSize];
            int i = 0;
            foreach (var room in section.Generate(_maze))
            {
                _maze.Add(room.Key, room.Value);
                SpawnRoom(room, _sectionIndex);
                temp[i] = room.Key;
                i++;
            }
            _mazeSections.Add(_sectionIndex, temp);
            _sectionIndex++;
        }

        List<(int, int)> hallSpaces;
        for (int i = 1; i < _mazeSections.Count; i++)
        {
            
        }
    }

    private void SpawnRoom(KeyValuePair<(int, int), RoomType> room, int index)
    {
        GameObject roomGO = Instantiate(_roomPrefabs[room.Value], new Vector3(room.Key.Item1 * 20, 0, 
            room.Key.Item2 * 20), Quaternion.identity).gameObject;
        roomGO.transform.parent = transform;
        roomGO.name = $"ID_{index} {room.Value.ToString()} Room ({room.Key.Item1}, {room.Key.Item2})";
    }
}
