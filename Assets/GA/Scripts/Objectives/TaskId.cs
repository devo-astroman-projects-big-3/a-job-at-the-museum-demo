using UnityEngine;
using UnityEngine.Events;


public class TaskId : MonoBehaviour
{
    [SerializeField] private string _id;
    private bool _isCompleted;

    public string Id => _id;
    public bool IsCompleted => _isCompleted;

    public void MarkCompleted()
    {
        _isCompleted = true;
    }

}
