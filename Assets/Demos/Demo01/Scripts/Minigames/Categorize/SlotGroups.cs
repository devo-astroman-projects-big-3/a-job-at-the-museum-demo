using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SlotGroups : MonoBehaviour
{


   [SerializeField] private Image[] _slotImages;
   
   /* public void ShowImage1() */
   /* reset ... */

   /* UnityEvent Invoke when 4 images are shown */

   public void CorrectDrop(int index)
   {
      _slotImages[index].gameObject.SetActive(true);
   }

   public void SetImages(Sprite[] spriteImgs)
   {
      
   }


}
