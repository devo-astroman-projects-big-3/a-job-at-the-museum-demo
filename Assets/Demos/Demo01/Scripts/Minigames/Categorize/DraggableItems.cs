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
      SetImages(_leftSlotDraggableItemImages, spriteImgs);
   }

   public void SetLeftRightImages(Sprite[] spriteImgs)
   {
      SetImages(_rightSlotDraggableItemImages, spriteImgs);
   }

   private static void SetImages(Image[] images, Sprite[] sprites)
   {
      for (int i = 0; i < images.Length; i++)
      {
         images[i].sprite = i < sprites.Length ? sprites[i] : null;
         images[i].gameObject.SetActive(i < sprites.Length);
      }
   }

   public bool CanSetImages(Sprite[] left, Sprite[] right)
   {
      bool leftValid = CanSetImages(_leftSlotDraggableItemImages, left, "_leftSlotDraggableItemImages");
      bool rightValid = CanSetImages(_rightSlotDraggableItemImages, right, "_rightSlotDraggableItemImages");
      return leftValid && rightValid;
   }

   private bool CanSetImages(Image[] images, Sprite[] sprites, string fieldName)
   {
      string prefix = $"draggableItems on '{name}': {fieldName}";
      if (images == null || sprites == null)
      {
         Debug.LogWarning($"{prefix}: {(sprites == null ? "source sprite array" : "image array")} is null.", this);
         return false;
      }

      bool valid = true;
      string counts = $"Required: {sprites.Length}; configured image spaces: {images.Length}.";
      if (sprites.Length > images.Length)
      {
         Debug.LogWarning($"{prefix}: not enough image spaces. {counts}", this);
         valid = false;
      }
      for (int i = 0; i < images.Length; i++)
      {
         if (images[i] != null)
            continue;
         Debug.LogWarning($"{prefix}: missing Image reference at Element {i} (zero-based). {counts}", this);
         valid = false;
      }
      return valid;
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
