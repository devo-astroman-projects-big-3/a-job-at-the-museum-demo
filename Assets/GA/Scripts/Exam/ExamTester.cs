using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ExamTester : MonoBehaviour
{
    #region Fields
    [SerializeField] private Exam _exam;
    [SerializeField] private bool _test;
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        if (_test)
        {
            _exam.InitialSetup();
            _test = false;
        }


    }
    #endregion

    #region Public methods
    public void Passed()
    {
        Debug.Log("Passed!!!");
    }

    public void Fail()
    {
        Debug.Log("Fail!!!");
    }
    #endregion
}
