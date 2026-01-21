using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class UIObstacleData
{
    public Sprite Icon;
    public int LevelUnlock;
    public string TextTitle;
    public string Description;
}

public class ProgressObstacles : MonoBehaviour
{
    [SerializeField] private List<UIObstacleData> obstacleDatas;
    [SerializeField] private Image icon;
    [SerializeField] private Image imgFill;
    [SerializeField] private TextMeshProUGUI txtProgress;

    public void SetLevel(int currLevel, bool isWin, Action<bool, UIObstacleData> onComplete = null)
    {
        if (obstacleDatas == null || obstacleDatas.Count == 0)
            return;

        obstacleDatas.Sort((a, b) => a.LevelUnlock.CompareTo(b.LevelUnlock));

        if (currLevel >= obstacleDatas[^1].LevelUnlock)
        {
            gameObject.SetActive(false);
            onComplete?.Invoke(false, null);
            return;
        }

        imgFill.fillAmount = 0;

        UIObstacleData current = obstacleDatas[0];
        int prevLevel = 0;

        for (int i = 0; i < obstacleDatas.Count; i++)
        {
            if (currLevel < obstacleDatas[i].LevelUnlock)
            {
                current = obstacleDatas[i];
                if(i > 0)
                    prevLevel = obstacleDatas[i-1].LevelUnlock;

                break;
            }
        }

        icon.sprite = current.Icon;

        int range = 0;

        int currInSegment = 0;

        if (prevLevel > 1)
        {
            range = current.LevelUnlock - prevLevel;
            currInSegment = currLevel - prevLevel + 1;
        }    
        else
        {
            range = current.LevelUnlock - prevLevel - 1;
            currInSegment = currLevel - prevLevel;
        }    

        if (!isWin)
        {
            currInSegment--;
        }

        float progress = Mathf.Clamp01((float)currInSegment / range);

        imgFill.DOKill();

        DOTween.To(() => imgFill.fillAmount, x =>
        {
            imgFill.fillAmount = x;

            txtProgress.text = $"{currInSegment}/{range}";
        }, progress, 0.5f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
            onComplete?.Invoke(imgFill.fillAmount == 1, current);
        });
    }
}
