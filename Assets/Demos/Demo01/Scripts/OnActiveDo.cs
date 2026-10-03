using UnityEngine;
using UnityEngine.Events;

public class OnActiveDo : MonoBehaviour
{
    public UnityEvent OnActivated = new UnityEvent();

    private void OnEnable()
    {
        OnActivated?.Invoke();
    }
}
