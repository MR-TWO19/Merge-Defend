using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private ObjectLayout objectLayout;
    [SerializeField] private TargetLine targetLinePrefab;
    [SerializeField] private Transform parentLine;

    private List<TargetLine> allLines = new();
    private List<LineLevelData> currLineLevelData;

    public void SetUp(List<LineLevelData> lineLevelData)
    {
        currLineLevelData = lineLevelData;
        ResetAllLines();

        foreach (var lineData in lineLevelData)
        {
            TargetLine line = Instantiate(targetLinePrefab, parentLine != null ? parentLine : transform);
            line.gameObject.SetActive(true);
            line.Init(lineData.Targets);
            allLines.Add(line);
        }

        objectLayout.UpdateLayout();
    }

    public void ResetAllLines()
    {
        allLines.ForEach(_ =>
        {
            if (_ != null)
            {
                _.gameObject.SetActive(false);
                Destroy(_.gameObject, 1f);
            }
        });

        allLines.Clear();
    }

    public Balloon GetTarget(int itemId)
    {
        foreach (var line in allLines)
        {
            var target = line.GetTargetById(itemId);
            if (target != null)
                return target;
        }
        return null;
    }

    public Balloon GetFirstTarget(int itemId)
    {
        foreach (var line in allLines)
        {
            var target = line.GetFirstTargetById(itemId);
            if (target != null)
                return target;
        }
        return null;
    }

    public bool IsAllCompete()
    {
        if (allLines == null || allLines.Count == 0)
            return true;

        return allLines.All(line => line != null && line.IsAllTargetCompleted());
    }

}
