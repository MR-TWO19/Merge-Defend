using System.Collections.Generic;
using UnityEngine;
using ColorFight;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ConfigBase<GameConfig>
{
    public bool IsCheatMode = false;

    public List<ItemData> ItemDatas;
    public List<CharacterData> EnemyDatas;
    public List<CharacterData> HeroDatas;

    public ItemData GetItemData(int id)
    {
        var data = ItemDatas.Find(_ => _.id == id);
        return data;
    }

    public ItemData GetRondomItemData()
    {
        if (ItemDatas == null || ItemDatas.Count == 0)
            return null;
        int index = UnityEngine.Random.Range(0, ItemDatas.Count);
        return ItemDatas[index];
    }

    public CharacterData GetEnemyData(int id)
    {
        return EnemyDatas.Find(_ => _.id == id);
    }

    public CharacterData GetHeroData(int id)
    {
        return HeroDatas.Find(_ => _.id == id);
    }

}