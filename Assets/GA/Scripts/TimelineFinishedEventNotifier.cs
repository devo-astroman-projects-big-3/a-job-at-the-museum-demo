using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Playables;

public class TimelineFinishedEventNotifier : MonoBehaviour
{
    #region Fields
    [SerializeField] private PlayableDirector _timeline;
    #endregion

    #region public properties
    public event Action<GameObject> Completed;

    public UnityEvent<GameObject> OnCompleted;
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        Debug.Log("flag1");
        _timeline.stopped += HandleTimelineStopped;
    }

    private void OnDisable()
    {
        _timeline.stopped -= HandleTimelineStopped;
    }
    #endregion

    #region Private methods
    private void HandleTimelineStopped(PlayableDirector director)
    {
        Debug.Log("flag2");
        Completed?.Invoke(gameObject);
        OnCompleted?.Invoke(gameObject);
    }
    #endregion
}
