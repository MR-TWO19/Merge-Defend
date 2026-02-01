using System.Collections;
using System.Collections.Generic;
using ColorFight;
using DG.Tweening;
using TwoCore;
using UnityEngine;


public class ItemControl : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    [SerializeField] private GameObject objFrame;
    [SerializeField] private Transform tranItem;
    [SerializeField] private List<Transform> pos;
    [SerializeField] private Renderer renLid;
    [SerializeField] private Collider col;


    private bool isClick;
    private GridCell currGridCell;

    [Header("Raycast Check")]
    [SerializeField] private float rayDistance = 1f;

    private ItemControlLevelData currItemControlLevelData;

    public ItemControlLevelData CurrItemControlLevelData => currItemControlLevelData;

    public void SetUp(ItemControlLevelData itemControlLevelData, GridCell gridCell)
    {
        currItemControlLevelData = itemControlLevelData;
        currGridCell = gridCell;

        foreach (var oldItem in items)
        {
            if (oldItem != null)
                Destroy(oldItem.gameObject);
        }
        items.Clear();

        for (int i = 0; i < 4; i++)
        {
            ItemData itemData = GameConfig.Ins.GetRondomItemData();

            if (itemData == null || itemData.Prefab == null)
            {
                continue;
            }

            GameObject itemObj = Instantiate(itemData.Prefab, tranItem);
            itemObj.transform.position = pos[i].position;
            itemObj.transform.localRotation = Quaternion.identity;
            itemObj.transform.localScale = Vector3.one;

            if (itemObj.TryGetComponent<Item>(out var item))
            {
                item.SetUp(itemData);
                items.Add(item);
            }
        }

        if (objFrame != null)
            objFrame.SetActive(true);

        if (col != null)
            col.enabled = true;

        renLid.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        SoundManager.Ins.PlayOneShot(SoundId.SWOOSH);

        if (isClick == true)
            return;

        isClick = true;

        if (HapticManager.Ins != null)
            HapticManager.Ins.PlaySuccess();

        objFrame.transform.DOScale(Vector3.zero, 0.5f);

        if (GameManager.Ins.IsLock || GameManager.Ins.ItemControlsWaiting.Count > 0)
        {
            foreach (var item in items)
            {
                item.SetItemWaiting();
            }
            GameManager.Ins.AddItemControlWaiting(this);
        }
        else
        {
            MoveItemsToTarget();
        }
    }

    public void MoveItemsToTarget()
    {

        if (!GameManager.Ins.IsLock)
        {
            col.enabled = false;
            currGridCell.IsUse = false;

            GameManager.Ins.ItemManager.CheckOpenAllLib();

            GameManager.Ins.AddItemControlWaitingRemove(this);

            for (int i = 0; i < items.Count; i++)
            {
                bool isLast = (i == items.Count - 1);

                items[i].MoveToTarget(() =>
                {
                    if (isLast)
                    {
                        if (HapticManager.Ins != null)
                            HapticManager.Ins.PlaySuccess();
                        SoundManager.Ins.PlayOneShot(SoundId.POP);
                        
                        DOVirtual.DelayedCall(5f, () =>
                        {
                            GameManager.Ins.ItemManager.CreateItem(currGridCell, CurrItemControlLevelData);
                        });

                    }
                });
            }

            GameBroker.Ins.Emit(GameEvents.CheckHint);
            GameBroker.Ins.Emit(GameEvents.CheckLineSpawn);
        }
    }

    public void CheckOpenLib()
    {
        var mat = renLid.material;
        mat.DOFade(0, 0.5f).SetDelay(0.2f);
        renLid.transform.DOLocalMoveZ(renLid.transform.localPosition.z - 2f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            renLid.gameObject.SetActive(false);
        });
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Vector3[] dirs = new Vector3[]
    //    {
    //        Vector3.forward,
    //        Vector3.back,
    //        Vector3.right,
    //        Vector3.left
    //    };

    //    foreach (var dir in dirs)
    //    {
    //        Gizmos.DrawLine(transform.position, transform.position + dir * rayDistance);
    //    }
    //}
}
