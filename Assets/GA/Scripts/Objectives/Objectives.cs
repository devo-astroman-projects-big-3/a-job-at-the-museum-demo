using UnityEngine;
using UnityEngine.Events;

public class Objectives : MonoBehaviour
{
    #region Fields
    [SerializeField] private HudUI _hudUI;
    [SerializeField] private TasksCompleteState _screenInfoTasks;
    private int _totalScreenInfo;
    private int _nScreenInfoCompleted;
    [SerializeField] private TasksCompleteState _minigamesTasks;
    private int _totalMinigames;
    private int _nMinigamesCompleted;
    private bool _firstTimeTalkToEmmaCompleted;
    private bool _timeToExam = false;
    #endregion

    #region public properties
    public UnityEvent IsTimeToExam = new UnityEvent();
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        SetInitialSetup(
            _screenInfoTasks != null ? _screenInfoTasks.NTasks : _totalScreenInfo,
            _screenInfoTasks != null ? _screenInfoTasks.NCompletedTasks : _nScreenInfoCompleted,
            _minigamesTasks != null ? _minigamesTasks.NTasks : _totalMinigames,
            _minigamesTasks != null ? _minigamesTasks.NCompletedTasks : _nMinigamesCompleted);
    }
    #endregion

    #region Public methods
    public void SetInitialSetup(int totalScreenInfo, int nScreenInfoCompleted, int totalMinigames, int nMinigamesCompleted)
    {
        _totalScreenInfo = Mathf.Max(0, totalScreenInfo);
        _nScreenInfoCompleted = Mathf.Clamp(nScreenInfoCompleted, 0, _totalScreenInfo);
        _totalMinigames = Mathf.Max(0, totalMinigames);
        _nMinigamesCompleted = Mathf.Clamp(nMinigamesCompleted, 0, _totalMinigames);
        UpdateObjectives();
    }

    public void FirstTimeTalkToEmmaCompleted()
    {
        _firstTimeTalkToEmmaCompleted = true;
        UpdateObjectives();
    }

    public void OneScreenInfoCompleted(string idScreenInfo, int completedCount, int totalCount)
    {
        _totalScreenInfo = Mathf.Max(0, totalCount);
        _nScreenInfoCompleted = Mathf.Clamp(completedCount, 0, _totalScreenInfo);
        UpdateObjectives();
    }

    public void AllScreenInfoAreCompleted()
    {
        _nScreenInfoCompleted = _totalScreenInfo;
        UpdateObjectives();
    }

    public void OneMinigameCompleted(string idMingame, int completedCount, int totalCount)
    {
        _totalMinigames = Mathf.Max(0, totalCount);
        _nMinigamesCompleted = Mathf.Clamp(completedCount, 0, _totalMinigames);
        UpdateObjectives();
    }

    public void AllMinigamesAreCompleted()
    {
        _nMinigamesCompleted = _totalMinigames;
        UpdateObjectives();
    }
    #endregion

    #region Private methods
    private void UpdateObjectives()
    {
        bool wasTimeToExam = _timeToExam;
        _timeToExam = _totalScreenInfo > 0 && _nScreenInfoCompleted == _totalScreenInfo
            && _totalMinigames > 0 && _nMinigamesCompleted == _totalMinigames;

        RefreshHud();

        if (_timeToExam && !wasTimeToExam)
            IsTimeToExam?.Invoke();
    }

    private void RefreshHud()
    {
        if (_hudUI == null)
        {
            Debug.LogWarning("HUD UI is not assigned.", this);
            return;
        }

        if (!_firstTimeTalkToEmmaCompleted)
        {
            _hudUI.SetObjectiveList(
                new string[] { "Talk to museum director" },
                new bool[] { false });
            return;
        }

        if (_timeToExam) //should go to take the exam
        {
            _hudUI.SetObjectiveList(
            new string[]
            {
                "Talk to museum director",
                $"Screens learnt {_nScreenInfoCompleted}/{_totalScreenInfo}",
                $"Minigames completed {_nMinigamesCompleted}/{_totalMinigames}",
                "Take interview with museum director",
            },
            new bool[]
            {
                _firstTimeTalkToEmmaCompleted,
                _totalScreenInfo > 0 && _nScreenInfoCompleted == _totalScreenInfo,
                _totalMinigames > 0 && _nMinigamesCompleted == _totalMinigames,
                false
            });

            return;
        }

        _hudUI.SetObjectiveList(
            new string[]
            {
                "Talk to museum director",
                $"Screens learnt {_nScreenInfoCompleted}/{_totalScreenInfo}",
                $"Minigames completed {_nMinigamesCompleted}/{_totalMinigames}"
            },
            new bool[]
            {
                _firstTimeTalkToEmmaCompleted,
                _totalScreenInfo > 0 && _nScreenInfoCompleted == _totalScreenInfo,
                _totalMinigames > 0 && _nMinigamesCompleted == _totalMinigames
            });
    }
    #endregion
}
