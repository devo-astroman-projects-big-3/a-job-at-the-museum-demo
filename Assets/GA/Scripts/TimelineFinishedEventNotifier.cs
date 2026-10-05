using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Playables;

public class TimelineFinishedEventNotifier : MonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;
    public event Action<GameObject> Completed;
    public UnityEvent<GameObject> OnCompleted;

    private void OnEnable()
    {
        Debug.Log("flag1");
        _timeline.stopped += HandleTimelineStopped;
    }

    private void OnDisable()
    {
        _timeline.stopped -= HandleTimelineStopped;
    }

    private void HandleTimelineStopped(PlayableDirector director)
    {
        Debug.Log("flag2");
        Completed?.Invoke(gameObject);
        OnCompleted?.Invoke(gameObject);
    }
}
