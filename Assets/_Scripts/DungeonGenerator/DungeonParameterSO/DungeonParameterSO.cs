using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonParameterSO : ScriptableObject
{
    public int minRoomWidth = 10, minRoomHeigth = 10;
    public int dungeonWidth = 40, dungeonHeigth = 40;
    public int walkIteration = 15, walkLength = 20;
    public int roomCount = 2;
}
