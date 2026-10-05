using UnityEngine;

public class IntroControl : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject[] _gos;

    private int _iGo = 0;
    #endregion

    #region Public methods
    public void Next()
    {
        if (_gos == null || _gos.Length == 0 || _iGo >= _gos.Length)
            return;

        int previousIndex = _iGo;
        int nextIndex = ++_iGo;

        if (previousIndex >= 0 && previousIndex < _gos.Length && _gos[previousIndex] != null)
            _gos[previousIndex].SetActive(false);

        if (nextIndex < _gos.Length && _gos[nextIndex] != null)
            _gos[nextIndex].SetActive(true);
    }
    #endregion
}
