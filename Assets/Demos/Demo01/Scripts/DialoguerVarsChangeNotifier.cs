using System;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class DialoguerVarsChangeNotifier : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private string[] _variables;
    public UnityEvent<string,object> VariableChanged = new UnityEvent<string,object>();

    private IDisposable _storageChangeSubscription;

    private void OnEnable()
    {
        Debug.Log("_dialogueRunner " + _dialogueRunner);
        Debug.Log("_dialogueRunner.VariableStorage " + _dialogueRunner.VariableStorage);
        _storageChangeSubscription = _dialogueRunner.VariableStorage.AddChangeListener(OnStorageChange);
    }

    private void OnDisable()
    {
        _storageChangeSubscription?.Dispose();
        _storageChangeSubscription = null;
    }

    private void OnStorageChange(string variableName, object newValue)
    {
        if (_variables == null || Array.IndexOf(_variables, variableName) < 0)
        {
            return;
        }

        VariableChanged?.Invoke(variableName,newValue);
    }
}
