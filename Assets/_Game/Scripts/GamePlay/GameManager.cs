using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ColorFight;
using UnityEngine;
using TwoCore;

public class GameManager : SingletonMono<GameManager>
{
    public static GameManager Instance => Ins;

    public SlotManager SlotManager;
    public InputHandler InputHandler;
    public TargetManager TargetManager;
    public ItemManager ItemManager;
    public GridManager GridManager;
    public LineSpawnManager LineSpawnManager;
    public UIInGame UIInGame;

    private List<ItemControl> itemControlsWatingRemove = new();
    private List<ItemControl> itemControlsWaiting = new();
    private List<Item> itemsOnSlot = new();
    private List<Item> itemsClear = new();

    private int countMeger;
    public bool isEndGame;
    public bool IsAnimRuning;
    public List<ItemControl> ItemControlsWaiting => itemControlsWaiting;

    public bool IsFullSlot()
    {
        return itemsOnSlot.Count >= 10;
    }

    public bool IsLock => IsAnimRuning || IsFullSlot() || isEndGame;

    public Slot GetSlot(Item item)
    {
        Slot slot;
        Item bottleDuplicate = FindDuplicateItem(item.ID, item.Level);
        if (bottleDuplicate == null)
        {
            slot = SlotManager.GetFreeSlot();
        }
        else
        {
            SortItems();

            Item itemBehind = GetAndCheckItem(bottleDuplicate);
            if (itemBehind == null)
            {
                slot = SlotManager.GetFreeSlot();
            }
            else
            {
                slot = SlotManager.GetSlot(itemBehind.CurrSlot.Index + 1);
                UpdateSlotItemBehind(itemBehind);
            }
        }

        itemsOnSlot.Add(item);

        var itemMegers = itemsOnSlot.Where(b => b.ID == item.ID && b.Level == item.Level && !b.IsChangeShapeLevel2).ToList();

        if (itemMegers.Count >= 3)
        {
            itemMegers = itemMegers.Take(3).ToList();
            itemMegers.ForEach(_ => {
                _.UpLevel();
                //itemsClear.Add(_);
            });
        }
        return slot;
    }

    private void UpdateSlotItemBehind(Item item)
    {
        float duration = 0.4f;
        int idxBottle = itemsOnSlot.IndexOf(item);

        bool isPlaySound = false;

        for (int i = idxBottle + 1; i < itemsOnSlot.Count; i++)
        {
            if (itemsClear.Contains(itemsOnSlot[i]))
                continue;

            isPlaySound = true;

            if (i != itemsOnSlot.Count - 1)
            {
                Item newBottle = itemsOnSlot[i + 1];
                if (itemsClear.Contains(newBottle))
                {
                    itemsOnSlot[i].UpdateSlot(SlotManager.GetSlot(newBottle.CurrSlot.Index + 1), duration);
                }
                else
                {
                    itemsOnSlot[i].UpdateSlot(newBottle.CurrSlot, duration);
                }
            }
            else
            {
                itemsOnSlot[i].UpdateSlot(SlotManager.GetSlot(itemsOnSlot[i].CurrSlot.Index + 1), duration);
            }

        }

        if (isPlaySound)
        {
            //HapticManager.PlayHaptic(HapticPatterns.PresetType.Success);
            //SoundManager.Instance.PlaySound(SoundId.POP);
        }
    }

    public void AddItemControlWaiting(ItemControl itemControl)
    {
        itemControlsWaiting.Add(itemControl);
    }

    public void RemoveItemControlWaiting(ItemControl itemControl)
    {
        itemControlsWaiting.Remove(itemControl);
    }

    public void AddItemControlWaitingRemove(ItemControl itemControl)
    {
        itemControlsWatingRemove.Add(itemControl);
    }

    public void SortItems()
    {
        itemsOnSlot.Sort((a, b) => a.CurrSlot.Index.CompareTo(b.CurrSlot.Index));
    }

    private Item GetAndCheckItem(Item item)
    {
        int index = itemsOnSlot.IndexOf(item);

        if (index == -1 || index >= itemsOnSlot.Count - 1)
        {
            if (itemsClear.Contains(item))
            {
                return null;
            }
            else
            {
                return item;
            }
        }

        Item nextItem = itemsOnSlot[index + 1];

        if (itemsClear.Contains(nextItem))
        {
            return GetAndCheckItem(nextItem);
        }
        else
        {
            return item;
        }
    }

    private Item FindDuplicateItem(int id, int Level)
    {
        for (int i = itemsOnSlot.Count - 1; i >= 0; i--)
        {
            var item = itemsOnSlot[i];
            if (item.ID == id && item.Level == Level)
            {
                return item;
            }
        }
        return null;
    }

