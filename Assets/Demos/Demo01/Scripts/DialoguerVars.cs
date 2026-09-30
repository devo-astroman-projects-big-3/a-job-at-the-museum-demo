using UnityEngine;
using Yarn.Unity;

public class DialoguerVars : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;


    public void SetToTrue(string variableName)
    {
        _dialogueRunner.VariableStorage.SetValue(variableName, true);
    }

    public void SetToFalse(string variableName)
    {
        _dialogueRunner.VariableStorage.SetValue(variableName, false);
    }
    

    public void ChangeVariable(string variableName, object newValue)
    {
        if (_dialogueRunner == null || _dialogueRunner.VariableStorage == null)
        {
            Debug.LogWarning("Dialogue runner or variable storage is not assigned.", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(variableName))
        {
            Debug.LogWarning("A Yarn variable name is required.", this);
            return;
        }

        if (!variableName.StartsWith("$"))
            variableName = "$" + variableName;

        switch (newValue)
        {
            case string stringValue:
                _dialogueRunner.VariableStorage.SetValue(variableName, stringValue);
                break;
            case bool boolValue:
                _dialogueRunner.VariableStorage.SetValue(variableName, boolValue);
                break;
            case float floatValue:
                _dialogueRunner.VariableStorage.SetValue(variableName, floatValue);
                break;
            case int intValue:
                _dialogueRunner.VariableStorage.SetValue(variableName, (float)intValue);
                break;
            default:
                Debug.LogWarning("Yarn variables require a string, bool, float, or int value.", this);
                break;
        }
    }
}
