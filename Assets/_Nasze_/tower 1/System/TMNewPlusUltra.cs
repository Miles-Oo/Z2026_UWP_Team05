using UnityEngine;
using UnityEngine.InputSystem;

public class TMNewPlusUltra : MonoBehaviour
{
    private IUseMode[] modes;
    private IUseMode currentMode;

    [Header("Input")]
    [SerializeField] private InputAction leftClickAction;   
     [SerializeField] private InputAction rightClickAction;


    void Start()
    {
        modes = GetComponents<IUseMode>();
        currentMode=null;
    }

void OnEnable()
{
    leftClickAction.Enable();
    leftClickAction.performed += OnLeftClick;

    rightClickAction.Enable();
    rightClickAction.performed += OnRightClick;
}

void OnDisable()
{
    leftClickAction.performed -= OnLeftClick;
    leftClickAction.Disable();

    rightClickAction.performed -= OnRightClick;
    rightClickAction.Disable();
}
private void OnRightClick(InputAction.CallbackContext ctx)
{
    ExitMode();
}
    void Update()
    {
        if (currentMode != null)
        {
            currentMode.PrewMode();
        }
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (currentMode == null)
        {
            return;
        }

        if (currentMode.ActionMode())
        {
            ExitMode();
        }
    }

    public void SetMode(Mode mode)
    {
        ExitMode();

        foreach (var m in modes)
        {
            if (m.GetMode() == mode)
            {
                currentMode = m;
                currentMode.EnterMode();
                break;
            }
        }
    }

    public void SetBuildMode() => SetMode(Mode.BUILD);
    public void SetSellMode() => SetMode(Mode.SELL);
    public void SetUpgradeMode() => SetMode(Mode.UPGRADE);
    public void SetIdleMode() => ExitMode();

    public void ExitMode()
    {
        if (currentMode != null)
        {
            currentMode.ExitMode();
            currentMode = null;
        }
    }
}