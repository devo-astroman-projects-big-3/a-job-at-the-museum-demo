using UnityEngine;

public class Template : MonoBehaviour
{
   /* #region Fields
    private const string WalkParam = "Walk";
    private const string ReceiveHitParam = "ReceiveHit";
    private const string AttackParam = "Attack";
    private const string DieParam = "Die";
    private const string ShowParam = "Show";
    private const string HideParam = "Hide";

    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyEventManager _enemyEventManager;
    #endregion

    #region public properties
    public Action ReceiveHitAnimationEndsAction;
    public Action AttackAnimationEndsAction;
    public Action AttackAnimationCoreMomentAction;
    public Action DieAnimationEndsAction;
    public Action ShowAnimationEndsAction;
    public Action HideAnimationEndsAction;
    #endregion

    #region Private properties    

    #endregion
    #region Unity Callbacks
    private void OnEnable()
    {

        var behaviours = _animator.GetBehaviours<EndStateBehaviour>();

        Debug.Log("n behaviours: " + behaviours.Length);

        foreach (var b in behaviours)
            b.StateExitAction += HandleAnimationFinished;
        
    }
    private void OnDisable()
    {
        var behaviours = _animator.GetBehaviours<EndStateBehaviour>();
        foreach (var b in behaviours)
            b.StateExitAction -= HandleAnimationFinished;
    }


    #endregion

    #region Public methods
    public void PlayWalk()
    {
        Reset();
        _animator.SetTrigger(WalkParam);
    }

    public void PlayReceiveHit()
    {
        Reset();
        _animator.SetTrigger(ReceiveHitParam);
    }

    public void PlayAttack()
    {
        Reset();
        _animator.SetTrigger(AttackParam);
    }

    public void PlayDie()
    {
        Reset();
        _animator.SetTrigger(DieParam);
    }

    public void PlayShow()
    {
        Reset();
        _animator.SetTrigger(ShowParam);
    }

    public void PlayHide()
    {
        Reset();
        _animator.SetTrigger(HideParam);
    }

	#endregion
    
    #region Private methods
    public void HandleAnimationFinished(string stateName)
    {
        Debug.Log("FLAG00  _ " + stateName);
        if(stateName == "ReceiveHitCopy")
        {

            Debug.Log("FLAG ====>>>> " + stateName);
            _enemyEventManager.NotifyReceiveDamageAnimationEnds();
            ReceiveHitAnimationEndsAction?.Invoke();

        }else if(stateName == "AttackCopy")
        {
            _enemyEventManager.NotifyAttackAnimationEnds();
            AttackAnimationEndsAction?.Invoke();
        }else if(stateName == "DieCopy")
        {
            _enemyEventManager.NotifyDieAnimationEnds();
            DieAnimationEndsAction?.Invoke();
        }else if(stateName == "Show")
        {
            ShowAnimationEndsAction?.Invoke();
        }else if(stateName == "Hide")
        {
            HideAnimationEndsAction?.Invoke();
        }
    }

    public void Reset()
    {
        _animator.ResetTrigger(WalkParam);
        _animator.ResetTrigger(ReceiveHitParam);
        _animator.ResetTrigger(AttackParam);
        _animator.ResetTrigger(DieParam);
        _animator.ResetTrigger(ShowParam);
        _animator.ResetTrigger(HideParam);
    }


    public void OnCoreMoment(String animationName)
    {
        _enemyEventManager.NotifyAttackAnimationCoreMomentAction();
        AttackAnimationCoreMomentAction?.Invoke();
    }

    
    
    #endregion */
}
