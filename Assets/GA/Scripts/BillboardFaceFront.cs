using UnityEngine;

public class BillboardFaceFront : MonoBehaviour
{
    #region Fields
    private Camera mainCamera;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
    #endregion
}
