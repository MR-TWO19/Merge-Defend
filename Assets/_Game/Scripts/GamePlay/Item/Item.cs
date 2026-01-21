using System;
using System.Collections;
using System.Collections.Generic;
using ColorFight;
using DG.Tweening;
using Hawky.Sound;
using TwoCore;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
    public int Level => level;
    public bool IsMove => isMove;

    public bool IsChangeShapeLevel2 => objLevel2.activeSelf;

    public void SetUp(ItemData itemData)
    {
        currItemData = itemData;

        objLevel1.SetActive(true);
        objLevel2.SetActive(false);

        meshRenderers.ForEach(_ => _.material = currItemData.MaterialItem);
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

    public void ShootTarget(Balloon target, Action onComplete = null)
    {
        if (target == null) return;

        //isMove = true;

        isShoting = true;

        currSlot.IsUsed = false;

        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);

        Vector3 euler = transform.rotation.eulerAngles;
        euler.y = lookRot.eulerAngles.y;

        float landPauseDuration = 0.05f;

        Vector3 backPos = transform.position - dir * 0.5f;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotateQuaternion(Quaternion.Euler(euler), landPauseDuration)
            .SetEase(Ease.OutQuad));

        seq.Append(transform.DOMove(backPos, 0.15f)
            .SetEase(Ease.OutQuad));

        seq.AppendInterval(0.05f);

        seq.Append(transform.DOMove(target.transform.position, 0.3f)
            .SetEase(Ease.InQuad).OnStart(() =>
            {
                SoundManager.Instance.PlaySound(SoundId.SWOOSH);
            }));

        seq.OnComplete(() =>
        {
            target.Explode(() =>
            {
                onComplete?.Invoke();
            });
            gameObject.SetActive(false);
            GameBroker.Ins.Emit(GameEvents.CheckLock);
            isShoting = false;
            //isMove = false;
        });
    }

}
