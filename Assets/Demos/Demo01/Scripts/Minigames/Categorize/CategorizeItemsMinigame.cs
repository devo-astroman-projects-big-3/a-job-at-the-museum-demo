using UnityEngine;

public class CategorizeItemsMinigame : MonoBehaviour
{
    
   [SerializeField] private CategorizeMinigameDatabase _categorizeMinigameDatabase;
   
   [SerializeField] private CategorizeCanvasUI _categorizeCanvasUI;
   [SerializeField] private GameObject _categorizeCanvasGO;

   public void InitCategorizeMiniGame(string id)
   {
      if (_categorizeMinigameDatabase == null || _categorizeCanvasUI == null || _categorizeCanvasGO == null)
      {
         Debug.LogWarning("Assign the categorize database, canvas UI and canvas GameObject.", this);
         return;
      }
      CategorizeMinigameData categorizeMinigameData = _categorizeMinigameDatabase.GetById(id);
      if (categorizeMinigameData == null)
      {
         Debug.LogWarning($"Categorize minigame '{id}' was not found.", this);
         return;
      }

      bool leftValid = TrySelectImages(categorizeMinigameData.CategoryLeft,
         categorizeMinigameData.CategoryLeftSelectedIndexes, "left", out Sprite[] leftImages);
      bool rightValid = TrySelectImages(categorizeMinigameData.CategoryRight,
         categorizeMinigameData.CategoryRightSelectedIndexes, "right", out Sprite[] rightImages);
      if (!leftValid || !rightValid)
         return;

      _categorizeCanvasGO.SetActive(true);
      _categorizeCanvasUI.HideCongratsMsg();
      _categorizeCanvasUI.SetValues(leftImages, rightImages);
   }

   private bool TrySelectImages(CategoryData category, int[] indexes, string side, out Sprite[] images)
   {
      images = null;
      if (category == null || category.Images == null || indexes == null)
      {
         Debug.LogWarning($"Categorize {side}: assign a category, its images and selected indexes.", this);
         return false;
      }

      images = new Sprite[indexes.Length];
      for (int i = 0; i < indexes.Length; i++)
      {
         int index = indexes[i];
         if (index < 0 || index >= category.Images.Length)
         {
            Debug.LogWarning($"Categorize {side}, category '{category.name}': selected indexes Element {i} " +
               $"contains {index}, but the category has {category.Images.Length} images. Indexes are zero-based.", this);
            images = null;
            return false;
         }
         images[i] = category.Images[index];
      }
      return true;
   }

   public void CloseMinigame()
   {
      _categorizeCanvasGO.SetActive(false);
   }
}
