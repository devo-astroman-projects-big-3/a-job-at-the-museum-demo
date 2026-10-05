using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SlotGroups : MonoBehaviour
{
    #region Fields
   [SerializeField] private Image[] _slotImages;
    #endregion

    #region public properties
   public UnityEvent<int> CorrectDropItem;
    #endregion

    #region Public methods
   public bool CanSetImages(Sprite[] sprites, string groupName = "SlotGroups")
   {
      string prefix = $"{groupName} on '{name}': _slotImages";
      if (sprites == null || _slotImages == null)
      {
         Debug.LogWarning($"{prefix}: {(sprites == null ? "source sprite array" : "image array")} is null.", this);
         return false;
      }

      bool valid = true;
      string counts = $"Required: {sprites.Length}; configured image spaces: {_slotImages.Length}.";
      if (sprites.Length > _slotImages.Length)
      {
         Debug.LogWarning($"{prefix}: not enough image spaces. {counts}", this);
         valid = false;
      }
      for (int i = 0; i < _slotImages.Length; i++)
      {
         if (_slotImages[i] != null)
            continue;
         Debug.LogWarning($"{prefix}: missing Image reference at Element {i} (zero-based). {counts}", this);
         valid = false;
      }
      return valid;
   }

   public void CorrectDrop(int index)
   {
      Debug.Log("_-CorrectDrop-_" + index);
      _slotImages[index].gameObject.SetActive(true);
      CorrectDropItem?.Invoke(index);
   }

   public void SetImages(Sprite[] spriteImgs)
   {
      for (int i = 0; i < _slotImages.Length; i++)
      {
         _slotImages[i].sprite = i < spriteImgs.Length ? spriteImgs[i] : null;
         _slotImages[i].gameObject.SetActive(false);
      }
   }
    #endregion
}
