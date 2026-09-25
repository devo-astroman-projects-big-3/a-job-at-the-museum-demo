using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    
    [SerializeField] private PuzzleMinigameDatabase _puzzleMinigameDatabase;
    [SerializeField] private PuzzleCanvasUI _puzzleCanvasUI;
    [SerializeField] private GameObject _puzzleCanvasGO;


    public void InitPuzzleMinigame(string id)
    {
        _puzzleCanvasGO.SetActive(true);

        PuzzleMinigameData puzzleMinigameData = _puzzleMinigameDatabase.GetById(id);

        _puzzleCanvasUI.Setup(puzzleMinigameData.ImageToSlice);

    }

    public void CloseMinigame()
    {
        _puzzleCanvasGO.SetActive(false);
    }

}