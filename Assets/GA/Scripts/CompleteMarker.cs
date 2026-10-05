using UnityEngine;

public class CompleteMarker : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _emptySymbol;
    [SerializeField] private GameObject _completedSymbol;
    #endregion

    #region Public methods
    public void MarkAsComplete()
    {
        _emptySymbol.SetActive(false);
        _completedSymbol.SetActive(true);
    }
    #endregion

    #region Private methods    
    #endregion
}
