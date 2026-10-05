using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Yarn.Unity;

public class Dialoguer : MonoBehaviour
{
    #region Fields
    [SerializeField] private Canvas _canvas;

    [SerializeField] private ProximityPrompt _proximityPrompt;

    [SerializeField] private GameObject _dialogueRunnerHolder;

    [SerializeField] private string _startNode;

    [SerializeField] private MouseCursorUtil _mouseCursorUtil;

    [SerializeField] private GlobalServices _globalServices;
    #endregion

    #region Public methods
    public void PlayerAskToTalk()
    {   
        HandlePlayerMovedAndLookingAtPlace();
    }

    public void TalkingFinished()
    {
        _canvas.sortingOrder = 0;
        _proximityPrompt.gameObject.SetActive(true);
        _mouseCursorUtil.HideCursor();

         _globalServices.EnablePlayerThirdPerson();
        
    }
    #endregion

    #region Private methods
    private void HandlePlayerMovedAndLookingAtPlace()
    {
        _canvas.sortingOrder = 10;
        _globalServices.DisablePlayerThirdPerson();
        _proximityPrompt.gameObject.SetActive(false);
        _dialogueRunnerHolder.GetComponent<DialogueRunner>().StartDialogue(_startNode);
        _mouseCursorUtil.ShowCursor();

    }
    #endregion
}
