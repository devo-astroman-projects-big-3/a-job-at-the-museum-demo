using UnityEngine;

public class ActivatorsUtils : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject[] _objectsToActivate;
    #endregion

    #region Public methods
    public void ActivateObjects()
    {
        SetObjectsActive(true);
    }

    public void DeactivateObjects()
    {
        SetObjectsActive(false);
    }
    #endregion

    #region Private methods
    private void SetObjectsActive(bool active)
    {
        if (_objectsToActivate == null)
            return;

        foreach (GameObject objectToActivate in _objectsToActivate)
        {
            if (objectToActivate != null)
                objectToActivate.SetActive(active);
        }
    }
    #endregion
}
