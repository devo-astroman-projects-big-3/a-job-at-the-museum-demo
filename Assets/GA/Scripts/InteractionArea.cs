using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class InteractionArea : MonoBehaviour
{
    #region Fields
    private readonly HashSet<Collider> _playerColliders = new HashSet<Collider>();
    #endregion

    #region public properties
    public UnityEvent<GameObject> Interacted = new UnityEvent<GameObject>();

    public event Action<GameObject> InteractionMade;

    public event Action<GameObject> EntersArea;

    public event Action<GameObject> ExitsArea;
    #endregion

    #region Unity Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (isActiveAndEnabled && other.CompareTag("Player"))
        {
            _playerColliders.Add(other);
            EntersArea?.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _playerColliders.Remove(other);
        ExitsArea?.Invoke(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActiveAndEnabled && other.CompareTag("Player"))
            _playerColliders.Add(other);
    }

    private void Update()
    {
        _playerColliders.RemoveWhere(collider =>
            collider == null || !collider.enabled || !collider.gameObject.activeInHierarchy);

        if (_playerColliders.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            
            Interacted?.Invoke(gameObject);
            InteractionMade?.Invoke(gameObject);
            
        }
    }

    private void OnDisable()
    {
        _playerColliders.Clear();
    }
    #endregion

    #region Public methods
    public void Deactivate()
    {
        enabled = false;
    }

    public void Activate()
    {
        enabled = true;
    }
    #endregion
}
