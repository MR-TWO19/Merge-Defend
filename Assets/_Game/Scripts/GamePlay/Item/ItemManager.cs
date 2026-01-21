using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<ItemControl> itemControls;
    [SerializeField] private ItemControl itemControlPrefab;

    public void LoadItem(List<ItemControlLevelData> itemControlLevelDatas)
    {
        foreach (var item in itemControls)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        itemControls.Clear();

        if (itemControlLevelDatas == null || itemControlLevelDatas.Count == 0)
            return;

        foreach (var itemData in itemControlLevelDatas)
        {
            GridCell gridCell = GameManager.Instance.GridManager.GetGridCell(itemData.Cell.x, itemData.Cell.y);
            gridCell.IsUse = true;

            gridCell.UnLockCell();

            if (itemData.ItemIds.Count <= 0)
                continue;

            ItemControl newItem = Instantiate(itemControlPrefab, transform);
            newItem.transform.position = gridCell.gameObject.transform.position;

            int count = itemControls.Count(i => i.name.Contains("ItemControl"));
            newItem.name = $"ItemControl_{count}";

            newItem.SetUp(itemData, gridCell);

            itemControls.Add(newItem);
        }

        DOVirtual.DelayedCall(0.5f, CheckOpenAllLib);
    }

    public ItemControl GetItemControl(int idx)
    {
        if (itemControls == null || itemControls.Count == 0)
        {
            return null;
        }

        if (idx < 0 || idx >= itemControls.Count)
        {
            return null;
        }

        return itemControls[idx];
    }

    public void CheckOpenAllLib()
    {
        foreach (var itemControl in itemControls)
        {
            if (itemControl != null)
            {
                itemControl.CheckOpenLib();
            }
        }
    }

    public void ResetData()
    {
        foreach (var item in itemControls)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        itemControls.Clear();
    }

    public void AddBottle(ItemControl itemControl)
    {
        itemControl.gameObject.transform.SetParent(transform);
        itemControls.Add(itemControl);
    }

    public ItemControl GetItemByLockID(int lockID)
    {
       return itemControls.Find(_ => _.CurrItemControlLevelData.LockId == lockID && _.CurrItemControlLevelData.EventType == ItemEventType.Lock);
    }
}
