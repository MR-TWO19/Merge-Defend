using System.Collections.Generic;
using UnityEngine;
using ColorFight;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ConfigBase<GameConfig>
{
    public bool IsCheatMode = false;

    public List<ItemData> ItemDatas;
    public List<LockData> LockDatas;
    public ItemData GetItemData(int id)
    {
        var data = ItemDatas.Find(_ => _.id == id);
        return data;
    }

    public LockData GetLockData(int id)
    {
        var data = LockDatas.Find(_ => _.id == id);
        return data;
    }

}