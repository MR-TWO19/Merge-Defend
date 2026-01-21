using System.Collections.Generic;
using System.Linq;
using TwoCore;
using UnityEditor;
using UnityEngine;

public class GlobalTab : TabContent
{

    public override void DoDraw()
    {
        Draw.BeginVertical();
        Draw.LabelBoldBox("Global Config", Color.cyan);
        Draw.Space(20);
        GameConfig.Ins.IsCheatMode = Draw.ToggleField("Cheat", GameConfig.Ins.IsCheatMode);
        Draw.EndVertical();
    }
}
