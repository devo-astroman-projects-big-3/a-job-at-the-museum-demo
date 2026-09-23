using UnityEngine;

[CreateAssetMenu(fileName = "NewPairConnectMinigameData", menuName = "Game/ConnectPairs/PairConnectMinigameData")]
public class PairConnectMinigameData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private PairData[] _pairData;

    public string Id => _id;
    public PairData[] PairData => _pairData;


    
}
