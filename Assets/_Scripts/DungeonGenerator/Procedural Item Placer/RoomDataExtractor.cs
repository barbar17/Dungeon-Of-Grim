using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomDataExtractor : MonoBehaviour
{
    private DungeonData dungeonData;

    public UnityEvent OnFinishedRoomProcessing;

    private void Start()
    {
        dungeonData = FindObjectOfType<DungeonData>();
    }

    public void ProcessRooms()
    {
        if (dungeonData == null)
            dungeonData = FindObjectOfType<DungeonData>();


        foreach (Room room in dungeonData.Rooms)
        {
            //find corener, near wall and inner tiles
            foreach (Vector2Int tilePosition in room.FloorTiles)
            {
                string neighboursBinaryType = "";
                foreach (var direction in Direction2D.eightDirectionLIst)
                {
                    var neighbourPosition = tilePosition + direction;
                    if (room.FloorTiles.Contains(neighbourPosition))
                    {
                        neighboursBinaryType += "1";
                    }
                    else
                    {
                        neighboursBinaryType += "0";
                    }
                }

                int typeASInt = Convert.ToInt32(neighboursBinaryType, 2);
                if (FloorTypesHelper.NearWallTilesUp.Contains(typeASInt))
                {
                    room.NearWallTilesUp.Add(tilePosition);
                }
                else if (FloorTypesHelper.NearWallTilesDown.Contains(typeASInt))
                {
                    room.NearWallTilesDown.Add(tilePosition);
                }
                else if (FloorTypesHelper.NearWallTilesLeft.Contains(typeASInt))
                {
                    room.NearWallTilesLeft.Add(tilePosition);
                }
                else if (FloorTypesHelper.NearWallTilesRight.Contains(typeASInt))
                {
                    room.NearWallTilesRight.Add(tilePosition);
                }
                else if (FloorTypesHelper.CornerTiles.Contains(typeASInt))
                {
                    room.CornerTiles.Add(tilePosition);
                }
                else if (FloorTypesHelper.InnerTiles.Contains(typeASInt))
                {
                    room.InnerTiles.Add(tilePosition);
                }
                else if (FloorTypesHelper.NarrowTiles.Contains(typeASInt))
                {
                    room.NarrowTiles.Add(tilePosition);
                }
            }

            room.NearWallTilesUp.ExceptWith(dungeonData.Path);
            room.NearWallTilesDown.ExceptWith(dungeonData.Path);
            room.NearWallTilesLeft.ExceptWith(dungeonData.Path);
            room.NearWallTilesRight.ExceptWith(dungeonData.Path);
            room.NarrowTiles.ExceptWith(dungeonData.Path);
            room.CornerTiles.ExceptWith(dungeonData.Path);
            room.InnerTiles.ExceptWith(dungeonData.Path);
        }
        //OnFinisheRoomProcessing?.Invoke();
        Invoke("RunEvent", 1);
    }

    public void RunEvent()
    {
        OnFinishedRoomProcessing?.Invoke();
    }
}