using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0), //right
        new Vector2Int(0,-1),//down
        new Vector2Int(-1,0) //left
    };

    public static List<Vector2Int> diagonallDirectionList = new List<Vector2Int>
    {
        new Vector2Int(1,1), //up-right
        new Vector2Int(1,-1), //right-down
        new Vector2Int(-1,-1),//down-left
        new Vector2Int(-1,1) //left-up

    };

    public static List<Vector2Int> eightDirectionLIst = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,1), //up-right
        new Vector2Int(1,0), //right
        new Vector2Int(1,-1), //right-down
        new Vector2Int(0,-1),//down
        new Vector2Int(-1,-1),//down-left
        new Vector2Int(-1,0), //left
        new Vector2Int(-1,1) //left-up
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
}
