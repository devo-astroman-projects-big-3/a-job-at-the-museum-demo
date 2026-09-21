using System;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class PlayerThirdPerson : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private ThirdPersonController _thirdPersonController;

    public void SetEnable(bool enabledValue)
    {
        _characterController.enabled = enabledValue;
        _thirdPersonController.enabled = enabledValue;
    }
}
