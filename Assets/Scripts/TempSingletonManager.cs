
using CommonUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSingletonManager : MonoSingleton<TempSingletonManager>
{
    public SelectionController m_selectionController;
    public MouseInput m_mouseInput;

    [SerializeField] private PlayerInput m_playerInput;
    protected override void Init()
    {
        base.Init();

        m_mouseInput = new MouseInput(m_playerInput);
        m_selectionController = new SelectionController(m_mouseInput);
    }


}