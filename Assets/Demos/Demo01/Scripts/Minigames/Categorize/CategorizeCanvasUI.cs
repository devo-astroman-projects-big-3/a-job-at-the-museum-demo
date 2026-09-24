using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CategorizeCanvasUI : MonoBehaviour
{

    [SerializeField] private SlotGroups _slotGroupLeft;
    [SerializeField] private SlotGroups _slotGroupRight;
    [SerializeField] private DraggableItems _draggableItems;

    public UnityEvent OnComplete = new UnityEvent();

    public void SetValues(CategoryData categoryOne,CategoryData categoryTwo)
    {

            //from each CategoryData
            //should set the images in the draggableItems

            //should set the images in the Left SlotGroup
            //should set the images in the Right SlotGroups
            
    }

    public void OnItemLeftPlacedCorrectly(int index)
    {
        _draggableItems.HideLeftItem(index);
        //should check if game is complete
    }

    public void OnItemRightPlacedCorrectly(int index)
    {
        _draggableItems.HideRightItem(index);
        //should check if game is complete
    }

    private void CheckAllCorrects()
    {
        
    }
    
    
}
