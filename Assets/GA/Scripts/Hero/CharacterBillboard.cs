using UnityEngine;


public class CharacterBillboard : MonoBehaviour
{
     private Camera _mainCamera;

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
}