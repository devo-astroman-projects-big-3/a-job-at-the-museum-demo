using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableItems : MonoBehaviour
{
   
   [SerializeField] private Image[] _leftSlotDraggableItemImages;
   [SerializeField] private Image[] _rightSlotDraggableItemImages;
   [SerializeField] private Image[] _allSlotDraggableItemImages;


   public void RandomizePositions()
   {
      if (_allSlotDraggableItemImages == null)
         return;

      var items = new List<Transform>();
      Transform parent = null;
      foreach (Image image in _allSlotDraggableItemImages)
      {
         if (image == null || items.Contains(image.transform))
            continue;

         Transform item = image.transform;
         if (item.parent == null || (parent != null && item.parent != parent))
         {
            Debug.LogWarning("All draggable images must be children of the same layout object to randomize their order.", this);
            return;
         }

         parent = item.parent;
         items.Add(item);
      }

      // Fisher-Yates shuffle of sibling positions; keep the category arrays unchanged.
      for (int i = items.Count - 1; i > 0; i--)
      {
         int j = Random.Range(0, i + 1);
         if (i == j)
            continue;

         int firstIndex = items[i].GetSiblingIndex();
         int secondIndex = items[j].GetSiblingIndex();
         items[i].SetSiblingIndex(secondIndex);
         items[j].SetSiblingIndex(firstIndex);
      }

      if (parent is RectTransform layoutRect)
         LayoutRebuilder.MarkLayoutForRebuild(layoutRect);
   }

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
