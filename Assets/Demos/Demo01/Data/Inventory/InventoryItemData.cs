using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryItemData", menuName = "Game/Inventory/ItemData")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _inventoryIcon;

    public string Id => _id;
    public Sprite InventoryIcon => _inventoryIcon;
    
}
