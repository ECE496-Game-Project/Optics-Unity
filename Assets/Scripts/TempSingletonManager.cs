
using CommonUtils;
using GO_Wave;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSingletonManager : MonoSingleton<TempSingletonManager>
{
    public SelectionController m_selectionController;
    public MouseInput m_mouseInput;

    [SerializeField] private PlayerInput m_playerInput;

    [Header("Rendering")]
    public LineWaveSampleMaterialController m_lineWaveSampleMaterialController;

    protected override void Init()
    {
        base.Init();

        m_mouseInput = new MouseInput(m_playerInput);
        m_selectionController = new SelectionController(m_mouseInput);
    }


}