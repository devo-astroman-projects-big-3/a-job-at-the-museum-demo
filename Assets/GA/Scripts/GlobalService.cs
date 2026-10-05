using UnityEngine;
using System;

public class GlobalService : MonoBehaviour
{
    #region Fields
    [SerializeField] private MoverPlayerService _moverPlayerService;

    [SerializeField] private PlayerThirdPerson _playerThirdPerson;

    private Vector3 _lookAtPosition;
    #endregion

    #region public properties
    public event Action PlayerMovedAndLookingAtPlace;
    #endregion

    #region Unity Callbacks
    private void OnDisable()
    {
        if (_moverPlayerService != null)
            _moverPlayerService.DestinationReached -= OnPlayerReachedDestination;
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
        _moverPlayerService.DestinationReached -= OnPlayerReachedDestination;

       DisablePlayerThirdPerson();

        if (_moverPlayerService.GoToAndThenLookAt(position.position, lookTo.position))
            _moverPlayerService.DestinationReached += OnPlayerReachedDestination;
    }

    public void DisablePlayerThirdPerson()
    {
        _playerThirdPerson.SetEnable(false);
    }

    public void EnablePlayerThirdPerson()
    {
        _playerThirdPerson.SetEnable(true);
    }

    public void DisableNavAgent()
    {
        _moverPlayerService.DeactivateNavmeshAgent();
    }
    #endregion

    #region Private methods
    private void OnPlayerReachedDestination()
    {

        //_moverPlayerService.PlayerLookAt(_lookAtPosition);

        _moverPlayerService.DestinationReached -= OnPlayerReachedDestination;
        PlayerMovedAndLookingAtPlace?.Invoke();
    }
    #endregion
}
