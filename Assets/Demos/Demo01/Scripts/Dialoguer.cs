using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Yarn.Unity;

public class Dialoguer : MonoBehaviour
{   

    [SerializeField] private Canvas _canvas;
    [SerializeField] private ProximityPrompt _proximityPrompt;
    [SerializeField] private GameObject _dialogueRunnerHolder;
    [SerializeField] private string _startNode;
    [SerializeField] private MouseCursorUtil _mouseCursorUtil;
    [SerializeField] private GlobalServices _globalServices;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _npcPosition;

    

    public void PlayerAskToTalk()
    {   
        HandlePlayerMovedAndLookingAtPlace();
        //_globalServices.MoveAndLookPlayerTo(_playerPosition,_npcPosition);

        //_globalServices.PlayerMovedAndLookingAtPlace += HandlePlayerMovedAndLookingAtPlace;
        
    }

    public void TalkingFinished()
    {
        _canvas.sortingOrder = 0;
        _proximityPrompt.gameObject.SetActive(true);
        _mouseCursorUtil.HideCursor();

         //_globalServices.DisableNavAgent();
         _globalServices.EnablePlayerThirdPerson();
        
    }


    private void HandlePlayerMovedAndLookingAtPlace()
    {
        _canvas.sortingOrder = 10;
        _globalServices.DisablePlayerThirdPerson();
        _proximityPrompt.gameObject.SetActive(false);
        _dialogueRunnerHolder.GetComponent<DialogueRunner>().StartDialogue(_startNode);
        Debug.Log("_ShowCursor_");
        _mouseCursorUtil.ShowCursor();

        //_globalServices.PlayerMovedAndLookingAtPlace -= HandlePlayerMovedAndLookingAtPlace;
    }
}
