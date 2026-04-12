using UnityEngine;
using UnityEngine.InputSystem;

public class TMNewPlusUltra : MonoBehaviour
{
    private IUseMode[] modes;
    private IUseMode currentMode;


    void Start()
    {
        modes = GetComponents<IUseMode>();
        currentMode=null;
    }

void OnEnable()
{
   AllInputAction.leftClickAction.Enable();
    AllInputAction.leftClickAction.performed += OnLeftClick;

    AllInputAction.rightClickAction.Enable();
    AllInputAction.rightClickAction.performed += OnRightClick;
}

void OnDisable()
{
    AllInputAction.leftClickAction.performed -= OnLeftClick;
    AllInputAction.leftClickAction.Disable();

    AllInputAction.rightClickAction.performed -= OnRightClick;
    AllInputAction.rightClickAction.Disable();
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
        SetMode(Mode.SELECT);
        currentMode?.ActionMode();
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