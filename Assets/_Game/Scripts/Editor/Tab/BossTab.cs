using ColorFight;
using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class BossTab : TabContent
{
    private readonly TableDrawer<CharacterData> bossTable;

    public override void DoDraw()
    {
        Draw.BeginHorizontal();
        Draw.BeginVertical(Draw.SubContentStyle);
        Draw.SpaceAndLabelBoldBox("Bosses", new Color(0.6f, 0.2f, 0.8f));
        bossTable.DoDraw(GameConfig.Ins.BossDatas);
        if (Draw.Button("+", new Color(0.6f, 0.2f, 0.8f), Color.white, 150))
        {
            GameConfig.Ins.BossDatas.Add(new CharacterData
            {
                id = GameConfig.Ins.BossDatas.Count > 0 ? GameConfig.Ins.BossDatas[^1].id + 1 : 1,
                name = $"New Boss{(GameConfig.Ins.BossDatas.Count > 0 ? GameConfig.Ins.BossDatas[^1].id + 1 : 1)}",
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

    public BossTab()
    {
        bossTable = new TableDrawer<CharacterData>();
        bossTable.AddCol("ID", 50, e => { e.id = Draw.Int(e.id, 30); Draw.Space(20); });
        bossTable.AddCol("Name", 120, e => { e.name = Draw.Text(e.name, 100); Draw.Space(20); });
        bossTable.AddCol("Type", 100, e => { e.Type = (EnemyType)Draw.Enum(e.Type, 80); Draw.Space(20); });
        bossTable.AddCol("Health", 80, e => { e.Health = Draw.Int(e.Health, 60); Draw.Space(20); });
        bossTable.AddCol("Speed", 80, e => { e.Speed = Draw.Float(e.Speed, 60); Draw.Space(20); });
        bossTable.AddCol("Damage", 80, e => { e.Damage = Draw.Int(e.Damage, 60); Draw.Space(20); });
        bossTable.AddCol("Prefab", 150, e => { e.Prefab = Draw.Object(e.Prefab, 130); Draw.Space(20); });
    }
}
