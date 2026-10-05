using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    #region Fields
    [SerializeField] private string _gameplay;

    [SerializeField] private string _outro;
    #endregion

    #region Public methods
    public void GoGameplay()
    {
        ChangeScene(_gameplay);
    }

    public void GoOutro()
    {
        ChangeScene(_outro);
    }
    #endregion

    #region Private methods
    private void ChangeScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName) || !Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning($"Scene '{sceneName}' is not assigned or cannot be loaded. Check the Build Settings.", this);
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
