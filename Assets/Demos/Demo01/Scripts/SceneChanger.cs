using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{   
    [SerializeField] private string _gameplay;
    [SerializeField] private string _outro;

    public void GoGameplay()
    {
        ChangeScene(_gameplay);
    }

    public void GoOutro()
    {
        ChangeScene(_outro);
    }

    private void ChangeScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName) || !Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning($"Scene '{sceneName}' is not assigned or cannot be loaded. Check the Build Settings.", this);
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
   
}
