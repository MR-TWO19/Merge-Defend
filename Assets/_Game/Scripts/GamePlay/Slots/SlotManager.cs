using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private List<Slot> slots;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetUp(i);
        }
    }

    public Slot GetFreeSlot()
    {
        Slot lastFree = null;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            var slot = slots[i];

            if (!slot.IsUsed)
            {
                lastFree = slot;
            }
            else
            {
                if (lastFree != null)
                    return lastFree;
            }
        }

        return lastFree;
    }

    public Slot GetFrontFreeSlot(Slot currentSlot)
    {
        int startIndex = slots.IndexOf(currentSlot);
        if (startIndex <= 0)
            return null;

        for (int i = 0; i < startIndex; i++)
        {
            var slot = slots[i];
            if (!slot.IsUsed)
            {
                slot.IsUsed = true;
                return slot;
            }
        }

        return null;
    }

    public Slot GetPreviousFreeSlot(Slot currentSlot)
    {
        int startIndex = slots.IndexOf(currentSlot);
        if (startIndex <= 0)
            return null;

        Slot slot = slots[startIndex - 1];
        if (!slot.IsUsed)
        {
            return slot;
        }

        return null;
    }

    public Slot GetSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            return null;
        }
        return slots[slotIndex];
    }

    public int GetIndexSlot(Slot slot)
    {
        if (slot == null || slots.Count <= 0)
        {
            return -1;
        }

        return slots.IndexOf(slot);
    }

    public void ResetAllSlot()
    {
        slots.ForEach(_ =>
        {
            _.IsUsed = false;
        });
    }
}
