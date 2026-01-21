using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using TwoCore;
using UnityEngine;

public class LineSpawn : MonoBehaviour
{
    [SerializeField] private GameObject frameText;
    [SerializeField] private TextMeshPro txtTotleItemControl;
    [SerializeField] private ItemControl itemControlPrefab;
    [SerializeField] private Collider col;

    private LineSpawnData currLineSpawnData;
    private List<ItemControlLevelData> itemControlLevelData;
    private GridCell cellSpawn;

    private bool isSpawning;

    private void OnDestroy()
    {
        GameBroker.Ins.Unsubscribe(GameEvents.CheckLineSpawn, CheckSpawn);
    }

    public void SetUp(LineSpawnData lineSpawnData)
    {
        currLineSpawnData = lineSpawnData;
        itemControlLevelData = new List<ItemControlLevelData>(currLineSpawnData.ItemControlLevelDatas);
        txtTotleItemControl.text = $"{itemControlLevelData.Count}";

        cellSpawn = GameManager.Instance.GridManager.GetGridCell(currLineSpawnData.CellSpawn.x, currLineSpawnData.CellSpawn.y);
        cellSpawn.UnLockCell();
        transform.LookAt(cellSpawn.transform.position);
        txtTotleItemControl.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                 Camera.main.transform.rotation * Vector3.up);

        GameBroker.Ins.Subscribe(GameEvents.CheckLineSpawn, CheckSpawn);

        CheckSpawn();
    }

    public void CheckSpawn()
    {
        if (isSpawning || itemControlLevelData == null || itemControlLevelData.Count == 0)
            return;

        if (cellSpawn.IsUse) return;

        SpawnItem();
    }

    private void SpawnItem()
    {
        isSpawning = true;
        cellSpawn.IsUse = true;
        ItemControlLevelData data = itemControlLevelData[0];

        ItemControl newItem = Instantiate(itemControlPrefab);
        newItem.transform.position = transform.position;
        newItem.SetUp(data, cellSpawn);
        newItem.transform.localScale = Vector3.zero;
        

        GameManager.Instance.ItemManager.AddBottle(newItem);

        newItem.transform.DOMove(cellSpawn.transform.position, 0.2f);
        newItem.transform.DOScale(Vector3.one, 0.2f).OnComplete(() => {
            isSpawning = false;
            DOVirtual.DelayedCall(0.2f, () =>
            {
                newItem.CheckOpenLib();
            });
        });

        itemControlLevelData.Remove(data);
        txtTotleItemControl.text = $"{itemControlLevelData.Count}";

        if (itemControlLevelData.Count <= 0)
        {
            col.gameObject.SetActive(false);
            GameBroker.Ins.Unsubscribe(GameEvents.CheckLineSpawn, CheckSpawn);
        }
    }

    public void DeleteLineSpawn()
    {
        transform.DOKill(true);

        StopAllCoroutines();

        Destroy(gameObject);
    }
}
