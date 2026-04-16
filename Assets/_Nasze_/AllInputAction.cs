using UnityEngine.InputSystem;

public static class AllInputAction
{
    private static readonly InputSystem_Actions _actionsAsset = new InputSystem_Actions();

    public static InputAction leftClickAction = FindOrCreate("Gameplay/LeftClick", "<Mouse>/leftButton");
    public static InputAction rightClickAction = FindOrCreate("Gameplay/RightClick", "<Mouse>/rightButton");
    public static InputAction escClickAction = FindOrCreate("Gameplay/Pause", "<Keyboard>/escape");
    public static InputAction buildModeAction = FindOrCreate("Gameplay/BuildMode", "<Keyboard>/digit1");
    public static InputAction sellModeAction = FindOrCreate("Gameplay/SellMode", "<Keyboard>/digit2");
    public static InputAction upgradeModeAction = FindOrCreate("Gameplay/UpgradeMode", "<Keyboard>/digit3");
    public static InputAction selectModeAction = FindOrCreate("Gameplay/SelectMode", "<Keyboard>/digit4");

    static AllInputAction()
    {
        if (buildModeAction.actionMap == null)
        {
            buildModeAction.AddBinding("<Keyboard>/numpad1");
            buildModeAction.AddBinding("<Keyboard>/b");
        }

        if (sellModeAction.actionMap == null)
        {
            sellModeAction.AddBinding("<Keyboard>/numpad2");
            sellModeAction.AddBinding("<Keyboard>/v");
        }

        if (upgradeModeAction.actionMap == null)
        {
            upgradeModeAction.AddBinding("<Keyboard>/numpad3");
            upgradeModeAction.AddBinding("<Keyboard>/u");
        }

        if (selectModeAction.actionMap == null)
        {
            selectModeAction.AddBinding("<Keyboard>/numpad4");
            selectModeAction.AddBinding("<Keyboard>/q");
        }
    }

    private static InputAction FindOrCreate(string actionPath, string fallbackBinding)
    {
        InputAction action = _actionsAsset.asset.FindAction(actionPath, throwIfNotFound: false);
        if (action != null)
        {
            return action;
        }

        return new InputAction(type: InputActionType.Button, binding: fallbackBinding);
    }

    public static void Enable()
    {
        leftClickAction.Enable();
        rightClickAction.Enable();
        escClickAction.Enable();
        buildModeAction.Enable();
        sellModeAction.Enable();
        upgradeModeAction.Enable();
        selectModeAction.Enable();
    }

    public static void Disable()
    {
        leftClickAction.Disable();
        rightClickAction.Disable();
        escClickAction.Disable();
        buildModeAction.Disable();
        sellModeAction.Disable();
        upgradeModeAction.Disable();
        selectModeAction.Disable();
    }
}
