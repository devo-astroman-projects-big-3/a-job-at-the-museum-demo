using UnityEngine;

public class ActivatorsUtils : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;

    public void ActivateObjects()
    {
        SetObjectsActive(true);
    }

    public void DeactivateObjects()
    {
        SetObjectsActive(false);
    }

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
}
