using UnityEngine;
using UnityEngine.Events;

public class ElementInventoryItemCombination : MonoBehaviour
{
    #region Fields
    [SerializeField] private ElementMinigame _elementMinigame;
    [SerializeField] private InventoryItemData _inventoryItem;
    [SerializeField] private string _description;
    #endregion

    #region public properties
    public UnityEvent<ElementInventoryItemCombination> CombinationMade = new UnityEvent<ElementInventoryItemCombination>();

    public UnityEvent<string> InventoryItemCombinated = new UnityEvent<string>();

    public ElementMinigame ElementMinigame => _elementMinigame;

    public InventoryItemData InventoryItem => _inventoryItem;
    #endregion

    #region Public methods
    public void NotifyCombinationMade()
    {
        Debug.Log("_CombinationMade_");
        CombinationMade?.Invoke(this);
    }

    public void NotifyInventoryItemCombinated()
    {
        Debug.Log("NotifyInventoryItemCombinated");
        InventoryItemCombinated?.Invoke(_inventoryItem.Id);
    }
    #endregion
}
