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
    public List<LineSpawnData> LineSpawnDatas;
    public List<LineLevelData> Lines;
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
    public List<int> ItemIds;
    public ItemEventType EventType;
    public int LockId;
    public bool KeepTheKey;
}

[Serializable]
public class LockLevelData : BaseData
{
    public LockType lockType;
    public int Quantity;
    public Vector2Int Cell;
}

[Serializable]
public class LockCellData
{
    public LockType lockType;
    public int Quantity;
    public Vector2Int Cell;
}

[Serializable]
public class LineSpawnData
{
    public Vector2Int CellLineSpawn;
    public Vector2Int CellSpawn;
    public List<ItemControlLevelData> ItemControlLevelDatas;
}

public enum LockType
{
    None,
    LockedByCondition,
    Permanent
}

public enum ItemEventType
{
    None,
    Lock,
    Hint,
}