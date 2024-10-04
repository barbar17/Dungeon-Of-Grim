using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine;

public class AgentPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private List<GameObject> enemyPrefab;

    [SerializeField]
    private GameObject bossPrefab;

    public static int bossLevel = 5;

    private int playerRoomIndex, bossRoomIndex;

    [SerializeField]
    private CinemachineVirtualCamera vCamera;

    [SerializeField]
    private static int roomEnemiesCount = 4;

    public UnityEvent OnFinishedAgentPlacement;

    DungeonData dungeonData;

    [SerializeField]
    private bool showGizmo = false;

    private void Start()
    {
        dungeonData = FindObjectOfType<DungeonData>();
        playerRoomIndex = 0;
    }

    public void PlaceAgents()
    {
        if (dungeonData == null)
            return;
        bossRoomIndex = dungeonData.Rooms.Count - 1;

        //loop setiap room yang ada
        for (int i = 0; i < dungeonData.Rooms.Count; i++)
        {
            // mengetahui room tiles yang dapat dijangkau dari path
            Room room = dungeonData.Rooms[i];
            RoomGraph roomGraph = new RoomGraph(room.FloorTiles);

            // mencari path pada ruang yang sedang di proses
            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>(room.FloorTiles);
            //mencari tiles yang dimiliki oleh path pada room
            roomFloor.IntersectWith(dungeonData.Path);

            //jalankan BFS untuk mencari semua tiles yang dapat diakses melauli path
            Dictionary<Vector2Int, Vector2Int> roomMap = roomGraph.RunBFS(roomFloor.First(), room.PropPositions);

            //posisi yang dapat dijangkau + path == posisi penempatan musuh
            room.PositionsAccessibleFromPath = roomMap.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            //apakah ruangan akan diisi musuh?
            if (DungeonGenerator.dungeonLevel == bossLevel)
            {
                if (i != playerRoomIndex && i != bossRoomIndex)
                {
                    PlaceEnemies(room, roomEnemiesCount);
                }

                if (i == bossRoomIndex)
                {
                    GameObject boss = Instantiate(bossPrefab);
                    boss.transform.localPosition = dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f;
                    room.EnemiesInTheRoom.Add(boss);
                    room.EnemiesInTheRoomPos.Add(new Vector2Int((int)boss.transform.position.x, (int)boss.transform.position.y));
                }
            }
            else
            {
                if (i != playerRoomIndex)
                {
                    PlaceEnemies(room, roomEnemiesCount);
                }
            }

            //menempatkan player pada ruangan
            if (i == playerRoomIndex)
            {
                GameObject player = Instantiate(playerPrefab);
                player.transform.localPosition = dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f;

                //kamera mengikuti player
                vCamera.Follow = player.transform;
                vCamera.LookAt = player.transform;
                dungeonData.PlayerReference = player;
            }
        }
        OnFinishedAgentPlacement?.Invoke();
    }

    /// <summary>
    /// Places enemies in the positions accessible from the path
    /// </summary>
    /// <param name="room"></param>
    /// <param name="enemysCount"></param>
    /// 

    private void PlaceEnemies(Room room, int enemysCount)
    {
        for (int i = 0; i < enemysCount; i++)
        {
            if (room.PositionsAccessibleFromPath.Count <= i)
            {
                return;
            }

            int enemyIndex = UnityEngine.Random.Range(0, enemyPrefab.Count());
            GameObject enemy = Instantiate(enemyPrefab[enemyIndex]);
            enemy.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[i] + Vector2.one * 0.5f;
            room.EnemiesInTheRoom.Add(enemy);

            //kebutuhan benchmark menyimpan posisi musuh;
            room.EnemiesInTheRoomPos.Add(new Vector2Int((int)enemy.transform.position.x, (int)enemy.transform.position.y));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (dungeonData == null || showGizmo == false)
            return;
        foreach (Room room in dungeonData.Rooms)
        {
            Color color = Color.green;
            color.a = 0.3f;
            Gizmos.color = color;

            foreach (Vector2Int pos in room.PositionsAccessibleFromPath)
            {
                Gizmos.DrawCube((Vector2)pos + Vector2.one * 0.5f, Vector2.one);
            }
        }
    }
}

public class RoomGraph
{
    public static List<Vector2Int> fourDirections = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    Dictionary<Vector2Int, List<Vector2Int>> graph = new Dictionary<Vector2Int, List<Vector2Int>>();

    public RoomGraph(HashSet<Vector2Int> roomFloor)
    {
        foreach (Vector2Int pos in roomFloor)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();
            foreach (Vector2Int direction in fourDirections)
            {
                Vector2Int newPos = pos + direction;
                if (roomFloor.Contains(newPos))
                {
                    neighbours.Add(newPos);
                }
            }
            graph.Add(pos, neighbours);
        }
    }

    /// <summary>
    /// Creates a map of reachable tiles in our dungeon.
    /// </summary>
    /// <param name="startPos">Door position or tile position on the path between rooms inside this room</param>
    /// <param name="occupiedNodes"></param>
    /// <returns></returns>
    public Dictionary<Vector2Int, Vector2Int> RunBFS(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes)
    {
        //BFS related variuables
        Queue<Vector2Int> nodesToVisit = new Queue<Vector2Int>();
        nodesToVisit.Enqueue(startPos);

        HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
        visitedNodes.Add(startPos);

        //The dictionary that we will return 
        Dictionary<Vector2Int, Vector2Int> map = new Dictionary<Vector2Int, Vector2Int>();
        map.Add(startPos, startPos);

        while (nodesToVisit.Count > 0)
        {
            //get the data about specific position
            Vector2Int node = nodesToVisit.Dequeue();
            List<Vector2Int> neighbours = graph[node];

            //loop through each neighbour position
            foreach (Vector2Int neighbourPosition in neighbours)
            {
                //add the neighbour position to our map if it is valid
                if (visitedNodes.Contains(neighbourPosition) == false &&
                    occupiedNodes.Contains(neighbourPosition) == false)
                {
                    nodesToVisit.Enqueue(neighbourPosition);
                    visitedNodes.Add(neighbourPosition);
                    map[neighbourPosition] = node;
                }
            }
        }

        return map;
    }
}
