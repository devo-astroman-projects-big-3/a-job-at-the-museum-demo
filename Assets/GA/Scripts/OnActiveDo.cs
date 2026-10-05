using UnityEngine;
using UnityEngine.Events;

public class OnActiveDo : MonoBehaviour
{
    #region public properties
    public UnityEvent OnActivated = new UnityEvent();
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        OnActivated?.Invoke();
    }
    #endregion
}
