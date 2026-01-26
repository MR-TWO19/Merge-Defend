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
    public int SizeGrid;
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

