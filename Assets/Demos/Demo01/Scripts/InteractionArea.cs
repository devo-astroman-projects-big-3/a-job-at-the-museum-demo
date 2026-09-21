using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class InteractionArea : MonoBehaviour
{   
    public UnityEvent<GameObject> Interacted = new UnityEvent<GameObject>();
    public event Action<GameObject> InteractionMade;

    private readonly HashSet<Collider> _playerColliders = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (isActiveAndEnabled && other.CompareTag("Player"))
            _playerColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _playerColliders.Remove(other);
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

    public void Deactivate()
    {
        enabled = false;
    }

    public void Activate()
    {
        enabled = true;
    }

    private void OnDisable()
    {
        _playerColliders.Clear();
    }
}
