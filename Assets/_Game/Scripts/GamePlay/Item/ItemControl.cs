using System.Collections;
using System.Collections.Generic;
using ColorFight;
using DG.Tweening;
using Hawky.Sound;
using TwoCore;
using UnityEngine;
using UnityEngine.UI;
using global::System;

public class ItemControl : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    [SerializeField] private GameObject objFrame;
    [SerializeField] private Transform tranItem;
    [SerializeField] private List<Transform> pos;
    [SerializeField] private Renderer renLid;
    [SerializeField] private Collider col;
    [SerializeField] private LockItem lockItem;
    //[SerializeField] private SpriteRenderer spHint;
    [SerializeField] private Renderer renHint;
    [SerializeField] private MeshRenderer meshRenkey;


    private bool isClick;
    private bool isLock;
    private GridCell currGridCell;

    [Header("Raycast Check")]
    [SerializeField] private float rayDistance = 1f;

    private ItemControlLevelData currItemControlLevelData;

    public ItemControlLevelData CurrItemControlLevelData => currItemControlLevelData;

    private void OnDisable()
    {
        GameBroker.Ins.Unsubscribe(GameEvents.CheckHint, CheckHint);
    }

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

        if (currItemControlLevelData.ItemIds == null || currItemControlLevelData.ItemIds.Count == 0)
            return;

        int count = Mathf.Min(currItemControlLevelData.ItemIds.Count, pos.Count);

        for (int i = 0; i < count; i++)
        {
            int itemId = currItemControlLevelData.ItemIds[i];
            ItemData itemData = GameConfig.Ins.GetItemData(itemId);

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

        //spHint.gameObject.SetActive(false);
        renHint.gameObject.SetActive(false);
        renLid.gameObject.SetActive(true);
        lockItem.Hide();

        meshRenkey.gameObject.SetActive(currItemControlLevelData.KeepTheKey);

        if (currItemControlLevelData.EventType == ItemEventType.Hint)
        {
            isLock = true;
            renHint.gameObject.SetActive(true);
            renLid.gameObject.SetActive(false);
            GameBroker.Ins.Subscribe(GameEvents.CheckHint, CheckHint);
        }
        else if (currItemControlLevelData.EventType == ItemEventType.Lock || currItemControlLevelData.KeepTheKey)
        {

            LockData lockData = GameConfig.Ins.GetLockData(currItemControlLevelData.LockId);

            if (lockData == null)
                return;

            meshRenkey.material = lockData.materialKey;

            if (!currItemControlLevelData.KeepTheKey)
            {
                lockItem.SetUp(lockData.materialLock);
                LockItem();
            }
        }


    }

    public void CheckHint()
    {
        if (IsObstacleAhead())
        {
            //if (TutorialManager.Instance.CurrTutorial == Tutorial.Hint)
            //{
            //    TutorialManager.Instance.NextStep();
            //}
            isLock = false;
            HideHint();
            GameBroker.Ins.Unsubscribe(GameEvents.CheckHint, CheckHint);
        }
    }

    private void HideHint()
    {

        //spHint.material.SetFloat("_DisolveAmount", 0f);

        //spHint.material
        //    .DOFloat(1f, "_DisolveAmount", 0.5f)
        //    .SetEase(Ease.OutQuad);

        //var mat = renHint.material;
        //mat.DOFade(0, 0.5f);
        Vector3 targetPos = renHint.transform.localPosition
                            + new Vector3(0f, 1f, -1f);

        renHint.transform
            .DOLocalMove(targetPos, 0.2f)
            .OnComplete(() =>
            {
                renHint.gameObject.SetActive(false);
            });
    }

    public void UnLock()
    {
        if (currItemControlLevelData.EventType == ItemEventType.Lock)
        {
            isLock = false;
            lockItem.UnLock(CheckOpenLib);
        }
    }

    private void LockItem()
    {
        isLock = true;
        lockItem.Show();
    }

    public void OnClick()
    {
        if (TutorialManager.Ins.CurrTutorial == Tutorial.GamePlayIntroduction)
        {
            if (TutorialManager.Ins.ItemControlRequiresClick == null
               || TutorialManager.Ins.ItemControlRequiresClick != this)
            {
                return;
            }
            else
            {
                TutorialManager.Ins.NextStep(this);
            }
        }

        SoundManager.Instance.PlaySound(SoundId.SWOOSH);

        if (!IsObstacleAhead() || isClick == true || isLock)
            return;

        isClick = true;

        if (HapticManager.Ins != null)
            HapticManager.Ins.PlaySuccess();

        objFrame.transform.DOScale(Vector3.zero, 0.5f);

        if (currItemControlLevelData.KeepTheKey)
        {
            ItemControl itemControlLock = GameManager.Ins.ItemManager.GetItemByLockID(currItemControlLevelData.LockId);
            Transform target = itemControlLock.transform;

            Sequence seq = DOTween.Sequence();

            SoundManager.Instance.PlaySound(SoundId.KEY);

            seq.Append(meshRenkey.transform.DOLocalMoveZ(-3f, 0.2f));

            seq.Append(
                meshRenkey.transform.DOMove(target.position, 0.4f)
                    .SetEase(Ease.InOutQuad)
            );

            seq.OnComplete(() =>
            {
                meshRenkey.gameObject.SetActive(false);
                itemControlLock.UnLock();
            });
        }

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
                        SoundManager.Instance.PlaySound(SoundId.POP);
                    }
                });
            }

            GameBroker.Ins.Emit(GameEvents.CheckHint);
            GameBroker.Ins.Emit(GameEvents.CheckLineSpawn);


            if(GameManager.Ins.IsLoseGame())
            {
                GameManager.Ins.LoseGame();
            }
        }
    }

    IEnumerator RunMoveItemsToTarget()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var item in items)
        {
            item.MoveToTarget();
        }
    }

    public void CheckOpenLib()
    {
        if (!IsObstacleAhead() || (isLock && currItemControlLevelData.EventType == ItemEventType.Lock))
            return;

        var mat = renLid.material;
        mat.DOFade(0, 0.5f).SetDelay(0.2f);
        renLid.transform.DOLocalMoveZ(renLid.transform.localPosition.z - 2f, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            renLid.gameObject.SetActive(false);
        });
    }

    public bool IsObstacleAhead()
    {
        Vector3[] dirs = new Vector3[]
        {
        Vector3.forward, // lên
        Vector3.back,    // xuống
        Vector3.right,   // phải
        Vector3.left     // trái
        };

        bool hasFreeDirection = false;

        //var cell = currItemControlLevelData.Cell;

        //int maxX = 5;

        for (int i = 0; i < dirs.Length; i++)
        {
            var dir = dirs[i];

            //if (dir == Vector3.left && cell.x <= 0) continue;
            //if (dir == Vector3.right && cell.x >= maxX) continue;
            //if (dir == Vector3.back && cell.y <= 0) continue;

            Debug.DrawRay(transform.position, dir * rayDistance, Color.green, 0.5f);

            if (!Physics.Raycast(transform.position, dir, out RaycastHit hit, rayDistance))
            {
                hasFreeDirection = true;
            }
        }

        return hasFreeDirection;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3[] dirs = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };

        foreach (var dir in dirs)
        {
            Gizmos.DrawLine(transform.position, transform.position + dir * rayDistance);
        }
    }
}
