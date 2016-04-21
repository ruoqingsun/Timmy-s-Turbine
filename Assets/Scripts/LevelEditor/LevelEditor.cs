using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class EnemyManagerEditor
{
    public int spawnLimit;
    public int spawnHealth;
    public int waterAmount;
}

public class LevelEditor : MonoBehaviour
{

    public bool GridInfo_showInfo;

    public float TerrainInfo_pathHeight;
    public int TerrainInfo_elevationInterval;
    public int TerrainInfo_elevationMax;

    public float EnemyManager_spawnTime;
    public float EnemyManager_timeBetweenNextWave;
    public EnemyManagerEditor[] EnemyEditor;

}
