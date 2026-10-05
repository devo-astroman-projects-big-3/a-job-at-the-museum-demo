using UnityEngine;
using UnityEngine.UI;

public class ConnectPairs : MonoBehaviour
{
    #region Fields
    [SerializeField] private PairConnectMinigameDatabase _pairConnectMinigameDatabase;
    [SerializeField] private ConnectPairsCanvasUI _connectPairsCanvasUI;
    [SerializeField] private GameObject _connectPairsCanvasGO;
    #endregion

    #region Public methods
    public void InitConnectPairsMiniGame(string id)
    {


        _connectPairsCanvasGO.SetActive(true);

        PairConnectMinigameData pairConnectMinigameData = _pairConnectMinigameDatabase.GetById(id);

        _connectPairsCanvasUI.SetCurrentRunningId(id);
        _connectPairsCanvasUI.SetValues(pairConnectMinigameData.PairData);

    }

    public void CloseMinigame()
    {
        _connectPairsCanvasGO.SetActive(false);
    }
    #endregion
}