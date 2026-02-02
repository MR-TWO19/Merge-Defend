using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine.Scripting;


[Preserve]
public class UserSaveData : BaseUserData
{
    public static UserSaveData Ins => LocalData.Get<UserSaveData>();

    public int Level;
    public bool MusicOn;
    public bool SfxOn;

    public int Coin;

    public List<CharacterInfo> CharacterInfos;

    protected internal override void OnInit()
    {
        base.OnInit();
        Level = 1;
        SfxOn = true;
        MusicOn = true;
        InitCharacterInfos();
    }

    public void NextLevel()
    {
        Level++;
        Save();
    }
    
    public void SetLevel(int level)
    {
        Level = level;
        Save();
    }

    public void AddCoin(int value)
    {
        Coin += value;
        SaveAndNotify("coin");
    }

    private void InitCharacterInfos()
    {
        CharacterInfos ??= new List<CharacterInfo>();

        foreach (var item in GameConfig.Ins.HeroDatas)
        {
            var info = new CharacterInfo()
            {
                CharId = item.id,
                Health = item.Health,
                Damage = item.Damage,
                Speed = item.Speed
            };
            CharacterInfos.Add(info);
        }
    }

    public CharacterInfo GetCharacter(int id)
    {
        return CharacterInfos.Find(_ => _.CharId == id);
    }
}
