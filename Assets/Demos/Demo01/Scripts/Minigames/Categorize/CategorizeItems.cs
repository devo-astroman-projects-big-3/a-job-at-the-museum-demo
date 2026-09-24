using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CategorizeItems : MonoBehaviour
{
    
   [SerializeField] private CategorizeMinigameDatabase _categorizeMinigameDatabase;
   
   [SerializeField] private CategorizeCanvasUI _categorizeCanvasUI;
   [SerializeField] private GameObject _categorizeCanvasGO;

   public void InitCategorizeMiniGame(string id)
   {
      _categorizeCanvasGO.SetActive(true);

      CategorizeMinigameData categorizeMinigameData = _categorizeMinigameDatabase.GetById(id);

      _categorizeCanvasUI.SetValues(categorizeMinigameData.CategoryOne,categorizeMinigameData.CategoryTwo);

   }

   public void CloseMinigame()
   {
      _categorizeCanvasGO.SetActive(false);
   }
}
