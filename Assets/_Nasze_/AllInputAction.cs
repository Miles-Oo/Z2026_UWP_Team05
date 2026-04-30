using UnityEngine.InputSystem;
public static class AllInputAction
{
    public static InputAction leftClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
    public static InputAction rightClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/rightButton");
    public static InputAction escClickAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");

    private static InputSystem_Actions inputActions;

    public static InputAction buildAction;
    public static InputAction sellAction;
    public static InputAction upgradeAction;
    public static void Init()
    {
        inputActions = new InputSystem_Actions();

        buildAction = inputActions.GamePlay.Build;
        sellAction = inputActions.GamePlay.Sell;
        upgradeAction = inputActions.GamePlay.Upgrade;
    }

    public static void Enable()
    {
        leftClickAction.Enable();
        rightClickAction.Enable();
        escClickAction.Enable();
        inputActions?.GamePlay.Enable();
    }

    public static void Disable()
    {
        leftClickAction.Disable();
        rightClickAction.Disable();
        escClickAction.Disable();
        inputActions?.GamePlay.Disable();
    }
}