    private void UpdateSlotItem()
    {
        if (countMeger > 0) return;
        SortItems();

        IsAnimRuning = true;

        int moveCount = 0;
        int totalToMove = 0;
        bool canMoveAgain = false;

        foreach (var item in itemsOnSlot)
        {
            if (itemsClear.Contains(item))
                continue;

            Slot slot = SlotManager.GetPreviousFreeSlot(item.CurrSlot);
            if (slot == null)
                continue;

            totalToMove++;

            item.CurrSlot.IsUsed = false;

            Slot nextSlot = SlotManager.GetPreviousFreeSlot(slot);
            if (nextSlot != null)
            {
                canMoveAgain = true;
            }

            item.UpdateSlot(slot, 0.15f, () =>
            {
                moveCount++;

                if (moveCount >= totalToMove)
                {
                    if (canMoveAgain)
                    {
                        UpdateSlotItem();
                    }
                    else
                    {
                        SortItems();
                        IsAnimRuning = false;

                        CheckAndShoot();
                        HandleItemControlWaiting();
                    }
                }
            });
        }

        if (totalToMove <= 0)
        {
            IsAnimRuning = false;
            HandleItemControlWaiting();
        }
        else
        {
            //HapticManager.PlayHaptic(HapticPatterns.PresetType.Success);
            //SoundManager.Instance.PlaySound(SoundId.POP);
        }
    }

