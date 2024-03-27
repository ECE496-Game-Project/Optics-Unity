using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class TutorialController : InputController
{
    public override string m_name => "TutorialController";
    private static TutorialController m_instance=null;
    public static TutorialController Instance => m_instance;

    private PlayerInput m_playerInput;

    public bool isInput {
        get {
            bool temp = _isInput;
            if(_isInput) _isInput = false;
            return temp;
        }
    }

    private bool _isInput = false;

    public TutorialController(InputController parent, PlayerInput playerInput): base(parent)
    {
        Assert.IsNull(m_instance);
        m_instance = this;
        m_playerInput = playerInput;

        m_playerInput.actions["TutorialContinue"].started += OnPressEnter;
        m_playerInput.actions["TutorialContinue"].performed += OnMouseLeftClick;
        m_playerInput.actions["TutorialContinue"].canceled += OnMouseFinished;
    }

    private void OnMouseLeftClick(InputAction.CallbackContext context){
        if (!m_isAllowed) return;
        _isInput = true;
    }

    private void OnPressEnter(InputAction.CallbackContext context){
        if (!m_isAllowed) return;
        _isInput = true;
        NotifyMyParentIsOn();
    }

    public void OnMouseFinished(InputAction.CallbackContext context){
        if (!m_isAllowed) return;
        _isInput = false;
        NotifyMyParentIsFinished();
    }
}
