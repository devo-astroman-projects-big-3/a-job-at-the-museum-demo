using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

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
}