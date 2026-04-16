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

    AllInputAction.buildModeAction.Enable();
    AllInputAction.buildModeAction.performed += OnBuildMode;

    AllInputAction.sellModeAction.Enable();
    AllInputAction.sellModeAction.performed += OnSellMode;

    AllInputAction.upgradeModeAction.Enable();
    AllInputAction.upgradeModeAction.performed += OnUpgradeMode;

    AllInputAction.selectModeAction.Enable();
    AllInputAction.selectModeAction.performed += OnSelectMode;
}

void OnDisable()
{
    AllInputAction.leftClickAction.performed -= OnLeftClick;
    AllInputAction.leftClickAction.Disable();

    AllInputAction.rightClickAction.performed -= OnRightClick;
    AllInputAction.rightClickAction.Disable();

    AllInputAction.buildModeAction.performed -= OnBuildMode;
    AllInputAction.buildModeAction.Disable();

    AllInputAction.sellModeAction.performed -= OnSellMode;
    AllInputAction.sellModeAction.Disable();

    AllInputAction.upgradeModeAction.performed -= OnUpgradeMode;
    AllInputAction.upgradeModeAction.Disable();

    AllInputAction.selectModeAction.performed -= OnSelectMode;
    AllInputAction.selectModeAction.Disable();
}
private void OnRightClick(InputAction.CallbackContext ctx)
{
    ExitMode();
}
private void OnBuildMode(InputAction.CallbackContext ctx)
{
    SetBuildMode();
}
private void OnSellMode(InputAction.CallbackContext ctx)
{
    SetSellMode();
}
private void OnUpgradeMode(InputAction.CallbackContext ctx)
{
    SetUpgradeMode();
}
private void OnSelectMode(InputAction.CallbackContext ctx)
{
    SetMode(Mode.SELECT);
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
