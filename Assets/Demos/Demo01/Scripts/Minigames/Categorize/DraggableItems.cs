using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableItems : MonoBehaviour
{
  
   /* add dragUI */
   /* add dragableImage */
   /* add validDropSlot */
   /* add UnityEvent notify correct */
   /* Hide image of those dragged correctly */
   [SerializeField] private Image[] _leftSlotDraggableItemImages;
   [SerializeField] private Image[] _rightSlotDraggableItemImages;


   public void SetLeftItemImages(Sprite[] spriteImgs)
   {
   
   }

   public void SetLeftRightImages(Sprite[] spriteImgs)
   {
   
   }

   public void HideLeftItem(int index)
   {
      _leftSlotDraggableItemImages[index].gameObject.SetActive(false);
   }

   public void HideRightItem(int index)
   {
      _rightSlotDraggableItemImages[index].gameObject.SetActive(false);
   }



}
