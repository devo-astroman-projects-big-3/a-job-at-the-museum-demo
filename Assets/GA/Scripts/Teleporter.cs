using UnityEngine;

public class Teleporter : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform[] _startPoints; //0-Street 1-Museum

    [SerializeField] private Transform _heroToTransport;
    #endregion

    #region Public methods
    public void TeleportToStreet()
    {
        TeleportTo(0);
    }

    public void TeleportToMuseum()
    {
        TeleportTo(1);
    }
    #endregion

    #region Private methods
    private void TeleportTo(int index)
    {
        if (_heroToTransport == null)
        {
            Debug.LogWarning("Assign the hero to transport in the Inspector.", this);
            return;
        }

        if (_startPoints == null || index >= _startPoints.Length || _startPoints[index] == null)
        {
            Debug.LogWarning($"Teleport start point {index} is not assigned.", this);
            return;
        }

        CharacterController controller = _heroToTransport.GetComponent<CharacterController>();
        bool controllerWasEnabled = controller != null && controller.enabled;
        if (controllerWasEnabled)
            controller.enabled = false;

        Transform destination = _startPoints[index];
        _heroToTransport.SetPositionAndRotation(destination.position, destination.rotation);

        if (controllerWasEnabled)
            controller.enabled = true;
    }
    #endregion
}
