using ColorFight;
using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class HeroTab : TabContent
{
    private readonly TableDrawer<CharacterData> heroTable;

    public override void DoDraw()
    {
        Draw.BeginHorizontal();
        Draw.BeginVertical(Draw.SubContentStyle);
        Draw.SpaceAndLabelBoldBox("Heroes", Color.cyan);
        heroTable.DoDraw(GameConfig.Ins.HeroDatas);
        if (Draw.Button("+", Color.cyan, Color.white, 150))
        {
            GameConfig.Ins.HeroDatas.Add(new CharacterData
            {
                id = GameConfig.Ins.HeroDatas.Count > 0 ? GameConfig.Ins.HeroDatas[^1].id + 1 : 1,
                name = $"New Hero{(GameConfig.Ins.HeroDatas.Count > 0 ? GameConfig.Ins.HeroDatas[^1].id + 1 : 1)}",
                Type = EnemyType.Normal,
                Health = 10,
                Speed = 2f,
                Damage = 1,
                Prefab = null,
            });
            Draw.SetDirty(GameConfig.Ins);
        }
        Draw.EndVertical();
        Draw.EndHorizontal();
    }

    public HeroTab()
    {
        heroTable = new TableDrawer<CharacterData>();
        heroTable.AddCol("ID", 50, e => { e.id = Draw.Int(e.id, 30); Draw.Space(20); });
        heroTable.AddCol("Name", 120, e => { e.name = Draw.Text(e.name, 100); Draw.Space(20); });
        heroTable.AddCol("Type", 100, e => { e.Type = (EnemyType)Draw.Enum(e.Type, 80); Draw.Space(20); });
        heroTable.AddCol("Health", 80, e => { e.Health = Draw.Int(e.Health, 60); Draw.Space(20); });
        heroTable.AddCol("Speed", 80, e => { e.Speed = Draw.Float(e.Speed, 60); Draw.Space(20); });
        heroTable.AddCol("Damage", 80, e => { e.Damage = Draw.Int(e.Damage, 60); Draw.Space(20); });
        heroTable.AddCol("Prefab", 150, e => { e.Prefab = Draw.Object(e.Prefab, 130); Draw.Space(20); });
    }
}
