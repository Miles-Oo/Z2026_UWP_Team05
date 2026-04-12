using UnityEngine.InputSystem;
public static class AllInputAction
{
    public static InputAction leftClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
    public static InputAction rightClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/rightButton");
    public static InputAction escClickAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");

    public static void Enable()
    {
        leftClickAction.Enable();
        rightClickAction.Enable();
        escClickAction.Enable();
    }

    public static void Disable()
    {
        leftClickAction.Disable();
        rightClickAction.Disable();
        escClickAction.Disable();
    }
}