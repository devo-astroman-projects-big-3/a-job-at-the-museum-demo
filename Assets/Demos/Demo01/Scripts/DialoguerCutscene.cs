using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Yarn.Unity;

public class DialoguerCutscene : MonoBehaviour
{   

    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _dialogueRunnerHolder;
    [SerializeField] private string _startNode;
    [SerializeField] private MouseCursorUtil _mouseCursorUtil;
    [SerializeField] private bool _playInAwake = false;    

    private void OnEnable(){
        if (_playInAwake)
        {
            PlayerAskToTalk();
        }
    }


    public void PlayerAskToTalk()
    {   
        HandlePlayerMovedAndLookingAtPlace();
    }

    public void TalkingFinished()
    {
        _canvas.sortingOrder = 0;
        _mouseCursorUtil.HideCursor();
    }


    private void HandlePlayerMovedAndLookingAtPlace()
    {
        _canvas.sortingOrder = 10;        
        _dialogueRunnerHolder.GetComponent<DialogueRunner>().StartDialogue(_startNode);
        _mouseCursorUtil.ShowCursor();
    }
}
