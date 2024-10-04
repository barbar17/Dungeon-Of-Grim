using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBypass : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator;
    public bool canBypassLevel = true;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!canBypassLevel)
            {
                return;
            }

            if (DungeonGenerator.dungeonLevel < AgentPlacer.bossLevel)
            {
                dungeonGenerator.GoNextLevel();
            }
            else
            {
                SceneController.instance.LoadFinishedGameScene();
            }
        }

    }

    public void CanBypass()
    {
        canBypassLevel = true;
    }

    public void cannotBypass()
    {
        canBypassLevel = false;
    }
}
