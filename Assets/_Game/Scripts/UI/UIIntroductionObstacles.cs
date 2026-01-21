using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hawky.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIntroductionObstacles : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI txtTitle;
    [SerializeField] TextMeshProUGUI txtDescription;
    [SerializeField] Button btnNextLevel;

    private PopupController popupController;

    private void Awake()
    {
        btnNextLevel.onClick.AddListener(OnClickNextLevel);
    }

    public void Show(UIObstacleData uIObstacleData, PopupController popupController)
    {
        image.transform.DOScale(Vector3.zero, 0);
        gameObject.SetActive(true);

        image.sprite = uIObstacleData.Icon;
        txtTitle.text = uIObstacleData.TextTitle;
        txtDescription.text = uIObstacleData.Description;
        this.popupController = popupController;

        image.transform.DOScale(Vector3.one, 0.5f);
    }

    public void OnClickNextLevel()
    {
        GameManager.Ins.NextLevel();
        popupController.Hide();
        gameObject.SetActive(false);
    }
}
