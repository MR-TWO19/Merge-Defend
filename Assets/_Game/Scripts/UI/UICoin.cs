using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwoCore;
using UnityEngine;

public class UICoin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCoin;

    private void OnEnable()
    {
        UserSaveData.Ins.Subscribe("coin", UpdateUI);
    }

    private void OnDisable()
    {
        UserSaveData.Ins.Unsubscribe("coin", UpdateUI);
    }

    public void UpdateUI(BaseUserData data)
    {
        txtCoin.text = UserSaveData.Ins.Coin.ToString();
    }    
}
