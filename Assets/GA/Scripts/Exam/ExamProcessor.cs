using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class ExamProcessor : MonoBehaviour
{
    #region Fields    
    private bool _examHasPassed = false;
    [SerializeField] private Exam _exam;
    #endregion

    #region public properties
    public bool ExamHasPassed => _examHasPassed;
    #endregion

    #region Unity Callbacks
    public UnityEvent TriggerIfExamIsPassed = new UnityEvent();
    #endregion

    #region Public methods
    public void TryTriggerIfExamIsPassed()
    {
        if(_exam.HasPassedExam())
            TriggerIfExamIsPassed?.Invoke();
    }

    #endregion

    #region Private methods    
    #endregion
}
