using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ProximityPrompt : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Text _EInteractLabel;

    [SerializeField] private InteractionArea _interactionArea;

    private Coroutine _cooldownCoroutine;

    private bool _isOnCooldown;

    private bool _labelWasEnabled;

    private bool _interactionWasEnabled;
    #endregion

    #region public properties
    public UnityEvent<GameObject> Interacted = new UnityEvent<GameObject>();
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        _interactionArea.InteractionMade += HandleInteractionMade;
        _interactionArea.EntersArea += HandleEntersArea;
        _interactionArea.ExitsArea += HandleExitsArea;
    }

    private void OnDisable()
    {

        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }

        RestoreAfterCooldown();

        _interactionArea.InteractionMade -= HandleInteractionMade;
        _interactionArea.EntersArea -= HandleEntersArea;
        _interactionArea.ExitsArea -= HandleExitsArea;
    }

    private void Start()
    {
        _EInteractLabel.enabled = false;
    }
    #endregion

    #region Public methods
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
    #endregion

    #region Private methods
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

    private void HandleEntersArea(GameObject interactedObject)
    {
        _EInteractLabel.enabled = true;
    }

    private void HandleExitsArea(GameObject interactedObject)
    {
        _EInteractLabel.enabled = false;
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
    #endregion
}
