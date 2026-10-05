using UnityEngine;
using UnityEngine.Events;
using System;

public class CompleteEventNotifier : MonoBehaviour
{
    
    public event Action<GameObject> Completed;
    public UnityEvent<GameObject> OnCompleted;

    public void NotifyComplete()
    {
        Completed?.Invoke(this.gameObject);
        OnCompleted?.Invoke(this.gameObject);

    }
}
