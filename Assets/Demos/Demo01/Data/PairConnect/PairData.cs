using UnityEngine;

[CreateAssetMenu(fileName = "NewPairData", menuName = "Game/ConnectPairs/PairData")]
public class PairData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _text;
    [SerializeField] private Sprite _image;


    public string Id => _id;    
    public string Text => _text;
    public Sprite Image => _image;
}
