using UnityEngine;
using System;

public class GlobalServices : MonoBehaviour
{
    #region Fields

    //[SerializeField] private MoverPlayerService _moverPlayerService;
    [SerializeField] private CharacterControllerMovement _characterControllerMovement;

    private Vector3 _lookAtPosition;
    #endregion

    #region public properties
    public event Action PlayerMovedAndLookingAtPlace;
    #endregion

    #region Unity Callbacks
    private void OnDisable()
    {
        //if (_moverPlayerService != null)
        //    _moverPlayerService.DestinationReached -= OnPlayerReachedDestination;
    }
    #endregion

    #region Public methods
    public void MoveAndLookPlayerTo(Transform position, Transform lookTo)
    {
        if (position == null || lookTo == null)
        {
            Debug.LogWarning("A destination and a look target are required.", this);
            return;
        }
        _lookAtPosition = lookTo.position;
        //_moverPlayerService.DestinationReached -= OnPlayerReachedDestination;

       DisablePlayerThirdPerson();

        //if (_moverPlayerService.GoToAndThenLookAt(position.position, lookTo.position))
            //_moverPlayerService.DestinationReached += OnPlayerReachedDestination;
    }

    public void DisablePlayerThirdPerson()
    {
        _characterControllerMovement.enabled = false;

    }

    public void EnablePlayerThirdPerson()
    {
        _characterControllerMovement.enabled = true;

    }

    public void DisableNavAgent()
    {
        //_moverPlayerService.DeactivateNavmeshAgent();
    }

    public void PlayScreenInfo(int id)
    {
        Debug.Log("playing " + id);
    }
    #endregion

    #region Private methods
    private void OnPlayerReachedDestination()
    {

        //_moverPlayerService.PlayerLookAt(_lookAtPosition);

        //_moverPlayerService.DestinationReached -= OnPlayerReachedDestination;
        PlayerMovedAndLookingAtPlace?.Invoke();
    }
    #endregion
}
