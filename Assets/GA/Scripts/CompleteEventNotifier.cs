using UnityEngine;
using UnityEngine.Events;
using System;

public class CompleteEventNotifier : MonoBehaviour
{
    #region public properties
    public event Action<GameObject> Completed;

    public UnityEvent<GameObject> OnCompleted;
    #endregion

    #region Public methods
    public void NotifyComplete()
    {
        Completed?.Invoke(this.gameObject);
        OnCompleted?.Invoke(this.gameObject);

    }
    #endregion
}
