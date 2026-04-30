using UnityEngine;
using UnityEngine.InputSystem;

public class TMNewPlusUltra : MonoBehaviour
{
    private IUseMode[] modes;
    private IUseMode currentMode;
    private GameObject selectedTower;


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

    AllInputAction.buildAction.Enable();
    AllInputAction.buildAction.performed += OnBuildPressed;

    AllInputAction.sellAction.Enable();
    AllInputAction.sellAction.performed += OnSellPressed;

    AllInputAction.upgradeAction.Enable();
    AllInputAction.upgradeAction.performed += OnUpgradePressed;
}

    void OnDisable()
    {
        AllInputAction.leftClickAction.performed -= OnLeftClick;
        AllInputAction.leftClickAction.Disable();

        AllInputAction.rightClickAction.performed -= OnRightClick;
        AllInputAction.rightClickAction.Disable();

        AllInputAction.buildAction.performed -= OnBuildPressed;
        AllInputAction.buildAction.Disable();

        AllInputAction.sellAction.performed -= OnSellPressed;
        AllInputAction.sellAction.Disable();

        AllInputAction.upgradeAction.performed -= OnUpgradePressed;
        AllInputAction.upgradeAction.Disable();
    }
    private void OnRightClick(InputAction.CallbackContext ctx)
    {
        ExitMode();
        selectedTower = null;
    }
    void Update()
    {
        currentMode?.PrewMode();
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

    private void OnBuildPressed(InputAction.CallbackContext ctx)
    {
        SetBuildMode();
    }

    private void OnSellPressed(InputAction.CallbackContext ctx)
    {
        SetSellMode();
    }

    private void OnUpgradePressed(InputAction.CallbackContext ctx)
    {
        SetUpgradeMode();
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