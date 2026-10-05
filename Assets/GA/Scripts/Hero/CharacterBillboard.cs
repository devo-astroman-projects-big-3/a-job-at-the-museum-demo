using UnityEngine;


public class CharacterBillboard : MonoBehaviour
{
    #region Fields
     private Camera _mainCamera;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera == null)
            return;

        transform.forward = _mainCamera.transform.forward;
    }
    #endregion
}