using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCharater : MonoBehaviour
{
    [SerializeField] private HPBar HPBar;
    [SerializeField] private MeshRenderer meshRenderer;

    public GameObject PosSpawnCharater;

    public void SetData(int totalHp)
    {
        if (HPBar != null)
        {
            HPBar.SetData(totalHp);
        }
    }

    public bool TakeDamage(int damage)
    {
        HPBar.AddHP(-damage);
        meshRenderer.material.color = Color.black;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            meshRenderer.material.color = Color.white;
        });

        return HPBar.IsOutOfHP;
    }
}
