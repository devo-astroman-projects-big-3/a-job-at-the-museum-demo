using UnityEngine;
using UnityEngine.Events;


public class TasksCompleteState : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private string _description;
    [SerializeField] private TaskId[] _allTasksId;

    public UnityEvent<string, int, int> OnCompleted = new UnityEvent<string, int, int>();
    public UnityEvent OnAllCompleted = new UnityEvent();

    public int NTasks
    {
        get
        {
            int count = 0;
            if (_allTasksId != null)
            {
                foreach (TaskId task in _allTasksId)
                {
                    if (task != null)
                        count++;
                }
            }
            return count;
        }
    }

    public int NCompletedTasks
    {
        get
        {
            int count = 0;
            if (_allTasksId != null)
            {
                foreach (TaskId task in _allTasksId)
                {
                    if (task != null && task.IsCompleted)
                        count++;
                }
            }
            return count;
        }
    }

    public void NotifyIsCompleted(string idTask)
    {
        Debug.Log("NotifyIsCompleted " + idTask);


        if (_allTasksId == null || string.IsNullOrWhiteSpace(idTask))
            return;

        TaskId matchingState = null;
        int completedCount = 0;
        int totalCount = 0;

        foreach (TaskId state in _allTasksId)
        {
            if (state == null)
                continue;

            totalCount++;
            if (state.IsCompleted)
                completedCount++;

            if (matchingState == null && state.Id == idTask)
                matchingState = state;
        }

        if (matchingState == null || matchingState.IsCompleted)
            return;

        matchingState.MarkCompleted();
        completedCount++;

        OnCompleted?.Invoke(matchingState.Id, completedCount, totalCount);

        if (completedCount == totalCount)
            OnAllCompleted?.Invoke();
    }

}
