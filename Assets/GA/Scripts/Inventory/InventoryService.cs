using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryService : MonoBehaviour
{
    [SerializeField] private InventoryItemDatabase _inventoryItemDatabase;

    private List<InventoryItemData> _takenItems = new();

    public IReadOnlyList<InventoryItemData> Items => _takenItems;

    public UnityEvent<string> OnAddedItem;
    public UnityEvent<string> OnDroppedItem;


    private void Start()
    {
        /* testing purposes */

        //AddItem("keys");
    }

    public void AddItem(string id)
    {
        if (_inventoryItemDatabase == null)
        {
            Debug.LogWarning("Inventory item database is not assigned.", this);
            return;
        }

        InventoryItemData item = _inventoryItemDatabase.GetById(id);
        if (item != null)
        {
            _takenItems.Add(item);
            Debug.Log("Added_ " + item.Id);
            OnAddedItem?.Invoke(item.Id);
        }
    }

    public void RemoveItem(string id)
    {
        InventoryItemData item = GetTakenItemBy(id);
        if (item != null && _takenItems.Remove(item))
            OnDroppedItem?.Invoke(item.Id);
    }

    public InventoryItemData GetTakenItemBy(string id)
    {
        foreach (InventoryItemData item in _takenItems)
        {
            if (item != null && item.Id == id)
                return item;
        }

        return null;
    }

    // Returns a copy of the range, including both zero-based indexes.
    public List<InventoryItemData> GetTakenItemsFromIndexes(int startIndex, int endIndex)
    {
        if (startIndex < 0 || startIndex >= _takenItems.Count)
            throw new System.ArgumentOutOfRangeException(nameof(startIndex));

        if (endIndex < startIndex || endIndex >= _takenItems.Count)
            throw new System.ArgumentOutOfRangeException(nameof(endIndex));

        return _takenItems.GetRange(startIndex, endIndex - startIndex + 1);
    }

}
