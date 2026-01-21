using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TargetLine : MonoBehaviour
{
    [SerializeField] private Balloon targetPrefab;
    [SerializeField] private List<GameObject> posTargets;
    [SerializeField] private int maxTarget = 4;

    private List<Balloon> targets = new();
    private int indexCurrentTarget = 0;
    private List<TargetLevelData> currTargets;

    public void Init(List<TargetLevelData> targetsData)
    {
        ResetBottleTargets();
        currTargets = new(targetsData);

        int totalTargetInit = Mathf.Min(currTargets.Count, maxTarget);

        for (int i = 0; i < totalTargetInit; i++)
        {
            TargetLevelData targetData = currTargets[i];
            indexCurrentTarget++;
            CreateBalloonTarget(targetData, i);
        }
    }

    private void ResetBottleTargets()
    {
        indexCurrentTarget = 0;
        foreach (var t in targets)
        {
            if (t != null) Destroy(t.gameObject);
        }
        targets.Clear();
    }

    private void CreateBalloonTarget(TargetLevelData targetData, int idx)
    {
        Balloon balloon = GetTarget();
        balloon.SetUp(targetData, this);
        balloon.IsUsed = true;
        balloon.Idx = idx;
        balloon.transform.position = posTargets[idx].transform.position;
        balloon.gameObject.SetActive(true);
        balloon.transform.localScale = Vector3.one;

        if (!targets.Contains(balloon))
            targets.Add(balloon);
    }

    private Balloon GetTarget()
    {
        Balloon balloon = targets.FirstOrDefault(t => !t.IsUsed || t.IsExploded);

        if (balloon == null)
        {
            balloon = Instantiate(targetPrefab, transform);
            targets.Add(balloon);
        }
        else
        {
            balloon.gameObject.SetActive(true);
            balloon.ResetAll();
        }

        return balloon;
    }
    public Balloon GetTargetById(int itemId)
    {
        return targets.Find(t => t.CurrTargetData.ItemId == itemId && !t.IsExploded && !t.IsExplodeWaiting && t.Idx == 0 && t.IsUsed);
    }

    public Balloon GetFirstTargetById(int itemId)
    {
        if (targets == null || targets.Count == 0)
            return null;

        var first = targets.Find(_ => _.Idx == 0);
        if (first == null)
            return null;

        return first.CurrTargetData.ItemId == itemId ? targets[0] : null;
    }

    public void RunCompleteTarget(Balloon targetBalloon)
    {
        if (targetBalloon == null) return;

        Vector3 startPos = targetBalloon.transform.position;
        Vector3 movePos = startPos + Vector3.up * 7f;

        targetBalloon.gameObject.SetActive(false);
        targetBalloon.IsUsed = false;
        targetBalloon.IsExploded = true;
        targetBalloon.Idx = -1;

        var usedTargets = targets.Where(t => t.IsUsed && !t.IsExploded).OrderBy(t => t.Idx).ToList();
        for (int i = 0; i < usedTargets.Count; i++)
        {
            int newIdx = i;
            usedTargets[i].Idx = newIdx;

            if (newIdx < posTargets.Count)
            {
                usedTargets[i].transform.DOMove(posTargets[newIdx].transform.position, 0.4f)
                    .SetEase(Ease.OutQuad);
            }
        }

        if (indexCurrentTarget < currTargets.Count)
        {
            TargetLevelData nextData = currTargets[indexCurrentTarget];
            indexCurrentTarget++;

            int newIdx = usedTargets.Count;
            if (newIdx < maxTarget)
            {
                Balloon newBalloon = GetTarget();
                newBalloon.SetUp(nextData, this);
                newBalloon.IsUsed = true;
                newBalloon.IsExploded = false;
                newBalloon.Idx = newIdx;
                newBalloon.transform.position = posTargets[newIdx].transform.position;
                newBalloon.transform.localScale = Vector3.zero;
                newBalloon.gameObject.SetActive(true);
                newBalloon.transform.DOScale(Vector3.one, 0.25f)
                    .SetEase(Ease.OutBack)
                    .SetDelay(0.1f);

                if (!targets.Contains(newBalloon))
                    targets.Add(newBalloon);
            }
        }

        GameManager.Instance.CheckWinGame();
    }

    public bool IsAllTargetCompleted()
    {
        return !targets.Any(t => t.IsUsed && !t.IsExploded);
    }
}
