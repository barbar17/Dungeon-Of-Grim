using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using TMPro;

public class BenchmarkDungeonGenerator : MonoBehaviour
{
    public TextMeshProUGUI levelDisplay;
    public List<DungeonParameterSO> dungeonParameters;
    public static int dungeonLevel;
    public int dungeonParameterIndex, regenerate = 0;

    [SerializeField]
    private TilePlacer tilePlacer = null;

    [SerializeField]
    private Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;

    private DungeonData dungeonData;

    public static List<Vector2Int> roomCenters = new List<Vector2Int>();

    public UnityEvent OnFinishedRoomGeneration, OnStartRoomGeneration;

    public static Vector2Int dungeonCenter;

    public bool canGenerate;

    public DungeonActivation dungeonActivation;

    private void Awake()
    {
        dungeonData = FindObjectOfType<DungeonData>();
        if (dungeonData == null)
            dungeonData = gameObject.AddComponent<DungeonData>();

        dungeonLevel = 0;
        dungeonParameterIndex = 0;
        canGenerate = true;

        levelDisplay.text = "level - " + (dungeonLevel + 1);

        dungeonData.Reset();
        roomCenters.Clear();
        tilePlacer.Reset();
    }

    public void CanGenerate()
    {
        canGenerate = true;
    }

    public void CannotGenerate()
    {
        canGenerate = false;
    }

    public void Reset()
    {
        dungeonData.Reset();
        roomCenters.Clear();
        tilePlacer.Reset();

        dungeonLevel = 0;
        regenerate = 0;
        canGenerate = true;
    }

    public void GenerateSmall()
    {
        if (!canGenerate)
        {
            return;
        }

        Reset();
        dungeonParameterIndex = 0;
        Generate(dungeonParameterIndex);
        CannotGenerate();
    }

    public void GenerateMedium()
    {
        if (!canGenerate)
        {
            return;
        }

        Reset();
        dungeonParameterIndex = 1;
        Generate(dungeonParameterIndex);
        CannotGenerate();
    }

    public void GenerateLarge()
    {
        if (!canGenerate)
        {
            return;
        }

        Reset();
        dungeonParameterIndex = 2;
        Generate(dungeonParameterIndex);
        CannotGenerate();
    }

    private void Generate(int dungeonParameterIndex)
    {
        OnStartRoomGeneration?.Invoke();
        dungeonActivation.StartTotalTime();

        CreateRooms(dungeonParameters[dungeonParameterIndex]);

        Debug.Log(dungeonData.Rooms.Count);

        if (dungeonData.Rooms.Count != dungeonParameters[dungeonParameterIndex].roomCount)
        {
            Regenerate();
        }
        else if (dungeonData.Rooms.Count <= 1)
        {
            Regenerate();
        }
        else
        {
            OnFinishedRoomGeneration?.Invoke();
        }
    }

    private void Regenerate()
    {
        Debug.LogWarning("!!DUNGEON REGENERATE!!");
        regenerate += 1;
        dungeonData.Reset();
        roomCenters.Clear();
        tilePlacer.Reset();
        Generate(dungeonParameterIndex);
    }

    private void CreateRooms(DungeonParameterSO dungeonParameter)
    {
        //untuk kebutuhan benchmark mencari titik tengah dungeon
        BoundsInt dungeonLocation = new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonParameter.dungeonWidth, dungeonParameter.dungeonHeigth, 0));
        dungeonCenter = new Vector2Int(Mathf.RoundToInt(dungeonLocation.center.x), Mathf.RoundToInt(dungeonLocation.center.y));


        var roomsList =
            BSP.RunBinarySpacePartitioning(dungeonLocation, dungeonParameter.minRoomWidth, dungeonParameter.minRoomHeigth, dungeonParameters[dungeonParameterIndex].roomCount);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateRoomsRandomly(roomsList, dungeonParameters[dungeonParameterIndex]);

        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);
        dungeonData.Path.UnionWith(corridors);
        tilePlacer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilePlacer);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList, DungeonParameterSO dungeonParameter)
    {
        HashSet<Vector2Int> dungeonfloor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RandomWalk.RunRandomWalk(dungeonParameter.walkIteration, dungeonParameter.walkLength, roomCenter);
            HashSet<Vector2Int> roomDataFloor = new();

            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin + offset) && position.y <= (roomBounds.yMax - offset))
                {
                    dungeonfloor.Add(position);
                    roomDataFloor.Add(position);
                }
            }
            dungeonData.Rooms.Add(new Room(roomCenter, roomDataFloor));
        }
        return dungeonfloor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            //tentukan lebar coridor pada for x
            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    corridor.Add(position + new Vector2Int(x, y));
                }
            }
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            for (int x = -1; x < 1; x++)
            {
                //tentukan lebar coridor pada for y
                for (int y = -1; y < 1; y++)
                {
                    corridor.Add(position + new Vector2Int(x, y));
                }
            }
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;

    }
}