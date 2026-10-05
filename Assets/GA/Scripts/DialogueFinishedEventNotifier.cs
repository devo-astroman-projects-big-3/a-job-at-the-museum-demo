using UnityEngine;
using UnityEngine.Events;
using System;
using Yarn.Unity;

public class DialogueFinishedEventNotifier : MonoBehaviour
{
    #region Fields
    [SerializeField] private DialogueRunner _dialogueRunner;
    #endregion

    #region public properties
    public event Action<GameObject> Completed;

    public UnityEvent<GameObject> OnCompleted;
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueFinished);
    }

    private void OnDisable()
    {
        _dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueFinished);
    }
    #endregion

    #region Private methods
    private void OnDialogueFinished()
    {
        
        Completed?.Invoke(gameObject);
        OnCompleted?.Invoke(gameObject);
    }
    #endregion
}
