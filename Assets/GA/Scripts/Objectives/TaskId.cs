using UnityEngine;
using UnityEngine.Events;


public class TaskId : MonoBehaviour
{
    #region Fields
    [SerializeField] private string _id;
    private bool _isCompleted;
    #endregion

    #region public properties
    public string Id => _id;

    public bool IsCompleted => _isCompleted;
    #endregion

    #region Public methods
    public void MarkCompleted()
    {
        _isCompleted = true;
    }
    #endregion
}
