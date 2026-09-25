using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPuzzleMinigameData", menuName = "Game/Puzzle/PuzzleMinigameData")]
public class PuzzleMinigameData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _imageToSlice;

    public string Id => _id;
    public Sprite ImageToSlice => _imageToSlice;
    
}
