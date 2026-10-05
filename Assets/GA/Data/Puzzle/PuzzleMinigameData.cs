using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPuzzleMinigameData", menuName = "Game/Puzzle/PuzzleMinigameData")]
public class PuzzleMinigameData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _imageToSlice;
    [SerializeField] private int _nSlices;

    public string Id => _id;
    public Sprite ImageToSlice => _imageToSlice;
    public int NSlices => _nSlices;
    
}
