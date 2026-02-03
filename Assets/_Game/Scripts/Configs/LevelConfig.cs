using System;
using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig")]
public class LevelConfig : ConfigBase<LevelConfig>
{
    public List<LevelData> levelDatas;
}


[Serializable]
public class LevelData
{
    public int CoinWin;
    public int CoinLose;

    public int SizeGrid;
    public List<Wave> Waves;
    public List<ItemControlLevelData> ItemControlLevelDatas;
}

[Serializable]
public class LineLevelData
{
    public List<TargetLevelData> Targets;
}

[Serializable]
public class TargetLevelData
{
    public int ItemId;
}

[Serializable]
public class ItemControlLevelData
{
    public Vector2Int Cell;
}

[System.Serializable]
public class Wave
{
    public List<EnemyLevelData> Enemies = new List<EnemyLevelData>();
    public float SpawnInterval = 2f;
}

[System.Serializable]
public class EnemyLevelData
{
    public int total = 1;
    public CharacterInfo enemyData;
    public float SpawnInterval = 0.5f;
}