    public void CheckAndShoot()
    {
        if (itemsClear.Count > 0 || IsAnimRuning) return;
        IsAnimRuning = true;

        int totalShoot = 0;

        List<Item> items = itemsOnSlot.FindAll(_ => _.IsChangeShapeLevel2 && !_.IsMove);

        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                Balloon target = TargetManager.GetTarget(item.ID);
                if (target)
                {
                    totalShoot++;
                    target.IsExplodeWaiting = true;
                    itemsClear.Add(item);
                    item.ShootTarget(target, () =>
                    {
                        itemsClear.Remove(item);
                        itemsOnSlot.Remove(item);
                        if (itemsClear.Count <= 0)
                        {
                            IsAnimRuning = false;
                            UpdateSlotItem();
                        }
                    });
                }
            }
        }

        if (totalShoot <= 0)
        {
            IsAnimRuning = false;

            if (IsLoseGame())
            {
                LoseGame();
            }
        }
    }

    private void HandleItemControlWaiting()
    {
        if (IsWaitingToShoot()
            || itemControlsWaiting.Count <= 0
            || isEndGame)
        {
            CheckAndShoot();
            return;
        }

        if (IsLoseGame())
        {
            LoseGame();
            return;
        }

        if (itemControlsWaiting.Count <= 0 || isEndGame)
        {
            CheckAndShoot();
            return;
        }
        ;

        foreach (var item in itemControlsWaiting)
        {
            item.MoveItemsToTarget();
        }

        foreach (var item in itemControlsWatingRemove)
        {
            itemControlsWaiting.Remove(item);
        }
        itemControlsWatingRemove.Clear();
    }

    public void CheckMerge(Item item)
    {

        var itemMegers = itemsOnSlot
            .Where(i => i.ID == item.ID && i.Level == item.Level && !i.IsMove && !i.IsChangeShapeLevel2 && !i.IsMerge)
            .OrderBy(i => i.CurrSlot.Index)
            .ToList();


        if (itemMegers.Count < 3)
        {
            return;
        }

        itemMegers = itemMegers.Take(3).ToList();

        foreach (var b in itemMegers)
        {
            b.IsMerge = true;
        }

        IsAnimRuning = true;

        countMeger++;
        Item minItem = itemMegers.OrderBy(i => i.CurrSlot.Index).First();
        foreach (var i in itemMegers)
        {
            if (i != minItem)
            {
                //itemsOnSlot.Remove(i);

            }
            //else
            //    itemsClear.Add(b);
        }

        DOVirtual.DelayedCall(0.1f, () =>
        {
            Vector3 center = Vector3.zero;
            foreach (var i in itemMegers)
            {
                if (i != minItem)
                {
                    itemsOnSlot.Remove(i);
                    i.CurrSlot.IsUsed = false;
                    center += i.transform.position;
                }
            }
            center /= itemMegers.Count;

            for (int i = 0; i < itemMegers.Count; i++)
            {
                var b = itemMegers[i];
                int index = i;

                Vector3 upPos = b.transform.position + Vector3.up * 1f;
                Sequence seq = DOTween.Sequence();
                seq.Append(b.transform.DOMove(upPos, 0.1f).SetEase(Ease.OutQuad));
                //SoundManager.Instance.PlaySound(SoundId.MERGE);
                //HapticManager.PlayHaptic(HapticPatterns.PresetType.Success);
                seq.Append(b.transform.DOMoveX(center.x, 0.2f).SetEase(Ease.InQuad));
                seq.OnComplete(() =>
                {

                    if (index == itemMegers.Count - 1)
                    {

                        foreach (var item in itemMegers)
                        {
                            if (item == minItem)
                            {
                                item.ChangeShape();
                                item.MoveToCurSlot(0.5f, () =>
                                {
                                    countMeger--;
                                    if (countMeger <= 0)
                                    {
                                        UpdateSlotItem();
                                    }
                                    //itemMegers.ForEach(_ => itemsClear.Remove(_));
                                });
                            }
                            else
                            {
                                item.gameObject.SetActive(false);
                                //BottleManager.RemoveBottle(item);
                            }
                        }
                    }
                });
            }
        });
    }

    public bool IsLoseGame()
    {
        if (isEndGame)
            return true;

        bool hasTriple = itemsOnSlot
             .Where(i => !i.IsChangeShapeLevel2)
             .GroupBy(b => new { b.ID, b.Level })
             .Any(g => g.Count() >= 3);

        int totalFreeSlot = 10 + itemsClear.Count;

        if (!hasTriple && itemsOnSlot.Count >= totalFreeSlot && !IsWaitingToShoot() && !IsAnimRuning)
        {
            return true;
        }

        return false;
    }

    private bool IsWaitingToShoot()
    {
        bool isItemWaiting = false;

        var listLevel2 = itemsOnSlot.Where(i => i.IsChangeShapeLevel2).ToList();
        foreach (var item in listLevel2)
        {
            Balloon target = TargetManager.GetFirstTarget(item.ID);
            if (target != null)
                isItemWaiting = true;
        }

        return isItemWaiting;
    }

    public void CheckWinGame()
    {
        if (TargetManager.IsAllCompete())
        {
            WinGame();
        }
        else
        {
            CheckAndShoot();
        }
    }

    public void LoadLevel(int level)
    {
        ReletAll();
        UIInGame.UpdateTextLevel();

        int idxLevel = level - 1;
        if (idxLevel > LevelConfig.Ins.levelDatas.Count - 1)
            idxLevel = UnityEngine.Random.Range(0, LevelConfig.Ins.levelDatas.Count - 1);
        LevelData levelData = LevelConfig.Ins.levelDatas[idxLevel];

        GridManager.GenerateGrid(levelData.SizeGrid);
        ItemManager.LoadItem(levelData.ItemControlLevelDatas);
        LineSpawnManager.LoadLineSpawn(levelData.LineSpawnDatas);
        TargetManager.SetUp(levelData.Lines);

        UIInGame.UpdateTextLevel();
    }

    public void ReletAll()
    {
        itemControlsWatingRemove.Clear();
        itemControlsWaiting.Clear();
        itemsOnSlot.Clear();
        itemsClear.Clear();
        countMeger = 0;
        isEndGame = false;
        IsAnimRuning = false;
        InputHandler.UnlockInput();
        ItemManager.ResetData();
        SlotManager.ResetAllSlot();
        GridManager.ClearGrid();
        LineSpawnManager.ResetData();
    }

    public void StartGame()
    {
        LoadLevel(UserSaveData.Ins.Level);
    }

    public void NextLevel()
    {
        UserSaveData.Ins.NextLevel();
        LoadLevel(UserSaveData.Ins.Level);
    }

    public void GoLevel(int level)
    {
        UserSaveData.Ins.Level = level;
        UserSaveData.Ins.Save();
        LoadLevel(level);
    }

    public void RestartLevel()
    {
        LoadLevel(UserSaveData.Ins.Level);
    }

    public void WinGame()
    {
        if (isEndGame == false)
        {
            isEndGame = true;
            InputHandler.LockInput();

            //CoroutineManager.Ins.Start(OpenPausePopup());

            //IEnumerator OpenPausePopup()
            //{
            //    RDM.Ins.WinPopupModel = new WinPopupModel();
            //    SceneManager.Instance.OpenPopup(SceneId.WIN_POPUP);
            //    yield return new WaitUntil(() => RDM.Ins.WinPopupModel.ReturnData.Returned != ReturnId.WAIT);
            //    SceneManager.Instance.ClosePopup(SceneId.WIN_POPUP);
            //}
        }
    }

    public void LoseGame()
    {
        if (isEndGame == false)
        {
            isEndGame = true;
            InputHandler.LockInput();

            DOVirtual.DelayedCall(3, () =>
            {
                //CoroutineManager.Ins.Start(OpenPausePopup());

                //IEnumerator OpenPausePopup()
                //{
                //    RDM.Ins.LosePopupModel = new LosePopupModel();
                //    SceneManager.Instance.OpenPopup(SceneId.LOSE_POPUP);
                //    yield return new WaitUntil(() => RDM.Ins.LosePopupModel.ReturnData.Returned != ReturnId.WAIT);
                //    SceneManager.Instance.ClosePopup(SceneId.LOSE_POPUP);
                //}
            });

        }
    }
}
