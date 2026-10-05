using UnityEngine;
using TMPro;
using System.Text;

public class HudUI : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Text _objectiveList;
    #endregion

    #region Public methods
    public void SetObjectiveList(string[] objectives, bool[] completionState)
    {
        if (_objectiveList == null)
        {
            Debug.LogWarning("Objective list text is not assigned.", this);
            return;
        }

        if (objectives == null || objectives.Length == 0)
        {
            _objectiveList.text = string.Empty;
            return;
        }

        StringBuilder text = new StringBuilder();
        for (int i = 0; i < objectives.Length; i++)
        {
            if (i > 0)
                text.Append('\n');

            text.Append(objectives[i]);
            if (completionState != null && i < completionState.Length && completionState[i])
                text.Append(" - (Completed)");
        }

        _objectiveList.text = text.ToString();
    }
    #endregion
}
