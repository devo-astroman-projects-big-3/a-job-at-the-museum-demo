using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ExamTester : MonoBehaviour
{
    [SerializeField] private Exam _exam;
    [SerializeField] private bool _test;
    private void Update()
    {
        if (_test)
        {
            _exam.InitialSetup();
            _test = false;
        }


    }
}
