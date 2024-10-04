using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DungeonValidation : MonoBehaviour
{
    DungeonData dungeonData;
    EnemyCounter enemyCounter;

    public GameObject greenGizmo, redGizmo;

    private GameObject[] gizmosList;

    public TextMeshProUGUI dungeonValidity, accessibleEnemy, enemyGenerated;

    public static int enemiesTotal;

    public List<Vector2Int> scannedTileForEnemy { get; set; } = new List<Vector2Int>();

    public bool showGizmo = true;
    public List<Vector2Int> enemyPosList { get; set; } = new List<Vector2Int>();

    void Start()
    {
        dungeonData = FindObjectOfType<DungeonData>();
        enemyCounter = FindObjectOfType<EnemyCounter>();
        enemiesTotal = 0;
        dungeonValidity.text = "Dungeon Validity - ";
        accessibleEnemy.text = "Accessible Enemy - ";
        enemyGenerated.text = "Enemy Generated - ";
        gizmosList = GameObject.FindGameObjectsWithTag("Gizmo");
    }

    public void RewriteText()
    {
        dungeonValidity.text = "Dungeon Validity - ";
        accessibleEnemy.text = "Accessible Enemy - ";
        enemyGenerated.text = "Enemy Generated - ";
    }

    public void StartValidation()
    {
        enemiesTotal = 0;
        scannedTileForEnemy = new List<Vector2Int>();
        enemyPosList = new List<Vector2Int>();
        int enemiesInDungeonData = 0;
        if (dungeonData == null)
        {
            Debug.Log("No dungeon data while validating dungeon");
            return;
        }

        for (int i = 0; i < dungeonData.Rooms.Count; i++)
        {
            Room currentRoom = dungeonData.Rooms[i];
            RoomGraphValidation currentRoomGraph = new RoomGraphValidation(currentRoom.FloorTiles);

            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>(currentRoom.FloorTiles);
            roomFloor.IntersectWith(dungeonData.Path);

            Dictionary<Vector2Int, Vector2Int> accesibleTile = currentRoomGraph.RunBFS(roomFloor.First(), currentRoom.PropPositions);
            Dictionary<Vector2Int, Vector2Int> enemiesRoomCount = currentRoomGraph.SeacrhEnemyBFS(roomFloor.First(), currentRoom.PropPositions, currentRoom.EnemiesInTheRoomPos);

            currentRoom.RoomAccesibilityValidation = accesibleTile.Keys.OrderBy(x => Guid.NewGuid()).ToList();
            currentRoom.RoomEnemyValidation = enemiesRoomCount.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            enemiesInDungeonData += currentRoom.EnemiesInTheRoomPos.Count;
        }
        Debug.Log("enemies hash map count - " + enemiesInDungeonData);
        Debug.Log("total musuh - " + enemiesTotal);
        Debug.Log("enemy counter - " + enemyCounter.enemyCount);

        enemyGenerated.text = "Enemy Generated - " + enemyCounter.enemyCount;
        accessibleEnemy.text = "Accessible Enemy - " + enemiesTotal;
        if (enemyCounter.enemyCount == enemiesTotal)
        {
            dungeonValidity.text = "Dungeon Validity - Valid";
        }
        else
        {
            dungeonValidity.text = "Dungeon Validity - Not Valid";
        }

        DrawVisualization();
    }

    private void DrawVisualization()
    {
        foreach (Room room in dungeonData.Rooms)
        {
            foreach (Vector2Int pos in room.RoomAccesibilityValidation)
            {
                Instantiate(greenGizmo, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
            }

            foreach (Vector2Int pos in room.RoomEnemyValidation)
            {
                Instantiate(redGizmo, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
            }
        }
    }

    public void DeleteVisualization()
    {
        gizmosList = GameObject.FindGameObjectsWithTag("Gizmo");

        foreach (GameObject item in gizmosList)
        {
            Destroy(item);
        }
    }
}

public class RoomGraphValidation
{

    public static List<Vector2Int> fourDirections = new()
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    Dictionary<Vector2Int, List<Vector2Int>> graph = new Dictionary<Vector2Int, List<Vector2Int>>();

    public RoomGraphValidation(HashSet<Vector2Int> roomFloor)
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

    public Dictionary<Vector2Int, Vector2Int> RunBFS(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes)
    {
        //BFS related variuables
        Queue<Vector2Int> nodesToVisit = new Queue<Vector2Int>();
        nodesToVisit.Enqueue(startPos);

        HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
        visitedNodes.Add(startPos);

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

    public Dictionary<Vector2Int, Vector2Int> SeacrhEnemyBFS(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes, HashSet<Vector2Int> enemyNodes)
    {
        //BFS related variuables
        Queue<Vector2Int> nodesToVisit = new Queue<Vector2Int>();
        nodesToVisit.Enqueue(startPos);

        HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
        visitedNodes.Add(startPos);

        Dictionary<Vector2Int, Vector2Int> map = new Dictionary<Vector2Int, Vector2Int>();

        if (enemyNodes.Contains(startPos))
        {
            map[startPos] = startPos;
            DungeonValidation.enemiesTotal++;
        }

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
                    if (enemyNodes.Contains(neighbourPosition))
                    {
                        map[neighbourPosition] = node;
                        DungeonValidation.enemiesTotal++;
                    }
                }
            }
        }

        return map;
    }
}