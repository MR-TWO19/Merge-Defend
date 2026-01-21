using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using TwoCore;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject objLock;
    [SerializeField] private GameObject objPermanentLock;
    [SerializeField] private TextMeshPro txtTimeLock;

    [SerializeField] private List<GameObject> Lines;

    [SerializeField] private bool isLock = true;
    private int curTimeLock;
    private bool occupied;
    public bool Occupied => occupied;
    [HideInInspector] public bool IsUse;

    private void Start()
    {
        objLock.SetActive(isLock);
    }

    public void ResetCell()
    {
        objLock.SetActive(false);
        objPermanentLock.SetActive(false);
        spriteRenderer.color = Color.white;
        curTimeLock = 0;
    }

    public void SetColorCell(Color color)
    {
        spriteRenderer.color = color;
    }

    public void LockCell()
    {
        isLock = true;
        objLock.SetActive(isLock);
    }

    private void CheckUnlockCell()
    {
        curTimeLock--;
        txtTimeLock.text = $"{curTimeLock}";
        if (curTimeLock <= 0)
        {
            UnLockCell();
            //GameBroker.Ins.Unsubscribe(GameEvents.PieceCleared, CheckUnlockCell);
        }
    }

    public void UnLockCell()
    {
        isLock = false;
        objLock.SetActive(false);
        occupied = false;
    }


    public void UpdateUI()
    {
        Vector3[] dirs = new Vector3[]
        {
        Vector3.forward, // 0: lên
        Vector3.back,    // 1: xuống
        Vector3.right,   // 2: phải
        Vector3.left     // 3: trái
        };

        // Ẩn tất cả trước
        foreach (var line in Lines)
            line.SetActive(false);

        for (int i = 0; i < dirs.Length; i++)
        {
            var dir = dirs[i];
            Debug.DrawRay(transform.position, dir * 1, Color.green, 0.5f);

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 1, LayerMask.GetMask("Lock")))
            {
                if (i >= 0 && i < Lines.Count)
                    Lines[i].SetActive(false);
            }
            else
            {
                Lines[i].SetActive(true);
            }
        }
    }
}
