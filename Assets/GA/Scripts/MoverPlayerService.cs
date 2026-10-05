using System;
using UnityEngine;
using UnityEngine.AI;

public class MoverPlayerService : MonoBehaviour
{
    #region Fields
    [SerializeField] private NavMeshAgent _agent;

    private bool _waitingForDestination;

    private bool _lookAtDestinationWhenReached;

    private Vector3 _lookAtPosition;
    #endregion

    #region public properties
    public bool IsMoving =>
        _agent != null &&
        _agent.hasPath &&
        !_agent.isStopped;

    public event Action DestinationReached;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();
        
    }

    private void Update()
    {
        CheckDestinationReached();
    }
    #endregion

    #region Public methods
    public bool GoTo(Vector3 position)
    {
        _lookAtDestinationWhenReached = false;

        _agent.enabled = true;
        if (!_agent.isOnNavMesh)
        {
            Debug.LogWarning("The player is not positioned on a NavMesh.");
            return false;
        }

        

        _agent.isStopped = false;
        _waitingForDestination = _agent.SetDestination(position);

        return _waitingForDestination;
    }

    public bool GoToAndThenLookAt(Vector3 position, Vector3 lookAtPosition)
    {
        if (!GoTo(position))
            return false;

        _lookAtPosition = lookAtPosition;
        _lookAtDestinationWhenReached = true;

        return true;
    }

    public void Stop()
    {
        if (!_agent.isOnNavMesh)
            return;

        _agent.isStopped = true;
        _agent.ResetPath();
        
        _waitingForDestination = false;
        _lookAtDestinationWhenReached = false;
    }

    public void DeactivateNavmeshAgent()
    {
        _agent.enabled = false;
    }

    public void PlayerLookAt(Vector3 lookAt)
    {
        _agent.transform.LookAt(lookAt);
    }
    #endregion

    #region Private methods
    private void CheckDestinationReached()
    {
        if (!_waitingForDestination || _agent.pathPending)
            return;

        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Stop();
            return;
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
            return;

        if (_agent.hasPath && _agent.velocity.sqrMagnitude > 0.01f)
            return;

        _waitingForDestination = false;

        if (_lookAtDestinationWhenReached)
        {
            PlayerLookAt(_lookAtPosition);
            _lookAtDestinationWhenReached = false;
        }

        DestinationReached?.Invoke();
    }
    #endregion
}
