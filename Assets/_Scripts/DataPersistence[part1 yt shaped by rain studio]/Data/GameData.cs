using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public string weaponUsed;
    public bool isFirstSession;
    public bool isGameFinished;


    //default value ketika GameData baru dibuat
    public GameData()
    {
        this.deathCount = 0;
        this.weaponUsed = "sword";
        this.isFirstSession = true;
        this.isGameFinished = false;
    }
}
