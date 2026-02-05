using ColorFight;
using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class EnemyTab : TabContent
{
    private readonly TableDrawer<CharacterData> enemyTable;

    public override void DoDraw()
    {
        Draw.BeginHorizontal();
        Draw.BeginVertical(Draw.SubContentStyle);
        Draw.SpaceAndLabelBoldBox("Enemies", Color.red);
        enemyTable.DoDraw(GameConfig.Ins.EnemyDatas);
        if (Draw.Button("+", Color.red, Color.white, 150))
        {
            GameConfig.Ins.EnemyDatas.Add(new CharacterData
            {
                id = GameConfig.Ins.EnemyDatas.Count > 0 ? GameConfig.Ins.EnemyDatas[^1].id + 1 : 1,
                name = $"New Enemy{(GameConfig.Ins.EnemyDatas.Count > 0 ? GameConfig.Ins.EnemyDatas[^1].id + 1 : 1)}",
                Type = EnemyType.Normal, // Default value, adjust as needed
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

    public EnemyTab()
    {
        enemyTable = new TableDrawer<CharacterData>();
        enemyTable.AddCol("ID", 50, e => { e.id = Draw.Int(e.id, 30); Draw.Space(20); });
        enemyTable.AddCol("Name", 120, e => { e.name = Draw.Text(e.name, 100); Draw.Space(20); });
        enemyTable.AddCol("Type", 100, e => { e.Type = (EnemyType)Draw.Enum(e.Type, 80); Draw.Space(20); });
        enemyTable.AddCol("Health", 80, e => { e.Health = Draw.Int(e.Health, 60); Draw.Space(20); });
        enemyTable.AddCol("Speed", 80, e => { e.Speed = Draw.Float(e.Speed, 60); Draw.Space(20); });
        enemyTable.AddCol("Damage", 80, e => { e.Damage = Draw.Int(e.Damage, 60); Draw.Space(20); });
        enemyTable.AddCol("Prefab", 150, e => { e.Prefab = Draw.Object(e.Prefab, 130); Draw.Space(20); });
    }
}
