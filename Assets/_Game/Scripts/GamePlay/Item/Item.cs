using System;
using System.Collections.Generic;
using ColorFight;
using DG.Tweening;
using TwoCore;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject objLevel1;
    [SerializeField] private GameObject objLevel2;
    [SerializeField] private GameObject vfx;
    [SerializeField] private List<MeshRenderer> meshRenderers;

    private int level = 1;

    private Slot currSlot;
    private ItemData currItemData;
    private bool isShoting;

    [HideInInspector] public bool IsMerge;
    [HideInInspector] public bool isMove;
    public Slot CurrSlot => currSlot;
    public int ID => currItemData.id;
    public int HeroID => currItemData.HeroID;
    public int Level => level;
    public bool IsMove => isMove;

    public bool IsChangeShapeLevel2 => objLevel2.activeSelf;

    public void SetUp(ItemData itemData)
    {
        currItemData = itemData;

        objLevel1.SetActive(true);
        objLevel2.SetActive(false);

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].material = itemData.material;
        }
    }

    public void SetItemWaiting()
    {
        transform.DOLocalMoveZ(-1, 0.1f).SetDelay(0.1f);
    }

    public void MoveToTarget(Action onComplete = null)
    {
        currSlot = GameManager.Ins.GetSlot(this);

        if (currSlot == null)
            return;

        currSlot.IsUsed = true;

        isMove = true;

        transform.DOScale(Vector3.one, 0.3f);
        transform.DOMove(currSlot.transform.position, 0.4f).OnComplete(() =>
        {
            isMove = false;
            currSlot.RunAnim();
            onComplete?.Invoke();
            GameManager.Ins.CheckMerge(this);
        });
    }

    public void UpLevel()
    {
        level = 2;
    }

    public void ChangeShape()
    {
        if (level < 2) return;

        vfx.SetActive(true);
        DOVirtual.DelayedCall(0.1f, () =>
        {
            objLevel1.SetActive(false);
            objLevel2.SetActive(true);
        });

    }

    public void UpdateSlot(Slot slot, float duration = 0.3f, Action onComplete = null)
    {
        if (isShoting == true) return;

        //isMove = true;
        currSlot = slot;
        currSlot.IsUsed = true;
        MoveToCurSlot(duration , () =>
        {
            onComplete?.Invoke();
            //isMove = false;
        });
    }


    public void MoveToCurSlot(float duration = 0.3f, Action onComplete = null)
    {
        //isMove = true;
        if (isShoting == true) return;

        Vector3 startPos = transform.position;
        Vector3 endPos = currSlot.transform.position;

        Vector3 midPos = (startPos + endPos) / 2 + Vector3.up * 1.5f;

        Vector3[] path = new Vector3[] { startPos, midPos, endPos };

        transform.DOPath(path, duration, PathType.CatmullRom)
            .OnComplete(() => {
                onComplete?.Invoke();
                //isMove = false;
                currSlot.RunAnim();
            });
    }

}
