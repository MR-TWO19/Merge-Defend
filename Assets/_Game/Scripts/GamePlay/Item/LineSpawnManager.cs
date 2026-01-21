using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawnManager : MonoBehaviour
{
    [SerializeField] private List<LineSpawn> lineSpawns;
    [SerializeField] private LineSpawn prefab;

    public void LoadLineSpawn(List<LineSpawnData> lineSpawnDatas)
    {
        foreach (var item in lineSpawnDatas)
        {
            LineSpawn newLineSpwan = Instantiate(prefab, transform);
            GridCell cellLineSpawn = GameManager.Ins.GridManager.GetGridCell(item.CellLineSpawn.x, item.CellLineSpawn.y);
            cellLineSpawn.UnLockCell();
            cellLineSpawn.IsUse = true;
            newLineSpwan.transform.position = cellLineSpawn.transform.position;

            newLineSpwan.name = $"LineSpwan";
            if (newLineSpwan != null)
            {
                newLineSpwan.SetUp(item);
                lineSpawns.Add(newLineSpwan);
            }
        }
    }

    public void ResetData()
    {
        foreach (var item in lineSpawns)
        {
            if (item != null)
            {
                item.DeleteLineSpawn();
            }
        }

        lineSpawns.Clear();
    }

    public LineSpawn GetLineSpawn(int idx)
    {
        return lineSpawns[idx];
    }
}
