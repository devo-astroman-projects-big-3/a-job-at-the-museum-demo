using UnityEngine;
using UnityEngine.Events;
using System;
using Yarn.Unity;

public class DialogueFinishedEventNotifier : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;
    public event Action<GameObject> Completed;
    public UnityEvent<GameObject> OnCompleted;

    private void OnEnable()
    {
        _dialogueRunner.onDialogueComplete.AddListener(OnDialogueFinished);
    }

    private void OnDisable()
    {
        _dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueFinished);
    }

    private void OnDialogueFinished()
    {
        
        Completed?.Invoke(gameObject);
        OnCompleted?.Invoke(gameObject);
    }
}
