using System.Collections;
using System.Collections.Generic;
using ColorFight;
using TwoCore;
using UnityEngine;

public class LockTab : TabContent
{
    private readonly TableDrawer<LockData> LockTable;

    public override void DoDraw()
    {
        Draw.BeginHorizontal();
        Draw.BeginVertical(Draw.SubContentStyle);
        Draw.SpaceAndLabelBoldBox("Items", Color.green);
        LockTable.DoDraw(GameConfig.Ins.LockDatas);
        if (Draw.Button("+", Color.green, Color.white, 150))
        {
            GameConfig.Ins.LockDatas.Add(new LockData
            {
                id = GameConfig.Ins.LockDatas.Count > 0 ? GameConfig.Ins.LockDatas[^1].id + 1 : 0,
                name = "New Lock",
            });
            Draw.SetDirty(GameConfig.Ins);
        }
        Draw.EndVertical();
        Draw.EndHorizontal();
    }

    public LockTab()
    {
        LockTable = new TableDrawer<LockData>();
        LockTable.AddCol("ID", 50, e => { e.id = Draw.Int(e.id, 30); Draw.Space(20); });
        LockTable.AddCol("Name", 150, e => { e.name = Draw.Text(e.name, 130); Draw.Space(20); });
        LockTable.AddCol("Material Lock", 150, e => { e.materialLock = Draw.Material(e.materialLock, false, 130); Draw.Space(20); });
        LockTable.AddCol("Material Key", 150, e => { e.materialKey = Draw.Material(e.materialKey, false, 130); Draw.Space(20); });
    }
}
