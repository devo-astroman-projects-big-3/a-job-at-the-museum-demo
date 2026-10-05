using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    #region Fields
    [SerializeField] private PuzzleMinigameDatabase _puzzleMinigameDatabase;
    [SerializeField] private PuzzleCanvasUI _puzzleCanvasUI;
    [SerializeField] private GameObject _puzzleCanvasGO;
    #endregion

    #region Public methods
    public void InitPuzzleMinigame(string id)
    {
        _puzzleCanvasGO.SetActive(true);

        PuzzleMinigameData puzzleMinigameData = _puzzleMinigameDatabase.GetById(id);


        _puzzleCanvasUI.SetCurrentRunningId(id);

        _puzzleCanvasUI.Setup(puzzleMinigameData.ImageToSlice, puzzleMinigameData.NSlices);

    }

    public void CloseMinigame()
    {
        _puzzleCanvasGO.SetActive(false);
    }
    #endregion
}