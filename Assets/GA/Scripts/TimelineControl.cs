using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour
{
    #region Fields
    [SerializeField] private PlayableDirector director;
    #endregion

    #region Public methods
    public void StartTimeline()
    {
        director.Play();
    }

    public void PauseTimeline()
    {
        director.Pause();
    }

    public void ResumeTimeline()
    {
        Debug.Log("Resume!!! ");
        director.Resume();
    }
    #endregion
}