using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;
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

    private struct UpgradeStep
    {
        public int Level;
        public float Multiplier;
        public int Cost;

        public UpgradeStep(int level, float multiplier, int cost)
        {
            Level = level;
            Multiplier = multiplier;
            Cost = cost;
        }
    }

    private static readonly UpgradeStep[] UpgradeSteps =
    {
        new UpgradeStep(1, 1.00f, 50),
        new UpgradeStep(2, 1.10f, 70),
        new UpgradeStep(3, 1.21f, 90),
        new UpgradeStep(4, 1.33f, 120),
        new UpgradeStep(5, 1.46f, 150),
        new UpgradeStep(6, 1.60f, 190),
        new UpgradeStep(7, 1.75f, 230),
        new UpgradeStep(8, 1.91f, 280),
        new UpgradeStep(9, 2.08f, 330),
        new UpgradeStep(10, 2.26f, 400),
        new UpgradeStep(15, 3.05f, 650),
        new UpgradeStep(20, 3.75f, 1000),
        new UpgradeStep(25, 4.35f, 1500),
        new UpgradeStep(30, 4.85f, 2100),
        new UpgradeStep(35, 5.25f, 2800),
        new UpgradeStep(40, 5.55f, 3600),
        new UpgradeStep(45, 5.80f, 4500),
        new UpgradeStep(50, 6.00f, int.MaxValue),
    };

    protected internal override void OnInit()
    {
        base.OnInit();
        Level = 1;
        SfxOn = true;
        MusicOn = true;
        Coin = 1000;
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
                Level = 1,
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

    public bool UpgradeCharacter(int id)
    {
        var info = GetCharacter(id);
        if (info == null) return false;

        int nextLevel = info.Level + 1;
        if (nextLevel > 50) return false;

        GetUpgradeData(info.Level, out _, out var cost);
        if (cost == int.MaxValue || Coin < cost) return false;

        Coin -= cost;
        info.Level = nextLevel;
        info.Health = Mathf.RoundToInt(info.Health + 1f);
        info.Damage = Mathf.RoundToInt(info.Damage + 1f);

        SaveAndNotify("coin");
        Save();
        return true;
    }

    public bool TryGetUpgradePreview(int id, out int nextLevel, out float nextHealth, out int nextDamage, out int cost)
    {
        nextLevel = 0;
        nextHealth = 0f;
        nextDamage = 0;
        cost = 0;

        var info = GetCharacter(id);
        if (info == null) return false;

        nextLevel = info.Level + 1;
        if (nextLevel > 50)
        {
            nextLevel = info.Level;
            nextHealth = info.Health;
            nextDamage = info.Damage;
            cost = int.MaxValue;
            return false;
        }

        GetUpgradeData(info.Level, out _, out cost);
        nextHealth = Mathf.RoundToInt(info.Health + 1f);
        nextDamage = Mathf.RoundToInt(info.Damage + 1f);
        return true;
    }

    private static void GetUpgradeData(int level, out float multiplier, out int cost)
    {
        if (level <= UpgradeSteps[0].Level)
        {
            multiplier = UpgradeSteps[0].Multiplier;
            cost = UpgradeSteps[0].Cost;
            return;
        }

        for (int i = 0; i < UpgradeSteps.Length; i++)
        {
            if (UpgradeSteps[i].Level == level)
            {
                multiplier = UpgradeSteps[i].Multiplier;
                cost = UpgradeSteps[i].Cost;
                return;
            }
        }

        UpgradeStep lower = UpgradeSteps[0];
        UpgradeStep upper = UpgradeSteps[^1];
        for (int i = 0; i < UpgradeSteps.Length - 1; i++)
        {
            if (level > UpgradeSteps[i].Level && level < UpgradeSteps[i + 1].Level)
            {
                lower = UpgradeSteps[i];
                upper = UpgradeSteps[i + 1];
                break;
            }
        }

        float t = (level - lower.Level) / (float)(upper.Level - lower.Level);
        multiplier = Mathf.Lerp(lower.Multiplier, upper.Multiplier, t);
        cost = Mathf.RoundToInt(Mathf.Lerp(lower.Cost, upper.Cost, t));
    }
}
