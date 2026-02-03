using System.Collections;
using System.Collections.Generic;
using ColorFight;
using TwoCore;
using UnityEngine;

public class ItemTab : TabContent
{
    private readonly TableDrawer<ItemData> itemTable;

    public override void DoDraw()
    {
        Draw.BeginHorizontal();
        Draw.BeginVertical(Draw.SubContentStyle);
        Draw.SpaceAndLabelBoldBox("Items", Color.green);
        itemTable.DoDraw(GameConfig.Ins.ItemDatas);
        if (Draw.Button("+", Color.green, Color.white, 150))
        {
            GameConfig.Ins.ItemDatas.Add(new ItemData
            {
                id = GameConfig.Ins.ItemDatas.Count > 0 ? GameConfig.Ins.ItemDatas[^1].id + 1 : 0,
                name = "New Item",
                Prefab = null,
            });
            Draw.SetDirty(GameConfig.Ins);
        }
        Draw.EndVertical();
        Draw.EndHorizontal();
    }

    public ItemTab()
    {
        itemTable = new TableDrawer<ItemData>();
        itemTable.AddCol("ID", 50, e => { e.id = Draw.Int(e.id, 30); Draw.Space(20); });
        itemTable.AddCol("Name", 150, e => { e.name = Draw.Text(e.name, 130); Draw.Space(20); });
        itemTable.AddCol("Prefab", 150, e => { e.Prefab = Draw.Object(e.Prefab, 130); Draw.Space(20); });
        itemTable.AddCol("HeroID", 150, e => { e.HeroID = Draw.Int(e.HeroID, 130); Draw.Space(20); });
        itemTable.AddCol("Material", 150, e => { e.material = Draw.Material(e.material, false, 130); Draw.Space(20); });
    }
}
