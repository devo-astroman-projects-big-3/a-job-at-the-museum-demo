using UnityEngine;
using UnityEngine.Events;

public class ElementInventoryItemCombination : MonoBehaviour
{
    
    [SerializeField] private ElementMinigame _elementMinigame;
    [SerializeField] private InventoryItemData _inventoryItem;

    [SerializeField] private string _description;
    public UnityEvent<ElementInventoryItemCombination> CombinationMade = new UnityEvent<ElementInventoryItemCombination>();
    public UnityEvent<string> InventoryItemCombinated = new UnityEvent<string>();

    public ElementMinigame ElementMinigame => _elementMinigame;
    public InventoryItemData InventoryItem => _inventoryItem;

    public void NotifyCombinationMade()
    {
        CombinationMade?.Invoke(this);
    }

    public void NotifyInventoryItemCombinated()
    {
        InventoryItemCombinated?.Invoke(_inventoryItem.Id);
    }

}
