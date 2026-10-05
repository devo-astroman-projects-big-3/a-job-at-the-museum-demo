using System;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class DialoguerVarsChangeNotifier : MonoBehaviour
{
    #region Fields
    [SerializeField] private DialogueRunner _dialogueRunner;

    [SerializeField] private string[] _variables;

    private IDisposable _storageChangeSubscription;
    #endregion

    #region public properties
    public UnityEvent<string,object> VariableChanged = new UnityEvent<string,object>();
    #endregion

    #region Unity Callbacks
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
    #endregion

    #region Private methods
    private void OnStorageChange(string variableName, object newValue)
    {
        if (_variables == null || Array.IndexOf(_variables, variableName) < 0)
        {
            return;
        }

        VariableChanged?.Invoke(variableName,newValue);
    }
    #endregion
}
