using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTab : MonoBehaviour
{
    [SerializeField] private UIButton btnLevel;


    private void Awake()
    {
        btnLevel.OnClick.OnTrigger.Event.AddListener(OnLevelButtonClicked);
    }

    private void OnLevelButtonClicked()
    {
        GameManager.Ins.StartGame();
    }
}
