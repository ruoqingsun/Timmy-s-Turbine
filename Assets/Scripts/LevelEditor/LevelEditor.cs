using UnityEngine;
using System.Collections;

public class LevelEditor : MonoBehaviour {

    public bool GridInfo_showInfo;

    public float TerrainInfo_pathHeight;
    public int TerrainInfo_elevationInterval;
    public int TerrainInfo_elevationMax;

    public float EnemyManager_spawnTime;
    public float EnemyManager_timeBetweenNextWave;
    public int[] EnemyManager_spawnLimits;
    public int[] EnemyManager_spawnHealth;
    public int[] EnemyManager_waterAmount;

}
