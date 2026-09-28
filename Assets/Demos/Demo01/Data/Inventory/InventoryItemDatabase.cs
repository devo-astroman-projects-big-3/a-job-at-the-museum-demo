using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItemDatabase", menuName = "Game/Inventory/InventoryItemDatabase")]
public class InventoryItemDatabase : ScriptableObject
{
    [SerializeField]
    private List<InventoryItemData> _items = new();

    public IReadOnlyList<InventoryItemData> Items => _items;

    public InventoryItemData GetById(string id)
    {
        foreach (InventoryItemData item in _items)
        {
            if (item != null && item.Id == id)
                return item;
        }

        Debug.LogWarning($"InventoryItemData info not found: {id}");
        return null;
    }

}
