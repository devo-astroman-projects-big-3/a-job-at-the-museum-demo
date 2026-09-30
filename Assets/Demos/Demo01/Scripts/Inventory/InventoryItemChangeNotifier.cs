using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class InventoryItemChangeNotifier : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private string _variableName;

    [SerializeField] private bool _newValueOnAdded;
    [SerializeField] private bool _newValueOnDropped;
    

    public void AddItem(string id)
    {
        if (id == _itemData.Id){
            Debug.Log("AddItem " + id);
            _dialogueRunner.VariableStorage.SetValue(_variableName, _newValueOnAdded);
        }
         
    }

    public void RemoveItem(string id)
    {
        if (id == _itemData.Id){
            Debug.Log("DropItem " + id);
            _dialogueRunner.VariableStorage.SetValue(_variableName, _newValueOnDropped);
        }
    }


}
