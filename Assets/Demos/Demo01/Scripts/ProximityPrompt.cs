using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ProximityPrompt : MonoBehaviour
{
    [SerializeField] private TMP_Text _EInteractLabel;
    [SerializeField] private InteractionArea _interactionArea;

    public UnityEvent<GameObject> Interacted = new UnityEvent<GameObject>();

    private Coroutine _cooldownCoroutine;
    private bool _isOnCooldown;
    private bool _labelWasEnabled;
    private bool _interactionWasEnabled;

    private void OnEnable()
    {
        _interactionArea.InteractionMade += HandleInteractionMade;
    }

    private void OnDisable()
    {

        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }

        RestoreAfterCooldown();
    }

    public void SetEInteractLabel(string text)
    {
        if (_EInteractLabel != null)
            _EInteractLabel.text = text;
    }

    public void DeactivateInteraction()
    {
        _interactionArea.Deactivate();
        _EInteractLabel.enabled = false;
    }

    public void ActivateInteraction()
    {
        _interactionArea.Activate();
        _EInteractLabel.enabled = true;
    }


    private void HandleInteractionMade(GameObject interactedObject)
    {
        if (_isOnCooldown || !isActiveAndEnabled)
            return;

        _isOnCooldown = true;
        _labelWasEnabled = _EInteractLabel.enabled;
        _interactionWasEnabled = _interactionArea.enabled;
        _interactionArea.Deactivate();
        _EInteractLabel.enabled = false;
        _cooldownCoroutine = StartCoroutine(Cooldown());

        Interacted?.Invoke(interactedObject);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2f);

        _cooldownCoroutine = null;
        RestoreAfterCooldown();
    }

    private void RestoreAfterCooldown()
    {
        if (!_isOnCooldown)
            return;

        _isOnCooldown = false;

        if (_interactionArea != null)
            _interactionArea.enabled = _interactionWasEnabled;

        if (_EInteractLabel != null)
            _EInteractLabel.enabled = _labelWasEnabled;
    }





}
